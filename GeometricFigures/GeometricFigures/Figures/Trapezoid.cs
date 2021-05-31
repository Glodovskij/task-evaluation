using System;

namespace GeometricFigures.Figures
{
    class Trapezoid : Figure
    {
        private double _a;
        private double _b;
        private double _c;
        private double _d;
        private double _h; // Высота.
        public override void GetFigureParameters()
        {
            Console.Write("Enter a base of the trapezoid: ");
            _a = double.Parse(Console.ReadLine());

            Console.Write("Enter b base of the trapezoid: ");
            _b = double.Parse(Console.ReadLine());

            Console.Write("Enter c side of the trapezoid: ");
            _a = double.Parse(Console.ReadLine());

            Console.Write("Enter d side of the trapezoid: ");
            _b = double.Parse(Console.ReadLine());

            Console.Write("Enter h heihgt of the trapezoid: ");
            _h = double.Parse(Console.ReadLine());
        }

        public override double GetPerimeter()
        {
            return _a + _b + _c + _d;
        }

        public override double GetSquare()
        {
            return .5 * (_a + _b) * _h;
        }
    }
}
