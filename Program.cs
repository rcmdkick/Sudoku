using System;

namespace Sudoku
{
    class Program
    {
        public static void AllapotFigyelo(int szint, object[] E)
        {
            Console.WriteLine("Szint: "+szint);
        }
        static void Main(string[] args)
        {
            Console.Title = "Sudoku";
            SudokuMegoldo asd = new SudokuMegoldo("SudokuTabla.txt");
            asd.Probalkozas += AllapotFigyelo;

            Console.WriteLine("Alap állapot:");
            asd.Megjelenites();
            Console.ReadLine();

            try
            {
                asd.Megoldas();
                asd.Megjelenites();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();

        }
    }
}
