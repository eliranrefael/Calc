using Autofac;
using System.Text.RegularExpressions;
using Calc.app_code.Model;
using Autofac.Core;

public interface ICalcFlowManager
{
    public double Calc(string equetion);
}

public interface IEquationPriorityManager
{
    public int[] GetSubequestionIndexes(string equation);
}

public interface ICalcOperator
{
    public double Operate (double x, double y);
}

public interface IEquationParser
{
    public ParsedEquationSimple Parse(string equation);
}

public class Multiply: ICalcOperator
{
    public double Operate (double x, double y)
    {
        return x * y;
    }
}

public class Substract : ICalcOperator
{
    public double Operate(double x, double y)
    {
        return x - y;
    }
}

public class Add : ICalcOperator
{
    public double Operate(double x, double y)
    {
        return x + y;
    }
}

public class Divide : ICalcOperator
{
    public double Operate(double x, double y)
    {
        if(y == 0)
        {
            throw new InvalidOperationException("Cannot divide in zero");
        }

        return Math.Round(x / y, 2);
    }
}

public class CalcFlowManagerSimple: ICalcFlowManager
{
    private readonly IEquationParser _parser;
    private readonly IComponentContext _context;
    public CalcFlowManagerSimple(IEquationParser parser, IComponentContext context)
    {
        _parser = parser;
        _context = context;
    }

    public double Calc(string equation)
    {
        ParsedEquationSimple result = _parser.Parse(equation);
        var calcOperator = _context.ResolveKeyed<ICalcOperator>(result.Operator);
        return calcOperator.Operate(result.FirstOperand, result.SecondOperand);
    }
}

public class CalcFlowManager : ICalcFlowManager
{
    private readonly IComponentContext _context;
    private readonly ICalcFlowManager _calcFlowManagerSimple;
    private readonly IEquationPriorityManager _equationPriorityManager;
    public CalcFlowManager(IComponentContext context, IEquationPriorityManager equationPriorityManager)
    {
        _context = context;
        _equationPriorityManager = equationPriorityManager;
        _calcFlowManagerSimple = _context.ResolveKeyed<ICalcFlowManager>("Simple");
    }

    public double Calc(string equetion)
    {
        var parsedEquetion = equetion;
        Regex regex = new Regex("([-]*[0-9.]+)[^0-9.]+([0-9.]+)");
        var isOperationExist = regex.Match(parsedEquetion).Success;

        while (isOperationExist)
        {
            var substringIndexes = _equationPriorityManager.GetSubequestionIndexes(parsedEquetion);
            var substring = parsedEquetion.Substring(substringIndexes[0], substringIndexes[1]);
            var result = _calcFlowManagerSimple.Calc(substring);
            parsedEquetion = parsedEquetion.Remove(substringIndexes[0], substringIndexes[1]);
            parsedEquetion = parsedEquetion.Insert(substringIndexes[0], result.ToString());
            var match = regex.Match(parsedEquetion);
            isOperationExist = match.Success && match.Groups.Count > 1;
        }

        return Double.Parse(parsedEquetion);
    }
}

public class EquationPriorityManager : IEquationPriorityManager
{
    public int[] GetSubequestionIndexes(string equetion)
    {
        int[] result;
        var regex = new Regex("[-]*[0-9.]+[/X]+[0-9.]+");
        var match = regex.Match(equetion);
        if (!match.Success)
        {
            regex = new Regex("[-]*[0-9.]+[^0-9.]+[0-9.]+");
            match = regex.Match(equetion);
        }

        var startIndex = match.Index;
        var length = match.Length;
        result = new int[] { startIndex, length };
        return result;
    }
}

public class EquationParser: IEquationParser
{
    public ParsedEquationSimple Parse(string equation)
    {
        Regex regex = new Regex("([-]*[0-9.]+)([^0-9.]+)([0-9.]+)");
        Match match = regex.Match(equation);
        if (!match.Success)
        {
            throw new InvalidOperationException("Invalid equation format");
        }

        double firstOperand = Double.Parse(match.Groups[1].Value);
        double secondOperand = Double.Parse(match.Groups[3].Value);
         
        return new ParsedEquationSimple(match.Groups[2].Value, firstOperand, secondOperand);
    }
}
