namespace Benday.CalculatorDemo.Api; public class Calculator : ICalculator
{
    public int Add(int value1, int value2)
    {
        return value1 + value2;
    }

    public int Subtract(int value1, int value2)
    {
        return value1 - value2;
    }

    public int Multiply(int value1, int value2)
    {
        return value1 * value2;
    }

    public int Divide(int value1, int value2)
    {
        return value1 / value2;
    }
}