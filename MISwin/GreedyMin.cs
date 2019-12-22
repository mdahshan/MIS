using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;

namespace MISmain
{
    public class GreedyMin
    {
        private byte[,] adjMatrix;
        private int[] degree;
        private int numNodes;
        private int[] Vc;
        public ArrayList path;

        Stopwatch stopWatch;
        public TimeSpan elapsedTime;

        public void Initialize()
        {
            stopWatch = new Stopwatch();
            numNodes = Network.numNodes;
            adjMatrix = new byte[numNodes, numNodes];
            Array.Copy(Network.adjMatrix, adjMatrix, numNodes * numNodes);
            for (int i = 0; i < numNodes; i++)
                adjMatrix[i, i] = 0;

            Vc = new int[numNodes];
            degree = new int[numNodes];
            path = new ArrayList();
        }

        public void Clean()
        {
            Vc = null;
            adjMatrix = null;
            degree = null;
        }

        public bool AdjEmpty()
        {
            for (int i = 0; i < numNodes; i++)
                for (int j = i+1; j < numNodes; j++)
                    if (adjMatrix[i, j] > 0)
                        return false;

            return true;
        }

        public int Degree(int v)
        {
            int d = 0;
            for (int u = 0; u < numNodes; u++)
                if (adjMatrix[u, v] > 0)
                    d++;
            return d;
        }

        public void MaxIndSet()
        {
            int minDegree, minNode;
            Array.Clear(Vc, 0, Vc.Length);

            stopWatch.Start();
            do
            {
                for (int i = 0; i < numNodes; i++)
                    degree[i] = Degree(i);

                minNode = 0;
                minDegree = degree[0];

                for (int i = 1; i < numNodes; i++)
                    if ((minDegree > degree[i] && degree[i] != 0) ||( minDegree == 0 && degree[i] != 0))
                    { 
                        minNode = i; 
                        minDegree = degree[i]; 
                    }

                if (degree[minNode] > 0)
                {
                    for (int i = 0; i < numNodes; i++)
                        if (adjMatrix[i, minNode] > 0)
                        {
                            Vc[i] = 1;
                            for (int j = 0; j < numNodes; j++)
                                adjMatrix[i, j] = adjMatrix[j, i] = 0;
                        }
                }

            } while (!AdjEmpty());

            stopWatch.Stop();

            elapsedTime = stopWatch.Elapsed;

            for (int i = 0; i < numNodes; i++)
                if (Vc[i] == 0)
                    path.Add(i);

            Clean();
        }
    }
}
