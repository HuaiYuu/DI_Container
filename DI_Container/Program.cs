using Microsoft.Extensions.DependencyInjection;
using System;

public interface ICalculator
{
    int Add(int a, int b);
    int Subtract(int a, int b);
}

public class SimpleCalculator : ICalculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }

    public int Subtract(int a, int b)
    {
        return a - b;
    }
}

public class AdvancedCalculator : ICalculator
{
    public int Add(int a, int b)
    {        
        return 10*(a + b);
    }

    public int Subtract(int a, int b)
    {       
        return 10*(a - b);
    }
}

public class CalculatorController
{
    private readonly ICalculator _calculator;
    private string _calculatorType;

    public CalculatorController(ICalculator calculator)
    {
        _calculator = calculator;
        _calculatorType = "Simple";
    }

    public string CalculatorType
    {
        get { return _calculatorType; }
        set { _calculatorType = value; }
    }

    public void Calculate(int a, int b)
    {
        int result;
        if (_calculatorType.ToLower()== "+")
        {
            result = _calculator.Add(a, b);
        }
        else if (_calculatorType.ToLower() == "-")
        {
            result = _calculator.Subtract(a, b);
        }
        else
        {
            throw new ArgumentException("+ OR -");
        }

        Console.WriteLine($"Result: {result}");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();        
        while (true)
        {
            Console.WriteLine("enter calculator type ('Simple' or 'Advanced'): ");
            string calculatorTyped = Console.ReadLine();
            if (calculatorTyped.ToLower() == "simple")
            {
            services.AddTransient<ICalculator, SimpleCalculator>();
            Console.WriteLine("SimpleCalculator + => a+b, - => :a-b");
            }
            else
            {
            services.AddTransient<ICalculator, AdvancedCalculator>();
            Console.WriteLine("AdvancedCalculator + => 10*(a+b), - =>10(a-b)");
            }
            services.AddTransient<CalculatorController>();
            var serviceProvider = services.BuildServiceProvider();
            var calculatorController = serviceProvider.GetRequiredService<CalculatorController>();
            Console.WriteLine("Please enter the calculator type ('+' or '-'): ");
            string calculatorType = Console.ReadLine();

            if (calculatorType.ToLower() == "+" || calculatorType.ToLower() == "-")
            {
                calculatorController.CalculatorType = calculatorType;
            }
            else
            {
                
                continue;
            }

            
            int a = 5;
            int b = 1;

            Console.WriteLine("a=5,b=1");

            calculatorController.Calculate(a, b);

            Console.WriteLine("enter 'exit' to quite");
            string input = Console.ReadLine();
            if (input.ToLower() == "exit")
            {
                break;
            }
        }
    }
}