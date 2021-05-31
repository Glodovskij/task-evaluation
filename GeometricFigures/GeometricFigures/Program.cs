using GeometricFigures.Figures;
using System;

namespace GeometricFigures
{
    class Program
    {
        private static string[] figureNames = { "Ellipsoid", "Rectangle", "Triangle", "Trapezoid" };

        static void Main(string[] args)
        {
            while(true)
            {
                Console.Clear();
                for(int i = 0; i < figureNames.Length; i ++)
                {
                    Console.WriteLine($"{i + 1} : {figureNames[i]}");
                }

                Console.WriteLine("Enter number which equals to the number of the figure: "); 
                
                switch (Console.ReadLine())
                {
                    case "1":
                        GetPerimeterAndSquareByEnteredValues(new Ellipsoid());
                        break;
                    case "2":
                        GetPerimeterAndSquareByEnteredValues(new Rectangle());
                        break;
                    case "3":
                        GetPerimeterAndSquareByEnteredValues(new Triangle());
                        break;
                    case "4":
                        GetPerimeterAndSquareByEnteredValues(new Trapezoid());
                        break;
                    default:
                        Console.WriteLine("Not a number of a figure was entered...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void GetPerimeterAndSquareByEnteredValues(Figure figure)
        {
            try
            {
                figure.GetFigureParameters();
                if (double.IsNaN(figure.GetSquare()))
                {
                    Console.WriteLine("Such figure is not existing.");
                }
                else
                {
                    Console.WriteLine($"Square: {figure.GetSquare()}. Perimeter: {figure.GetPerimeter()}.");
                }
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
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
