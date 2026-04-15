namespace MathToolCalling;

public static class MathFunctions
{
    public static double Add(double a, double b)
    {
        return a + b;
    }

    public static double Multiply(double a, double b)
    {
        return a * b;
    }

    public static long Factorial(int n)
    {
        long result = 1;
        for (var i = 2; i <= n; i++) result *= i;
        return result;
    }
}