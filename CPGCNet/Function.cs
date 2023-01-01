

using System.Xml.Serialization;

namespace CPGCNet;

internal enum Operation
{
    /// <summary>
    /// +
    /// </summary>
    Add,

    /// <summary>
    /// -
    /// </summary>
    Sub,

    /// <summary>
    /// *
    /// </summary>
    Mul,

    /// <summary>
    /// /
    /// </summary>
    Div,

    ///// <summary>
    ///// (
    ///// </summary>
    //ParStart,

    ///// <summary>
    ///// )
    ///// </summary>
    //ParEbd,
}

internal class ValueContainer
{
    internal enum Type
    {
        /// <summary>
        /// This is where x will be set in
        /// </summary>
        Varible,

        /// <summary>
        /// This is just any abatrary double value
        /// </summary>
        Value,

        /// <summary>
        /// Value is the given Functions return value
        /// </summary>
        Func,
    }

    internal Type type;
    internal double? Value;
    internal Func<double, double>? Function;

    public void SetValue()
    {

    }

    public void SetValue(double Val)
    {

    }

    public void SetValue(Func<double, double> Func)
    {

    }
}

internal class Function
{


    /// <summary>
    /// 
    /// </summary>
    /// <param name="Values">All the Values that will be used</param>
    /// <param name="Operations">Will have one less then, execpt</param>
    public Function(List<ValueContainer> Values, List<Operation> Operations)
    {
        SetFunction(Values, Operations);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Values">All the Values that will be used</param>
    /// <param name="Operations">Will have one less then, execpt</param>
    public void SetFunction(List<ValueContainer> Values, List<Operation> Operations)
    {

    }

    public double Call(float X)
    {
        throw new NotImplementedException();
    }
}
