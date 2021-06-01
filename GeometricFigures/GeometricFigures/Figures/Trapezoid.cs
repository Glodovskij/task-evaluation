namespace GeometricFigures.Figures
{
    class Trapezoid : Figure
    {
        private double _a;
        private double _b;
        private double _c;
        private double _d;
        private double _h; // Высота.

        public Trapezoid(double a, double b, double c, double d, double h)
        {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
            _h = h;
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
