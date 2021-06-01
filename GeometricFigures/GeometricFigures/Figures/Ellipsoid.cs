using System;

namespace GeometricFigures.Figures
{
    public class Ellipsoid : Figure
    {
        public Ellipsoid(double semiMinor, double semiMajor)
        {
            _semiMajor = semiMajor;
            _semiMinor = semiMinor;
        }

        private double _semiMinor; // Малая ось.
        private double _semiMajor; // Большая ось.

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
