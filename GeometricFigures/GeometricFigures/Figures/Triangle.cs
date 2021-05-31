using System;

namespace GeometricFigures.Figures
{
    public class Triangle : Figure
    {
        private double _a;
        private double _b;
        private double _c;
        public override void GetFigureParameters()
        {
            Console.Write("Enter a side of the triangle: ");
            _a = double.Parse(Console.ReadLine());

            Console.Write("Enter b side of the triangle: ");
            _b = double.Parse(Console.ReadLine());

            Console.Write("Enter c side of the triangle: ");
            _c = double.Parse(Console.ReadLine());
        }

        public override double GetPerimeter()
        {
            return _a + _b + _c;
        }

        public override double GetSquare()
        {
            double halfP = (_a + _b + _c) / 2; // Формула Герона, полупериметр.
            return Math.Sqrt(halfP * (halfP - _a) * (halfP - _b) * (halfP - _c));
        }
    }
}
