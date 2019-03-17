using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text;

using Edge = Microsoft.Msagl.Drawing.Edge;
using Node = Microsoft.Msagl.Drawing.Node;
using Point = Microsoft.Msagl.Core.Geometry.Point;
using Rectangle = Microsoft.Msagl.Core.Geometry.Rectangle;
using GeomEdge = Microsoft.Msagl.Core.Layout.Edge;

class ViewerSample
{
    public static void Main()
    {
        TextReader tr = new StreamReader("input.txt");
        int n;
        int i = 0;

        n = Convert.ToInt32(tr.ReadLine());
        Console.WriteLine(n);
        int[,] array = new int[n, 2];

        string s;
        while ((s= tr.ReadLine()) != null)
        {
            string[] strtemp = s.Split(' ');
            array[i, 0] = Convert.ToInt32(strtemp[0]);
            array[i, 1] = Convert.ToInt32(strtemp[1]);
            i++;
        }
        tr.Close();

        /*
        for(int j=0;j<n;j++)
        {
            Console.Write(array[j, 0]);
            Console.Write(" ");
            Console.Write(array[j, 1]);
            Console.WriteLine("");
        }
        */

        Console.ReadKey();

        
        
        //create a form 
        System.Windows.Forms.Form form = new System.Windows.Forms.Form();
        //create a viewer object 
        Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
        //create a graph object 
        Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
        graph.Directed = false;
        



        for (int j = 0; j < n; j++)
        {
            graph.AddEdge(Convert.ToString(array[j, 0]), Convert.ToString(array[j, 1])).Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
        }

        //bind the graph to the viewer 
        viewer.Graph = graph;
        //associate the viewer with the form 
        form.SuspendLayout();
        viewer.Dock = System.Windows.Forms.DockStyle.Fill;
        form.Controls.Add(viewer);
        form.ResumeLayout();
        //show the form 
        form.ShowDialog();
        

        /*
        //create the graph content 
        graph.AddEdge("A", "B");
        graph.AddEdge("B", "C");
        graph.AddEdge("A", "C").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
        graph.FindNode("A").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
        graph.FindNode("B").Attr.FillColor = Microsoft.Msagl.Drawing.Color.MistyRose;
        Microsoft.Msagl.Drawing.Node c = graph.FindNode("C");
        c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleGreen;
        c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
        */
        

    }
}