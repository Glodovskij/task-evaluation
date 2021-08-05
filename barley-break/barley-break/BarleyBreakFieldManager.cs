using barley_break.ViewModels;
using System;

namespace barley_break
{
    public static class BarleyBreakFieldManager
    { 
        public static CellViewModel[,] Generate()
        {
            Random rnd = new Random();

            CellViewModel[,] field = new CellViewModel[4,4];

            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j< 4; j++)
                {
                    field[i, j] = new CellViewModel();
                    field[i, j].X = j;
                    field[i, j].Y = i;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4;)
                {
                    int value = rnd.Next(1, 16);
                    if(i == 3 && j == 3)
                    {
                        field[i, j].Value = 0;
                        j++;
                        continue;
                    }
                    if (!FieldContainsValue(field, value))
                    {
                        field[i, j].Value = value;
                        j++;
                    }
                }
            }
            return field;
        }

        public static bool CheckForSolving(CellViewModel[,] field)
        {
            if(field[3,3].Value != 0)
            {
                return false;
            }

            int a = 0;
            
            for(int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if(x == 3 && y == 3)
                    {
                        continue;
                    }
                    if(x == 0 && y == 0)
                    {
                        if(field[y, x].Value == 1)
                        {
                            a = 1;
                            continue;
                        }
                    }
                    if (field[y, x].Value - 1 != a)
                    {
                        return false;
                    }
                    a = field[y, x].Value;
                }
            }
            return true;
        }

        private static bool FieldContainsValue(CellViewModel[,] field, int value)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (field[i, j].Value == value)
                    {
                        return true;
                    }
                }
                
            }
            return false;
        }

    }
}
