using Autofac;

public class AutofacContainerConfig: Module
{

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EquationParser>().As<IEquationParser>();
        builder.RegisterType<CalcFlowManagerSimple>().Keyed<ICalcFlowManager>("Simple");
        builder.RegisterType<CalcFlowManager>().As<ICalcFlowManager>();
        builder.RegisterType<EquationPriorityManager>().As<IEquationPriorityManager>();
        builder.RegisterType<Multiply>().Keyed<ICalcOperator>("X");
        builder.RegisterType<Divide>().Keyed<ICalcOperator>("/");
        builder.RegisterType<Add>().Keyed<ICalcOperator>("+");
        builder.RegisterType<Substract>().Keyed<ICalcOperator>("-");
    }
}
