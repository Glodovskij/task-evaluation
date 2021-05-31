using System;

namespace GeometricFigures.Figures
{
    public class Rectangle : Figure
    {
        private double _a;
        private double _b;

        public override void GetFigureParameters()
        {
            Console.Write("Enter a side of the rectangle: ");
            _a = double.Parse(Console.ReadLine());

            Console.Write("Enter b side of the rectangle: ");
            _b = double.Parse(Console.ReadLine());
        }

        public override double GetPerimeter()
        {
            return 2 * (_a + _b);
        }

        public override double GetSquare()
        {
            return _a * _b;
        }
    }
}
