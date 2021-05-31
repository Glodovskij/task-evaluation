using System;

namespace GeometricFigures.Figures
{
    public class Ellipsoid : Figure
    {
        private double _semiMinor; // Малая ось.
        private double _semiMajor; // Большая ось.

        public override void GetFigureParameters()
        {
            Console.Write("Enter semi-minor axis: ");
            _semiMinor = double.Parse(Console.ReadLine());

            Console.Write("Enter semi-major axis: ");
            _semiMajor = double.Parse(Console.ReadLine());
        }

        public override double GetPerimeter()
        {
            return 2 * Math.PI * Math.Sqrt((Math.Pow(_semiMajor, 2) + Math.Pow(_semiMinor, 2)) / 8);
        }

        public override double GetSquare()
        {
            return Math.PI * _semiMajor * _semiMinor;
        }
    }
}
