using System;
using System.IO;

namespace Stima
{
    public class Program
    {
        static int n;
        static int[] antrian;
        static int caraJalan, neffJalur, neffAntrian;
        static int tmptJose, tmptFerdiand;
        static int[] jalur;
        static int[,] data;
        static int[] dikunjungi;
        static bool jalanAja;

        public static void Main()
        {
            TextReader tr = new StreamReader("input.txt");
            int i, j;

            n = Convert.ToInt32(tr.ReadLine());
            neffAntrian = 0;
            neffJalur = 0;
            antrian = new int[n+1];
            jalur = new int[n + 1];
            dikunjungi = new int[n];
            data = new int[n,n];
            for (i = 0; i < n+1; i++)
            {
                antrian[i] = 0;
                jalur[i] = 0;
            }
            for (i = 0; i < n; i++)
            {
                dikunjungi[i] = 0;
            }

            string s;
            while ((s = tr.ReadLine()) != null)
            {
                string[] strtemp = s.Split(' ');
                i = Convert.ToInt32(strtemp[0]);
                j = Convert.ToInt32(strtemp[1]);
                data[i - 1, j - 1] = 1;
                data[j - 1, i - 1] = 1;
            }
            tr.Close();

            tr = new StreamReader("tanya.txt");
            s = tr.ReadLine();
            String[] strtemp2 = s.Split(' ');

            caraJalan = Convert.ToInt32(strtemp2[0]);
            tmptJose = Convert.ToInt32(strtemp2[1])-1;
            tmptFerdiand = Convert.ToInt32(strtemp2[2])-1;
            setAntrian(0);
            jalanAja = false;
            DFS(0);

            
            Console.ReadKey();
        }

        public static int GetSimpul(int i, int j)
        {
            return data[i,j];
        }

        public static int xSudahKunjungi(int x)
        {
            return dikunjungi[x];
        }

        public static void SetDikunjungi(int x)
        {
            dikunjungi[x] = 1;
        }

        public static void SetJalur(int value)
        {
            jalur[neffJalur] = value;
            neffJalur++;
        }

        public static void setAntrian(int value)
        {
            antrian[neffAntrian] = value;
            neffAntrian++;
        }

        public static void ResetJalur(int value)
        {
            neffJalur = value;
        }

        public static void PrintJalur()
        {
            for (int i = 0; i < neffJalur; i++)
            {
                Console.Write(jalur[i]);
                Console.Write(" ");
            }
            Console.WriteLine();
        }

        public static bool xIsDaun(int x)
        {
            bool isDaun = true;
            for (int i = 0; i < n; i++)
            {
                if (GetSimpul(x, i) == 1 && dikunjungi[i] == 0)
                {
                    isDaun = false;
                    break;
                }
            }

            return isDaun;
        }

        public static void DFS(int x)
        {
            if (jalanAja == false)
            {
                SetDikunjungi(x);
                SetJalur(x + 1);
                setTetangganX(x);
            }
        }

        public static void cekJalur()
        {
            int place1 = -99;
            int place2 = -99;
            for (int i = 0; i < neffJalur; i++)
            {
                if (jalur[i] == (tmptJose + 1))
                {
                    place1 = i;
                }
                if (jalur[i] == (tmptFerdiand + 1))
                {
                    place2 = i;
                }
            }
            if (place1 != -99 && place2 != -99)
            {
                if (caraJalan == 0)
                {
                    if (place1 < place2)
                    {
                        PrintJalur();
                    }
                }
                else
                {
                    if (place2 < place1)
                    {
                        PrintJalur();
                    }
                }
            }
        }

        public static void setTetangganX(int x)
        {
            int v = neffJalur;
            bool isDaun = xIsDaun(x);
            if (caraJalan == 0)
            {
                if (isDaun || tmptFerdiand == x)
                {
                    cekJalur();
                }
                else
                {
                    if (jalanAja == false)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            if (GetSimpul(x, i) == 1 && xSudahKunjungi(i) == 0)
                            {
                                DFS(i);
                                ResetJalur(v);
                            }
                        }
                    }
                }
            }
            else
            {
                if (isDaun || tmptJose == x)
                {
                    cekJalur();
                }
                else
                {
                    if (jalanAja == false)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            if (GetSimpul(x, i) == 1 && xSudahKunjungi(i) == 0)
                            {
                                DFS(i);
                                ResetJalur(v);
                            }
                        }
                    }
                }
            }
        }

    }
}
