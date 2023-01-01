

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
    //ParEnd,
}



internal class Node
{
    enum Type
    {
        Operation,
        ValueContainerFunc,
        ValueContainer
    }

    private Node? _node1;
    private Node? _node2;

    private Type _type;

    private Operation _operation;
    private ValueContainer _value;

    /// <summary>
    /// Opertation Node
    /// </summary>
    /// <param name="inNode1"></param>
    /// <param name="inNode2"></param>
    public Node(Node? inNode1, Node? inNode2, Operation operation)
    {
        _type = Type.Operation;

        if (inNode1 == null)
            throw new ArgumentNullException(nameof(inNode1));
        if (inNode2 == null)
            throw new ArgumentNullException(nameof(inNode2));

        _node1 = inNode1;
        _node2 = inNode2;
        _operation = operation;
    }

    /// <summary>
    /// Value node function
    /// </summary>
    public Node(ValueContainer value)
    {
        _type = Type.ValueContainer;

        _value = value;
    }

    public double GetValue(double x)
    {
        if (_type == Type.Operation)
        {
            if (_operation == Operation.Add)
                return _node1!.GetValue(x) + _node2!.GetValue(x);
            if (_operation == Operation.Sub)
                return _node1!.GetValue(x) - _node2!.GetValue(x);
            if (_operation == Operation.Mul)
                return _node1!.GetValue(x) * _node2!.GetValue(x);
            if (_operation == Operation.Div)
                return _node1!.GetValue(x) / _node2!.GetValue(x);
        }
        else if (_type == Type.ValueContainer)
        {
            return _value!.GetValue(x);
        }
    }
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
        type = Type.Varible;
        Value = null;
        Function = null;
    }

    public void SetValue(double Val)
    {
        type = Type.Value;
        Value = Val;
        Function = null;
    }

    public void SetValue(Func<double, double> Func)
    {
        type = Type.Func;
        Value = null;
        Function = Func;
    }

    public double GetValue(double x)
    {
        switch (type)
        {
            case Type.Varible:
                return x;
                break;
            case Type.Value:
                return Value.Value;
                break;
            case Type.Func:
                return Function.Invoke(x);
                break;
            default:
                throw new NotImplementedException();
        }
    }
}

internal class Function
{
    private Dictionary<string, Func<double, double>> _BuildInFunctions = new Dictionary<string, Func<double, double>>()
    {
        { "sin", x => Math.Sin(x) },
        { "cos", x => Math.Cos(x) },
        { "tan", x => Math.Tan(x) },
        { "sqrt", x => Math.Sqrt(x) },
    };

    private char[] OperationsAllowed = { '+', '-', '*', '/' };

    internal string Name;
    internal ValueContainer[] values = new ValueContainer[0];
    internal Operation[] operations = new Operation[0];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strFunction"></param>
    /// <param name="funcs"></param>
    public Function(string strFunction, Func<double, double> funcs)
    {
        SetFunction(strFunction, funcs);
    }

    public double Call(double X)
    {
        double Value = 0;
        throw new Exception();
    }


    /// <summary>
    /// The strFunction will have other varibles like "aa", so for each non (number and x) the next func will be put in.
    /// Format example: f(x) = 1 + sin(x) * 0.3 / 3 - x * customFunc(4) :end [funcs.Count == 1]
    /// </summary>
    /// <param name="strFunction"></param>
    /// <param name="funcs"></param>
    public void SetFunction(string strFunction, Func<double, double> funcs)
    {
        Operation GetOperationFromChar(char c)
        {
            switch (c)
            {
                case '+':
                    return Operation.Add;
                case '-':
                    return Operation.Sub;
                case '*':
                    return Operation.Mul;
                case '/':
                    return Operation.Div;
                default:
                    throw new NotImplementedException();
            }
        }
        string ParseName(string str)
        {
            int lastIndex = 0;
            for (int i = 0; i < str.Length; i++)
                if (str[i] != '(')
                    lastIndex++;
                else
                    break;
            if (lastIndex == 0)
                throw new Exception("There have not been found any name");
            if (lastIndex == str.Length - 1)
                throw new Exception("Invalid function found in ParseName");
            Name = str.Take(lastIndex).ToString();
            return str.Take(new Range(lastIndex, str.Length - 1)).ToString();
        }
        string GetExpresion(string str)
        {
            return str.Split('=')[1];
        }
        string RemoveSpaces(string str)
        {
            return str.TakeWhile(x => x != ' ').ToString();
        }

        strFunction = ParseName(strFunction);
        strFunction = GetExpresion(strFunction);
        strFunction = RemoveSpaces(strFunction);

        List<string> tokens = new List<string>();

        /*
         * Tokens will be the numbers such as "3N" or "0.44N" or "4242n"
         * functions will be split into two such as "sinF", "x" or "customFuncF", "4N"
         * and inbetween the numbers and the functions(pairs) there will be an
         * operation token such as "*O" or "-O" 
         * 
         * N = number
         * F = function
         * x = varible: can only be the input varible
         * O = operation
         */

        // Tokenize
        string token = "";
        for (int i = 0; i < strFunction.Length; i++)
        {
            if (strFunction[i] == 'x' && token == "")
            {
                tokens.Add("x");
            }
            else if (char.IsDigit(strFunction[i]))
            {
                token += strFunction[i];
            }
            else if (strFunction[i] == '.')
            {
                token += '.';
            }
            else if (OperationsAllowed.Contains(strFunction[i]))
            {
                if (token != "")
                {
                    if (double.TryParse(token, out double d))
                    {
                        token += "N";
                        tokens.Add(token);
                    }
                    else
                    {
                        token += "F";
                        tokens.Add(token);
                    }
                }

                tokens.Add(GetOperationFromChar(strFunction[i]).ToString() + "O");
                token = "";
            }
            else // Is function
            {
                token += strFunction[i];
            }
        }

        // Now unwrap the functions "sin(x)" -> "sin", "x"
        for (int i = 0; i < tokens.Count(); i++)
        {
            string str = tokens[i];
            tokens.RemoveAt(i);
            string[] newTokens = str.Split('(');
            tokens.Insert(i, newTokens[0]); // sin
            tokens.Insert(++i, newTokens[1].Split(')')[0]); // x
        }



        // Now tokenized

        /* 
         * Now we try and put into a hierarchy
         * Example:
         * 1 + sin(x) * 0.3 / 3 - x * customFunc(4) 
         * =
         *              
         *              
         *              
         *                "-"
         *                / \___________
         *               /              \
         *             "+"              "*"
         *             / \              / \
         *            /   \            /   \
         *          "1"   "/"        "x"  "Delegate"
         *                / \                  \
         *               /   \                  \
         *             "*"   "3"                "4"
         *             / \
         *            /   \
         *         "sin" "0.3"
         *          /
         *         /
         *       "x" 
         * 
         */



    }
}
