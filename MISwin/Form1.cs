using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MISmain
{
    public partial class Form1 : Form
    {
        BMA bma;
        DepthFirst df;
        VSA vs;
        GreedyMin grmin;
        GreedyMax grmax;
        GreedyC1 gc1;

        //string path;
        //string fileName;

        public Form1()
        {
            InitializeComponent();
            bma = new BMA();
            df = new DepthFirst();
            vs = new VSA();
            grmin = new GreedyMin();
            grmax = new GreedyMax();
            gc1 = new GreedyC1();
        }

        public void RunHeuristics()
        {
            if (!chkBFOnly.Checked)
            {
                vs.Initialize();
                vs.MaxIndSet();

                grmin.Initialize();
                grmin.MaxIndSet();

                grmax.Initialize();
                grmax.MaxIndSet();

                gc1.Initialize();
                gc1.MaxIndSet();
            }

            bma.Initialize();
            bma.MaxIndSet();
        }

        public void RunDepth()
        {
            df.Initialize();
            df.MaxIndSet();
        }

        public void DisplayDepth()
        {
            textBox1.Text += "DepthFirst MaxIndxSet() Result:";
            textBox1.Text += Environment.NewLine;
            textBox1.Text += "Path = ";
            foreach (int i in df.path)
                textBox1.Text += String.Format(" {0}", i);

            textBox1.Text += Environment.NewLine;
            textBox1.Text += "Cost = " + df.maxSet.cost.ToString() + Environment.NewLine;
            textBox1.Text += df.elapsedTime.ToString() + Environment.NewLine;
            textBox1.Text += Environment.NewLine;

        }

        public void DisplayResults()
        {
            textBox1.Text += Environment.NewLine;
            textBox1.Text += "BMA MaxIndSet() Result:";
            textBox1.Text += Environment.NewLine;
            textBox1.Text += "Path = ";
            foreach (int i in bma.path)
                textBox1.Text += String.Format(" {0}", i);
            textBox1.Text += Environment.NewLine;
            textBox1.Text += "Cost = " + bma.cost.ToString() + Environment.NewLine;
            textBox1.Text += bma.elapsedTime.ToString() + Environment.NewLine;
            textBox1.Text += Environment.NewLine;

            if (!chkBFOnly.Checked)
            {
                textBox1.Text += "GreedyMin MaxIndxSet() Result:";
                textBox1.Text += Environment.NewLine;
                textBox1.Text += "Path = ";
                foreach (int i in grmin.path)
                    textBox1.Text += String.Format(" {0}", i);
                textBox1.Text += Environment.NewLine;
                textBox1.Text += "Cost = " + grmin.path.Count.ToString() + Environment.NewLine;
                textBox1.Text += grmin.elapsedTime.ToString() + Environment.NewLine;
                textBox1.Text += Environment.NewLine;

                textBox1.Text += "GreedyMax MaxIndxSet() Result:";
                textBox1.Text += Environment.NewLine;
                textBox1.Text += "Path = ";
                foreach (int i in grmax.path)
                    textBox1.Text += String.Format(" {0}", i);
                textBox1.Text += Environment.NewLine;
                textBox1.Text += "Cost = " + grmax.path.Count.ToString() + Environment.NewLine;
                textBox1.Text += grmax.elapsedTime.ToString() + Environment.NewLine;
                textBox1.Text += Environment.NewLine;

                textBox1.Text += "GreedyC1 MaxIndxSet() Result:";
                textBox1.Text += Environment.NewLine;
                textBox1.Text += "Path = ";
                foreach (int i in gc1.path)
                    textBox1.Text += String.Format(" {0}", i);
                textBox1.Text += Environment.NewLine;
                textBox1.Text += "Cost = " + gc1.path.Count.ToString() + Environment.NewLine;
                textBox1.Text += gc1.elapsedTime.ToString() + Environment.NewLine;
                textBox1.Text += Environment.NewLine;

                textBox1.Text += "VSA MaxIndxSet() Result:";
                textBox1.Text += Environment.NewLine;
                textBox1.Text += "Path = ";
                foreach (int i in vs.path)
                    textBox1.Text += String.Format(" {0}", i);
                textBox1.Text += Environment.NewLine;
                textBox1.Text += "Cost = " + vs.path.Count.ToString() + Environment.NewLine;
                textBox1.Text += vs.elapsedTime.ToString() + Environment.NewLine;
                textBox1.Text += Environment.NewLine;

            }
            //path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            //fileName = string.Format("{0:yyyy-MM-dd-HH-mm-ss}", DateTime.Now);
            //Network.SaveAdjacencyMatrix(path + "\\" + fileName + ".txt");
            //Network.SaveGraphViz(path + "\\" + fileName + ".gv");
            //try
            //{
            //    using (TextWriter writer = File.CreateText(path + "\\" + fileName + "-results.txt"))
            //    {
            //        writer.Write(textBox1.Text);
            //        writer.Close();
            //    }
                
            //}
            //catch (IOException e)
            //{
            //    textBox1.Text += e.GetType().Name;
            //}

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Network.ReadAdjacencyMatrix(openFileDialog1.FileName);
                RunHeuristics();
                DisplayResults();
            }
        }

        private void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int randomSeed = (int)numericUpDown2.Value;
            Network.numNodes = (int) numericUpDown1.Value;
            Network.RandomAdjacencyMatrix(randomSeed);

            RunHeuristics();
            DisplayResults();
        }

        private void convertDIMACSFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "COL Files (*.col)|*.col|B Files (.b)|*.b";
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Network.ReadDIMACS(openFileDialog1.FileName);
                Network.SaveAdjacencyMatrix(openFileDialog1.FileName + ".txt");
            }


        }

        private void openDIMACSFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "COL Files (*.col)|*.col|B Files (.b)|*.b";
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Network.ReadDIMACS(openFileDialog1.FileName);
                RunHeuristics();
                DisplayResults();
            }

        }
    }
}
