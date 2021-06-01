using GeometricFigures.Figures;
using System;
using System.Collections.Generic;

namespace GeometricFigures
{
    class Program
    {
        private static string[] figureNames = { "Ellipsoid", "Rectangle", "Triangle", "Trapezoid" };
        private static readonly Dictionary<string, Func<Figure>> figures = new Dictionary<string, Func<Figure>>()
        {
                { "1", () => { return ConsoleFigureBuilder.BuildEllipoid(); } },
                { "2", () => { return ConsoleFigureBuilder.BuildRectangle(); } },
                { "3", () => { return ConsoleFigureBuilder.BuildTriangle(); } },
                { "4", () => { return ConsoleFigureBuilder.BuildTrapezoid(); } }
            };

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                for (int i = 0; i < figureNames.Length; i++)
                {
                    Console.WriteLine($"{i + 1} : {figureNames[i]}");
                }

                Console.WriteLine("Enter number which equals to the number of the figure: ");

                try
                {
                    DisplayPerimeterAndSquareForFigure(figures[Console.ReadLine()].Invoke());
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("Entered number was null. Only numbers are legal. Please, try again.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("It was impossible to convert entered number. Maybe you used non digit symbol. Please, try again.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Entered number was too big. Use scientific notation for such cases. Please, try again.");
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("Given figure key is not present in the collection. Please, try again.");
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
        }

        static void DisplayPerimeterAndSquareForFigure(Figure figure)
        {
            if (double.IsNaN(figure.GetSquare()))
            {
                Console.WriteLine("Such figure is not existing.");
            }
            else
            {
                Console.WriteLine($"Square: {figure.GetSquare()}. Perimeter: {figure.GetPerimeter()}.");
            }
        }
    }
}
