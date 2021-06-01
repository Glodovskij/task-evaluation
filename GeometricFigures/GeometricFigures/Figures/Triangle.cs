using System;

namespace GeometricFigures.Figures
{
    public class Triangle : Figure
    {
        private double _a;
        private double _b;
        private double _c;


        public Triangle(double a, double b, double c)
        {
            _a = a;
            _b = b;
            _c = c;
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
