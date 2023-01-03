// CommandPromptGraphingCalculator

/*
 * Ideas:
 * Implement "()"
 * Use = to see if it is a function decleration
 * Implement it so you can see what goes wrong and where
 * Use polymothy for the nodes to simplefy
 * Multi argumental functions
 * Operators ! ^
 * Better const/varible expersions
 * Add debuging util 
 * Change the node func to use another rather that a ValueContainor
 */

using CPGCNet;
using SlackingGameEngine.Rendering;

string help_message()
{
    string help_string = @"
CLEAR: Clears all functions and display.
HELP:  Displays this help message.
USAGE: Displays usage of the program.
EXIT:  Exits the program.
Functions:
    sin
    cos
    tan
    sqrt
    log
    abs
    square, sqr 
    PI
    E
";
    return help_string;
}

string usage_message()
{
    string usage_string = @"
>>> f(x) = x+2
>>> f(2)
f(2) = 4
";
    return usage_string;
}

Dictionary<string, Function> Functions = new Dictionary<string, Function>();
List<(string Name, Func<double, double> func)> funcList = new List<(string Name, Func<double, double> func)>();

while (true)
{
    // CLI prompt 
    Console.Write(">>> ");
    string? Input = Console.ReadLine();
    // prints help if no command given
    if (Input == "")
    {
        Console.WriteLine(help_message());
    }
    // clears functions and prompt screen
    if (Input?.ToUpper() == "CLEAR")
    {
        Functions.Clear();
        funcList.Clear();
        Console.Clear();
        continue;
    }
    if (Input?.ToUpper() == "HELP")
    {
        Console.WriteLine(help_message());
        continue;
    }
    // USAGE command
    if (Input?.ToUpper() == "USAGE")
    {
        Console.WriteLine(usage_message());
        continue;
    }
    // EXIT command
    if (Input?.ToUpper() == "EXIT")
    {
        Environment.Exit(0);
    }
    try
    {
        // replaces ',' with '.' 
        for (int i = 0; i < Input?.Length; i++)
            Input = Input.Replace('.', ',');
        if (double.TryParse(Input?.Split('(')[1].Split(')')[0], out double num)) // Name(10) // call function
        {
            try
            {
                if (Functions.ContainsKey(Input))
                    continue;
                Function func = Functions[Input.Split('(')[0]];
                Console.WriteLine(func.Name + $"({num}) = " + func.Call(num));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        else // try save the function
        {
            try
            {
                Function function = new Function(Input ?? "", funcList.ToArray(), Color.Red); // input is empty if null
                Functions.Add(function.Name, function);
                funcList.Add((function.Name, (x) => function.Call(x)));
                continue;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}
