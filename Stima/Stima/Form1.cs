﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Edge = Microsoft.Msagl.Drawing.Edge;
using Node = Microsoft.Msagl.Drawing.Node;
using Point = Microsoft.Msagl.Core.Geometry.Point;
using Rectangle = Microsoft.Msagl.Core.Geometry.Rectangle;
using GeomEdge = Microsoft.Msagl.Core.Layout.Edge;

namespace Stima
{
    public partial class Form1 : Form
    {
        int[,] array;
        int nEdges;
        int n;
        int[] antrian;
        int caraJalan, neffJalur, neffAntrian;
        int tmptJose, tmptFerdiand;
        int[] jalur;
        int[,] data;
        int[] dikunjungi;
        bool jalanAja;
        Microsoft.Msagl.Drawing.Graph graph;

        public Form1()
        {
            int i, j;
            ReadGraph();
            InitializeComponent();
            //create a graph object 
           graph = new Microsoft.Msagl.Drawing.Graph("graph");

            for (j = 0; j < nEdges; j++)
            {
                graph.AddEdge(Convert.ToString(array[j, 0]), Convert.ToString(array[j, 1])).Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
            }
            //bind the graph to the viewer 
            gViewer.Graph = graph;

            
            
            neffAntrian = 0;
            neffJalur = 0;
            antrian = new int[n + 1];
            jalur = new int[n + 1];
            dikunjungi = new int[n];
            data = new int[n, n];
            for (i = 0; i < n + 1; i++)
            {
                antrian[i] = 0;
                jalur[i] = 0;
            }
            for (i = 0; i < n; i++)
            {
                dikunjungi[i] = 0;
            }

            for(int a=0; a<nEdges; a++)
            {
                i = array[a,0];
                j = array[a,1];
                data[i - 1, j - 1] = 1;
                data[j - 1, i - 1] = 1;
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           //Milik InitializeComponent() jangan didelete

        }

        private void buttonsolve_Click()
        {
            //action untuk button solve
            int a = Convert.ToInt32(textBox1.Text);
            int x = Convert.ToInt32(textBox2.Text)-1;
            int y = Convert.ToInt32(textBox3.Text)-1;

            
            //Nampilin jawaban
            if (Solusi(a, x, y))
            {
                textBox4.Text = "Ya";
            }
            else
            {
                textBox4.Text = "Tidak";
            }
        }
        

        private void solveEksternal(object sender, EventArgs e)
        {
            //action untuk solve Eksternal
            //mengambil query dari file eksternal dan menampilkannya

            TextReader tr = new StreamReader("query.txt");
            int a, x, y;
            int nquery = Convert.ToInt32(tr.ReadLine());
            Queries.Items.Clear();
            Answers.Items.Clear();
            System.IO.File.WriteAllText(@"answer.txt", "");

            string s;
            while ((s = tr.ReadLine()) != null)
            {
                string[] strtemp = s.Split(' ');
                string jawaban;
                a = Convert.ToInt32(strtemp[0]);
                x = Convert.ToInt32(strtemp[1]);
                y = Convert.ToInt32(strtemp[2]);

                //insert dfs
                if(Solusi(a, x-1, y-1))
                {
                    jawaban = "YA";
                }
                else
                {
                    jawaban = "TIDAK";
                }

                Answers.Items.Add(jawaban);
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"answer.txt", true))
                {
                    file.WriteLine(jawaban);
                }


                // Nampilin query
                Queries.Items.Add(s);

            }
            tr.Close();
        }
        

        public void resetVariabel() {
            //reset warna dari graph

            for(int i=0; i<n+1; i++)
            {
                jalur[i] = 0;
            }
            neffJalur = 0;
            for (int i = 0; i < n; i++) {
                dikunjungi[i] = 0;
            }
        }

        public void ReadGraph()
        {
            //membaca graph dari file eksternal  

            TextReader tr = new StreamReader("input.txt");
            int i = 0;

            n = Convert.ToInt32(tr.ReadLine());
            nEdges = n - 1;
            array = new int[nEdges, 2];

            string s;
            while ((s = tr.ReadLine()) != null)
            {
                string[] strtemp = s.Split(' ');
                array[i, 0] = Convert.ToInt32(strtemp[0]);
                array[i, 1] = Convert.ToInt32(strtemp[1]);
                i++;
            }
            tr.Close();
        }

        public Boolean Solusi(int a,int b,int c)
        {
            //Mencari solusi melalui DFS

            caraJalan = a;
            tmptJose = b;
            tmptFerdiand = c;
            jalanAja = false;
            DFS(0);
            resetVariabel();
            return jalanAja;
        }

        public int GetSimpul(int i, int j)
        {
            //Getter Simpul
            return data[i, j];
        }

        public int xSudahKunjungi(int x)
        {
            //Getter array dikunjungi
            return dikunjungi[x];
        }

        public void SetDikunjungi(int x)
        {
            //Setter array dikunjungi
            dikunjungi[x] = 1;
        }

        public void SetJalur(int value)
        {
            //Setter array jalur
            jalur[neffJalur] = value;
            neffJalur++;
        }

        public void ResetJalur(int value)
        {
            //Untuk backtrack jalur
            neffJalur = value;
        }

        public bool xIsDaun(int x)
        {
            //Mengecek apakah suatu simpul daun atau tidak
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

        public void DFS(int x)
        {
            //Algoritma DFS
            if (jalanAja == false)
            {
                SetDikunjungi(x);
                SetJalur(x + 1);
                setTetangganX(x);
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            //Action Bila Form1 di click
            buttonsolve_Click();
        }

        private void Clean_Click(object sender, EventArgs e)
        {
            //Action bila button clear di click
            for (int i = 0; i < n; i++)
            {
                graph.FindNode(Convert.ToString(i + 1)).Attr.FillColor = Microsoft.Msagl.Drawing.Color.White;
            }
            gViewer.Graph = graph;
        }

        private void Queries_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Mencari solusi dari sebuah query yang ditaruh pada file eksternal

            String[] query = (Convert.ToString(Queries.SelectedItem)).Split(' ') ;
            int a = Convert.ToInt32(query[0]);
            int x = Convert.ToInt32(query[1]);
            int y = Convert.ToInt32(query[2]);

            Boolean tem =Solusi(a, x - 1, y - 1) ;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"answer.txt", true))
                {
                    file.WriteLine(textBox4.Text);
                }
            }
        }

        public void cekJalur()
        {
            //Mengecek jalur dfs, dan apakah ditemukan tempat jose dan ferdinant
            //Sekaligus mengeprint jalur ke viewer

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
                        jalanAja = true;
                        for (int i = 0; i < neffJalur; i++)
                        {
                            graph.FindNode(Convert.ToString(jalur[i])).Attr.FillColor = Microsoft.Msagl.Drawing.Color.MistyRose;
                        }
                        gViewer.Graph = graph;
                    }
                }
                else
                {
                    if (place2 < place1)
                    {
                        jalanAja = true;
                        for (int i = 0; i < neffJalur ; i++)
                        {
                            graph.FindNode(Convert.ToString(jalur[i])).Attr.FillColor = Microsoft.Msagl.Drawing.Color.MistyRose;
                        }
                        gViewer.Graph = graph;
                    }
                }
            }
        }

        public void PrintJalur()
        {
            //Mengeprint jalur ke terminal

            for (int i = 0; i < neffJalur; i++)
            {
                Console.Write(jalur[i]);
                Console.Write(" ");
            }
            Console.WriteLine();
        }

        public void setTetangganX(int x)
        {
            //Mengecek tetangga dari suatu simpul

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
