using GeometricFigures.Figures;
using System;


namespace GeometricFigures
{
    public static class ConsoleFigureBuilder
    {
        public static Figure BuildEllipoid()
        {
            double semiMinor, semiMajor;
            Console.Write("Enter semi-minor axis: ");
            semiMinor = double.Parse(Console.ReadLine());

            Console.Write("Enter semi-major axis: ");
            semiMajor = double.Parse(Console.ReadLine());

            return new Ellipsoid(semiMinor, semiMajor);
        }

        public static Figure BuildRectangle()
        {
            double a, b;
            Console.Write("Enter a side of the rectangle: ");
            a = double.Parse(Console.ReadLine());

            Console.Write("Enter b side of the rectangle: ");
            b = double.Parse(Console.ReadLine());
            return new Rectangle(a, b);
        }

        public static Figure BuildTrapezoid()
        {
            double a, b, c, d, h;
            Console.Write("Enter a base of the trapezoid: ");
            a = double.Parse(Console.ReadLine());

            Console.Write("Enter b base of the trapezoid: ");
            b = double.Parse(Console.ReadLine());

            Console.Write("Enter c side of the trapezoid: ");
            c = double.Parse(Console.ReadLine());

            Console.Write("Enter d side of the trapezoid: ");
            d = double.Parse(Console.ReadLine());

            Console.Write("Enter h heihgt of the trapezoid: ");
            h = double.Parse(Console.ReadLine());

            return new Trapezoid(a, b, c, d, h);
        }

        public static Figure BuildTriangle()
        {
            double a, b, c;
            Console.Write("Enter a side of the triangle: ");
            a = double.Parse(Console.ReadLine());

            Console.Write("Enter b side of the triangle: ");
            b = double.Parse(Console.ReadLine());

            Console.Write("Enter c side of the triangle: ");
            c = double.Parse(Console.ReadLine());

            return new Triangle(a, b, c);
        }
    }
}
