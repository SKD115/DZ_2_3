using System;

public class ComplexNumber
{
    public double Real { get; }
    public double Imagine { get; }

    // конструктор
    public ComplexNumber(double real, double imagine)
    {
        Real = real;
        Imagine = imagine;
    }

    // конструктор для обычных чисел (без мнимой части)
    public ComplexNumber(double real)
    {
        Real = real;
        Imagine = 0;
    }

    // вывод в строку
    public override string ToString()
    {
        if (Imagine == 0)
        {
            return Real.ToString("0.##");
        }

        if (Real == 0)
        {
            if (Imagine < 0)
                return Imagine.ToString("0.##") + "i";
            else
                return Imagine.ToString("0.##") + "i";
        }

        if (Imagine > 0)
        {
            return Real.ToString("0.##") + " + " + Imagine.ToString("0.##") + "i";
        }
        else
        {
            return Real.ToString("0.##") + " - " + (-Imagine).ToString("0.##") + "i";
        }
    }

    // +
    public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
    {
        return new ComplexNumber(a.Real + b.Real, a.Imagine + b.Imagine);
    }

    // -
    public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b)
    {
        return new ComplexNumber(a.Real - b.Real, a.Imagine - b.Imagine);
    }

    // *
    public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
    {
        double newReal = a.Real * b.Real - a.Imagine * b.Imagine;
        double newImagine = a.Real * b.Imagine + a.Imagine * b.Real;
        return new ComplexNumber(newReal, newImagine);
    }

    // %
    public static ComplexNumber operator /(ComplexNumber a, ComplexNumber b)
    {
        if (b.Real == 0 && b.Imagine == 0)
        {
            throw new DivideByZeroException("Не дели на нуль");
        }

        double den = b.Real * b.Real + b.Imagine * b.Imagine;
        double newReal = (a.Real * b.Real + a.Imagine * b.Imagine) / den;
        double newImagine = (a.Imagine * b.Real - a.Real * b.Imagine) / den;

        return new ComplexNumber(newReal, newImagine);
    }

    // ==
    public static bool operator ==(ComplexNumber a, ComplexNumber b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;

        return Math.Abs(a.Real - b.Real) < 0.0000001 && Math.Abs(a.Imagine - b.Imagine) < 0.0000001;
    }

    // !=
    public static bool operator !=(ComplexNumber a, ComplexNumber b)
    {
        return !(a == b);
    }

    // унарный +
    public static double operator +(ComplexNumber z)
    {
        return Math.Sqrt(z.Real * z.Real + z.Imagine * z.Imagine);
    }

    // унарный — 
    public static ComplexNumber operator -(ComplexNumber z)
    {
        return new ComplexNumber(z.Real, -z.Imagine);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("ComplexNumber\n");

        ComplexNumber z1 = new ComplexNumber(3, 4);
        ComplexNumber z2 = new ComplexNumber(1, -2);
        ComplexNumber z3 = new ComplexNumber(5);
        ComplexNumber z4 = new ComplexNumber(3, 4);

        Console.WriteLine("z1 = " + z1);
        Console.WriteLine("z2 = " + z2);
        Console.WriteLine("z3 = " + z3);
        Console.WriteLine("z4 = " + z4);
        Console.WriteLine();

        Console.WriteLine("z1 + z2 = " + (z1 + z2));
        Console.WriteLine("z1 * z2 = " + (z1 * z2));
        Console.WriteLine("z1 / z2 = " + (z1 / z2));
        Console.WriteLine();

        Console.WriteLine("Унарный + z1 = " + (+z1));
        Console.WriteLine("Унарный - к z1 = " + (-z1));
        Console.WriteLine();

        Console.WriteLine("z1 == z4: " + (z1 == z4));
        Console.WriteLine("z1 != z4: " + (z1 != z4));
        Console.WriteLine();

        Console.WriteLine("ToString");
        Console.WriteLine(new ComplexNumber(0, 7));
        Console.WriteLine(new ComplexNumber(-1.5, 0));

        try
        {
            var zero = z1 / new ComplexNumber(0, 0);
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }
}