using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace MISmain
{
    public static class Network
    {
        public const int INF = int.MaxValue;
        public static int numNodes, numEdges;
        public static byte[,] adjMatrix;

        public static void ReadAdjacencyMatrix(string fileName)
        {
            try
            {
                using (TextReader reader = File.OpenText(fileName))
                {
                    int i, j;

                    numNodes = int.Parse(reader.ReadLine());
                    adjMatrix = new byte[numNodes, numNodes];


                    for (i = 0; i < numNodes; ++i)
                    {
                        string line = reader.ReadLine();
                        string[] weights = line.Split(' ');

                        for (j = 0; j < numNodes; ++j)
                        {
                            adjMatrix[i, j] = byte.Parse(weights[j]);
                        }
                    }

                }

            }
            catch (IOException e)
            {
                Console.WriteLine(e.GetType().Name);
            }

        }

        public static void RandomAdjacencyMatrix(int seed)
        {
            Random randObj = new Random(seed);
            adjMatrix = new byte[numNodes, numNodes];
            for (int i = 0; i < numNodes; i++)
                for (int j = i + 1; j < numNodes; j++)
                    adjMatrix[i, j] = adjMatrix[j, i] = (byte)randObj.Next(2);


            for (int i = 0; i < numNodes; i++)
                adjMatrix[i, i] = 0;

        }

        public static void ReadDIMACS(string fileName)
        {
            try
            {
                using (TextReader reader = File.OpenText(fileName))
                {
                    string line;
                    string[] fields;
                    while ((line = reader.ReadLine()) != null)
                    {
                        fields = Regex.Split(line, @"\s+");
                        if (fields[0] == "p")
                            if (int.TryParse(fields[2], out numNodes) && int.TryParse(fields[3], out numEdges))
                            {
                                adjMatrix = new byte[numNodes, numNodes];
                                break;
                            }
                    }

                    while ((line = reader.ReadLine()) != null)
                    {
                        int i, j;
                        fields = Regex.Split(line, @"\s+");
                        if (fields[0] == "e")
                        {
                            int.TryParse(fields[1], out i);
                            int.TryParse(fields[2], out j);
                            adjMatrix[i-1, j-1] = adjMatrix[j-1, i-1] = 1;//zero based array
                        }
                    }

                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.GetType().Name);
            }
        }


        public static void SaveAdjacencyMatrix(string fileName)
        {
            try
            {
                using (TextWriter writer = File.CreateText(fileName))
                {
                    int i, j;
                    
                    writer.WriteLine(numNodes);
                    for (i=0; i< numNodes; i++)
                    {
                        String line = "";
                        for (j=0; j< numNodes; j++)
                        {
                            line += adjMatrix[i,j].ToString();
                            if(j < (numNodes-1))
                               line +=" ";
                        }
                        writer.WriteLine(line);
                    }
                    writer.Close();
                }
            }

            catch (IOException e)
            {
                Console.WriteLine(e.GetType().Name);
            }


        }

        public static void SaveGraphViz(string fileName)
        {
            try
            {
                using (TextWriter writer = File.CreateText(fileName))
                {
                    int i, j;

                    writer.WriteLine("graph G {");
                    writer.WriteLine("node [color = deepskyblue2, shape=circle];");

                    for (i = 0; i < numNodes; i++)
                    {
                        String line = "";
                        for (j = i+1; j < numNodes; j++)
                            if (adjMatrix[i, j] > 0)
                            {
                                line = String.Format("  {0} -- {1};", i, j);
                                writer.WriteLine(line);
                            }
                        
                    }
                    writer.WriteLine("}");
                    writer.Close();
                }
            }

            catch (IOException e)
            {
                Console.WriteLine(e.GetType().Name);
            }


        }


        public static void Clean()
        {
            adjMatrix = null;
        }
    }
}
