namespace Calc.app_code.Model
{
    public class ParsedEquationSimple
    {
        public string Operator { get; set; }
        public double FirstOperand { get; set; }
        public double SecondOperand { get; set; }

        public ParsedEquationSimple(string @operator, double firstOperand, double secondOperand)
        {
            Operator = @operator;
            FirstOperand = firstOperand;
            SecondOperand = secondOperand;
        }
    }
}
