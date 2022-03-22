using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sudoku
{
    abstract class Backtrack
    {
        protected int N;
        protected int[] M;
        protected object[,] R;
        protected abstract bool ft(int szint, object a);
        protected abstract bool fk(int szintA, object A,int szintB, object B);
        public delegate void AllapotFigyelo(int szint, object[] E);
        public event AllapotFigyelo Probalkozas;
        protected object[] Kereses()
        {
            bool van = false;
            object[] E = new object[N];
            //Probalkozas?.Invoke(1, E);
            Probal(0,ref van,ref E);
            if (van) return E;
            else throw new NincsMegoldasKivetel();
        }
        void Probal(int szint, ref bool van, ref object[] E)
        {
            int i = 0;
            while (!van&&i<M[szint])
            {
                if (ft(szint, E[szint]))
                {
                    if ((szint==0)|| fk(szint, E[szint], szint - 1, E[szint - 1])) 
                    {
                        E[szint] = R[szint,i];
                        Probalkozas?.Invoke(szint, E);
                        if (szint == N - 1)
                        {
                            van = true;
                        }
                        else
                        {
                            Probal(szint + 1, ref van, ref E);
                        }
                    }
                }
            }
            

        }
    }
    class Pozicio
    { 
        public int Sor { get; }
        public int Oszlop { get; }
        public bool Fix { get; }
        public object Ertek { get; set; }

        public Pozicio(int sor, int oszlop)
        {
            Sor = sor;
            Oszlop = oszlop;
            Fix = false;
        }

        public Pozicio(int sor, int oszlop, object ertek) : this(sor, oszlop)
        {
            Ertek = ertek;
            Fix = true;
        }
        public static bool Kizaroak(Pozicio a, Pozicio b)
        {
            bool x = false;
            if ( (a.Oszlop==b.Oszlop||a.Sor==b.Sor)
                && (a.Oszlop/3==b.Oszlop/3 && a.Sor/3==b.Sor/3) )
                /*ha azonos a két pozíció sora vagy oszlopa, 
                  illetve ha azonos 3x3 -as területen vannak.*/
            {
                x = true;
            }
            return x;
        }
    }
    class SudokuMegoldo : Backtrack
    {
        Pozicio[,] tabla = new Pozicio[9, 9];
        Pozicio[] fixMezok;
        Pozicio[] uresMezok;
        void TablaBetoltes(string file)
        {
            int uresek = 0;
            int fixek = 0;
            string[] f = File.ReadAllLines(file);
            for (int i = 0; i < f.Length; i++) // sor
            {
                for (int j = 0; j < f[i].Length; j++) // oszlop
                {
                    Pozicio x;
                    if (char.IsDigit(f[i][j])) // fix érték
                    {
                        x = new Pozicio(i, j, f[i][j]);
                        fixek++;
                    }
                    else // ures
                    {
                        x = new Pozicio(i, j);
                        uresek++;
                    }
                    tabla[i, j] = x;

                    
                }
            }
            fixMezok = new Pozicio[fixek];
            uresMezok = new Pozicio[uresek];
            fixek = 0;uresek = 0;
            for (int i = 0; i < f.Length; i++) // sor
            {
                for (int j = 0; j < f[i].Length; j++) // oszlop
                {
                    Pozicio x = tabla[i, j];
                    if (x.Fix) // fix
                    {
                        fixMezok[fixek++] = x;
                    }
                    else // ures
                    {
                        uresMezok[uresek++] = x;
                    }
                }
            }
            N = uresek;
            M = new int[N];
            for (int i = 0; i < N; i++)
            {
                M[i] = 9;
            }
            R = new object[N, 9];
            for (int i = 0; i < R.GetLength(0); i++)//sor
            {
                for (int j = 0; j < R.GetLength(1); j++)//oszlop
                {
                    R[i, j] = j + 1; // 1-9-ig számok
                }
            }
        }

        public SudokuMegoldo(string file): base()
        {
            TablaBetoltes(file);
        }
        public void Megoldas()
        {
            object[] E;
            try
            {
                E = Kereses();
                uresMezok = (Pozicio[]) E;
                    
            }
            catch (Exception )
            {

                throw;
            }
            
        }
        public void Megjelenites()
        {
            for (int i = 0; i < tabla.GetLength(0); i++)
            {
                for (int j = 0; j < tabla.GetLength(1); j++)
                {
                    
                    if (tabla[i,j].Fix)
                    {
                        Console.ForegroundColor = ConsoleColor.Green; // zöldek a fixek
                        Console.Write(tabla[i, j].Ertek);
                        Console.ResetColor();
                        
                    }
                    else
                    {
                        Console.Write(".");
                    }
                    
                }
                Console.WriteLine();
            }
        }
        protected override bool ft(int szint, object a)
        {
            int i = 0;
            bool kizarva = false;
            while (i<fixMezok.Length && !kizarva)
            {
                if (Pozicio.Kizaroak((Pozicio)a,fixMezok[i]))
                {
                    kizarva = true;
                }
                i++;
            }
            return !kizarva;
        }
        // fixxmezőkön végig nézi h az aktuális megoldás egyáltalában bele illhet-e a dologba...
        protected override bool fk(int szintA, object A, int szintB, object B)
        {
            bool kizarva = false;
            if (A.Equals(B)&&Pozicio.Kizaroak(uresMezok[szintA],uresMezok[szintB]))
            {
                kizarva = true;
            }
            return !kizarva;
        }
    }
}
