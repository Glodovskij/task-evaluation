namespace GeometricFigures.Figures
{
    public class Rectangle : Figure
    {
        public Rectangle(double a, double b)
        {
            _a = a;
            _b = b;
        }

        private double _a;
        private double _b;

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
