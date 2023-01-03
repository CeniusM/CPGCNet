using SlackingGameEngine.Rendering;
using System.Text;

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
    private ValueContainer? _value;

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
        throw new NotImplementedException();
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
        /// This is where -x will be set in
        /// </summary>
        negVarible,

        /// <summary>
        /// This is just any abatrary double value
        /// </summary>
        Value,

        /// <summary>
        /// Value is the given Functions return value
        /// </summary>
        Func,
    }

    private Type type;
    private double? Value;
    private Func<double, double>? Function;
    private ValueContainer? FuncVal;

    public void SetValue(bool negativ = false)
    {
        if (negativ)
            type = Type.negVarible;
        else
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

    public void SetValue(Func<double, double> Func, ValueContainer funcVal)
    {
        type = Type.Func;
        Value = null;
        Function = Func;
        this.FuncVal = funcVal;
    }

    public double GetValue(double x)
    {
        switch (type)
        {
            case Type.Varible:
                return x;
            case Type.negVarible:
                return -x;
            case Type.Value:
                return Value.Value;
            case Type.Func:
                return Function.Invoke(FuncVal.GetValue(x));
            default:
                throw new NotImplementedException();
        }
    }
}

internal class Function
{
    private static readonly Dictionary<string, Func<double, double>> _BuildInFunctions = new Dictionary<string, Func<double, double>>()
    {
        { "sin", x => Math.Sin(x) },
        { "cos", x => Math.Cos(x) },
        { "tan", x => Math.Tan(x) },
        { "sqrt", x => Math.Sqrt(x) },
        { "floor", x => Math.Floor(x) },
        { "log", x => Math.Log(x) },
        { "abs", x => Math.Abs(x) },
        { "sqr", x => x * x},
        { "PI", x => Math.PI },
        { "E", x => Math.E },
    };
    public static bool FunctionAllreadyBuildIn(string functionName) => _BuildInFunctions.ContainsKey(functionName);

    private char[] OperationsAllowed = { '+', '-', '*', '/' };

    internal Color color;
    internal string Name;
    private Node node;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strFunction"></param>
    /// <param name="Funcs"></param>
    public Function(string strFunction, (string FuncName, Func<double, double> Function)[] Funcs, Color color)
    {
        SetFunction(strFunction, Funcs);
        this.color = color;
    }

    public double Call(double X)
    {
        return node.GetValue(X);
    }


    /// <summary>
    /// The strFunction will have other varibles like "aa", so for each non (number and x) the next func will be put in.
    /// Format example: f(x) = 1 + sin(x) * 0.3 / 3 - x * customFunc(4) :end [Funcs.Length == 1]
    /// </summary>
    /// <param name="strFunction"></param>
    /// <param name="funcs"></param>
    public void SetFunction(string strFunction, (string FuncName, Func<double, double> Function)[] Funcs)
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
                    return (Operation)404;
            }
        }
        bool IsTokenFunction(string token)
        {
            if (token == "x")
                return false;
            if (double.TryParse(token, out double foo))
                return false;
            return true;
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
            Name = string.Join("", str.Take(lastIndex));
            return string.Join("", str.Take(new Range(lastIndex, str.Length)));
        }
        string GetExpresion(string str)
        {
            return str.Split('=')[1];
        }
        string RemoveSpaces(string str)
        {
            StringBuilder newString = new StringBuilder(str.Length);
            for (int i = 0; i < str.Length; i++)
                if (str[i] != ' ')
                    newString.Append(str[i]);
            return newString.ToString();
        }
        ValueContainer GetValueContainorFromtoken(string token)
        {
            ValueContainer val = new ValueContainer();
            if (token == "x")
            {
                val.SetValue();
            }
            else if (token == "-x")
            {
                val.SetValue(true);
            }
            else if (double.TryParse(token, out double foo))
            {
                val.SetValue(foo);
            }
            else if (_BuildInFunctions.ContainsKey(token))
            {
                val.SetValue(_BuildInFunctions[token], new ValueContainer());
            }
            return val;
        }
        List<string> GetTokensFromFunction(string strFunction)
        {
            List<string> tokens = new List<string>();

            // Tokenize
            string token = "";
            for (int i = 0; i < strFunction.Length; i++)
            {
                if (strFunction[i] == 'x' && token == "")
                {
                    token += "x";
                }
                else if (char.IsDigit(strFunction[i]))
                {
                    token += strFunction[i];
                }
                else if (strFunction[i] == ',' || strFunction[i] == '.')
                {
                    token += ',';
                }
                else if (strFunction[i] == '-' && token == "")
                {
                    token += "-";
                }
                else if (OperationsAllowed.Contains(strFunction[i]))
                {
                    if (token != "")
                    {
                        if (double.TryParse(token, out double d))
                        {
                            tokens.Add(token);
                        }
                        else
                        {
                            tokens.Add(token);
                        }
                    }

                    tokens.Add(new string(strFunction[i], 1));
                    token = "";
                }
                else // Is function
                {
                    token += strFunction[i];
                }
            }
            if (token != "")
                tokens.Add(token);

            // Now unwrap the functions "sin(x)" -> "sin", "x"
            for (int i = 0; i < tokens.Count(); i++)
            {
                string str = tokens[i];
                if (str.Contains('(') && str.Contains(')'))
                {
                    tokens.RemoveAt(i);
                    string[] newTokens = str.Split('(');
                    tokens.Insert(i, newTokens[0]); // sin
                    tokens.Insert(++i, newTokens[1].Split(')')[0]); // x
                }
            }

            return tokens;
        }
        Node GetNodeFromTokens(List<string> Tokens)
        {
            // You can call this with only the tokens in a pahrentesies and make a node from it

            /* 
             * Now we try and put into a hierarchy
             * Example:
             * 1 + sin(x) * 0.3 / 3 - x * customFunc(4) 
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

            // ValueContainer, either x, function or a value
            if (Tokens.Count == 1)
            {
                return new Node(GetValueContainorFromtoken(Tokens[0]));
            }
            // A function
            else if (Tokens.Count == 2)
            {
                ValueContainer func = new ValueContainer();
                ValueContainer funcVal = new ValueContainer();
                // Find the funtion
                bool FunctionFound = false;
                if (_BuildInFunctions.ContainsKey(Tokens[0]))
                {
                    funcVal = GetValueContainorFromtoken(Tokens[1]);
                    func.SetValue(_BuildInFunctions[Tokens[0]], funcVal);
                    FunctionFound = true;
                }
                else
                {
                    for (int i = 0; i < Funcs.Count(); i++)
                        if (Funcs[i].FuncName == Tokens[0])
                        {
                            funcVal = GetValueContainorFromtoken(Tokens[1]);
                            func.SetValue(Funcs[i].Function, funcVal);
                            FunctionFound = true;
                            break;
                        }
                }
                if (!FunctionFound)
                    throw new Exception("Were not able to find function: " + Tokens[0]);
                else
                    return new Node(func);
            }
            // Operator time
            else if (Tokens.Count == 3)
            {
                Operation operation = GetOperationFromChar(Tokens[1][0]);
                Node tempNode1 = GetNodeFromTokens(new List<string> { Tokens[0] });
                Node tempNode2 = GetNodeFromTokens(new List<string> { Tokens[2] });
                return new Node(tempNode1, tempNode2, operation);
            }

            // We go from right to left and slowly build up the nodes
            // We break down the right and left side and make them into node recursively
            Node? MainNode;
            for (int i = 0; i < Tokens.Count(); i++)
            {
                if (Operation.Add == GetOperationFromChar(Tokens[i][0]) && Tokens[i].Length == 1)
                {
                    MainNode = new Node(GetNodeFromTokens(Tokens.Take(new Range(0, i)).ToList()), // [0, i[
                        GetNodeFromTokens(Tokens.Take(new Range(i + 1, Tokens.Count())).ToList()), // ]i, endIndex]
                        Operation.Add);
                    return MainNode;
                }
                else if (Operation.Sub == GetOperationFromChar(Tokens[i][0]) && Tokens[i].Length == 1)
                {
                    MainNode = new Node(GetNodeFromTokens(Tokens.Take(new Range(0, i)).ToList()), // [0, i[
                        GetNodeFromTokens(Tokens.Take(new Range(i + 1, Tokens.Count())).ToList()), // ]i, endIndex]
                        Operation.Sub);
                    return MainNode;
                }
            }

            // After we do the same with * and /
            for (int i = 0; i < Tokens.Count(); i++)
            {
                if (Operation.Div == GetOperationFromChar(Tokens[i][0]) && Tokens[i].Length == 1)
                {
                    MainNode = new Node(GetNodeFromTokens(Tokens.Take(new Range(0, i)).ToList()), // [0, i[
                        GetNodeFromTokens(Tokens.Take(new Range(i + 1, Tokens.Count())).ToList()), // ]i, endIndex]
                        Operation.Div);
                    return MainNode;
                }
                else if (Operation.Mul == GetOperationFromChar(Tokens[i][0]) && Tokens[i].Length == 1)
                {
                    MainNode = new Node(GetNodeFromTokens(Tokens.Take(new Range(0, i)).ToList()), // [0, i[
                        GetNodeFromTokens(Tokens.Take(new Range(i + 1, Tokens.Count())).ToList()), // ]i, endIndex]
                        Operation.Mul);
                    return MainNode;
                }
            }

            throw new NotImplementedException();
        }

        strFunction = RemoveSpaces(strFunction);
        strFunction = ParseName(strFunction);
        if (_BuildInFunctions.ContainsKey(Name))
            throw new Exception("The function " + Name + " already exists");
        strFunction = GetExpresion(strFunction);
        var Tokens = GetTokensFromFunction(strFunction);
        node = GetNodeFromTokens(Tokens);
    }
}