using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace MISmain
{
    class Program
    {
        static void Main(string[] args)
        {

            BMA bma = new BMA();
            DepthFirst df = new DepthFirst();
            VSA vs = new VSA();
            GreedyMin grmin = new GreedyMin();
            GreedyMax grmax = new GreedyMax();
            GreedyC1 gc1 = new GreedyC1();

            string fileName = "";
            int randomSeed = 0;
            string param;

            bool isAdj = false;
            bool isDM = false;
            bool isRandom = false;
            bool isShowHeader = false;
            bool isDepth = false;

            foreach (string arg in args)
            {
                switch (arg.Substring(0, 2).ToUpper())
                {
                    case "/S": // Random - seed
                        param = arg.Substring(3);
                        int.TryParse(param, out randomSeed);
                        break;

                    case "/R": // Random - number of nodes
                        param = arg.Substring(3);
                        if (int.TryParse(param, out Network.numNodes))
                            isRandom = true;
                        break;

                    case "/H": // Show header
                        isShowHeader = true;
                        break;

                    case "/F": // Read AdjMatrix from file
                        fileName = arg.Substring(3);
                        if (File.Exists(fileName))
                            isAdj = true;
                        break;

                    case "/D": // Read DIMACS from file
                        fileName = arg.Substring(3);
                        if (File.Exists(fileName))
                            isDM = true;
                        break;

                    case "/P": // Do Depth algorithm instead of heuristics
                        isDepth = true;
                        break;


                    default:
                        Console.WriteLine("Invalid Parameters");
                        break;
                }
            }

            if (isAdj)
            {
                Network.ReadAdjacencyMatrix(fileName);
            }

            else if (isDM)
            {
                Network.ReadDIMACS(fileName);
            }

            else if (isRandom)
            {
                Network.RandomAdjacencyMatrix(randomSeed);
            }

            if (isShowHeader)
            {
                if (isDepth)
                {
                    Console.Write("Graph,Nodes,Edges,");
                    Console.Write("Depth,,,");
                    Console.WriteLine();
                    Console.Write(",,,");
                    Console.Write("MIS,NodeList,Time,");
                    Console.WriteLine();
                }
                else
                {
                    Console.Write("Graph,Nodes,Edges,");

                    Console.Write("BMA,,,");
                    Console.Write("Greedy-Min,,,");
                    Console.Write("Greedy-Max,,,");
                    Console.Write("Greedy-C1,,,");
                    Console.Write("VSA,,,");
                    Console.WriteLine();
                    Console.Write(",,,");
                    Console.Write("MIS,NodeList,Time,");
                    Console.Write("MIS,NodeList,Time,");
                    Console.Write("MIS,NodeList,Time,");
                    Console.Write("MIS,NodeList,Time,");
                    Console.Write("MIS,NodeList,Time,");
                    Console.WriteLine();

                }
            }


            if (!isAdj && !isDM && !isRandom)
            {
                // No input data
                return;
            }

            if (isDepth)
            {
                df.Initialize();
                df.MaxIndSet();
                Console.Write(string.Format("{0},{1},{2},", Path.GetFileNameWithoutExtension(fileName), Network.numNodes, Network.numEdges));
                Console.Write(string.Format("{0},{1},{2},", df.path.Count, String.Join(" ", df.path.ToArray()), df.elapsedTime.TotalMilliseconds));
                Console.WriteLine();
            }

            else
            {
                vs.Initialize();
                vs.MaxIndSet();
                grmin.Initialize();
                grmin.MaxIndSet();

                grmax.Initialize();
                grmax.MaxIndSet();

                gc1.Initialize();
                gc1.MaxIndSet();

                bma.Initialize();
                bma.MaxIndSet();

                Console.Write(string.Format("{0},{1},{2},", Path.GetFileNameWithoutExtension(fileName), Network.numNodes, Network.numEdges));

                Console.Write(string.Format("{0},({1}),{2},", bma.path.Count, String.Join(" ", bma.path.ToArray()), bma.elapsedTime.TotalMilliseconds));
                Console.Write(string.Format("{0},({1}),{2},", grmin.path.Count, String.Join(" ", grmin.path.ToArray()), grmin.elapsedTime.TotalMilliseconds));
                Console.Write(string.Format("{0},({1}),{2},", grmax.path.Count, String.Join(" ", grmax.path.ToArray()), grmax.elapsedTime.TotalMilliseconds));
                Console.Write(string.Format("{0},({1}),{2},", gc1.path.Count, String.Join(" ", gc1.path.ToArray()), gc1.elapsedTime.TotalMilliseconds));
                Console.Write(string.Format("{0},({1}),{2},", vs.path.Count, String.Join(" ", vs.path.ToArray()), vs.elapsedTime.TotalMilliseconds));
                Console.WriteLine();
                //SaveOutput();
            }
        }

        static void SaveOutput()
        {
            string path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            string fileName = string.Format("{0:yyyy-MM-dd-HH-mm-ss}", DateTime.Now);
            Network.SaveAdjacencyMatrix(path + "\\" + fileName + ".txt");
            Network.SaveGraphViz(path + "\\" + fileName + ".gv");
        }
    }
}
