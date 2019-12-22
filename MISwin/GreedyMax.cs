using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Collections;

namespace MISmain
{
    public class GreedyMax
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
                for (int j = i + 1; j < numNodes; j++)
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
            int maxDegree, maxNode;
            Array.Clear(Vc, 0, Vc.Length);

            stopWatch.Start();
            do
            {
                for (int i = 0; i < numNodes; i++)
                    degree[i] = Degree(i);

                maxNode = 0;
                maxDegree = degree[0];

                for (int i = 1; i < numNodes; i++)
                {
                    if (maxDegree < degree[i])
                    {
                        maxDegree = degree[i];
                        maxNode = i;
                    }
                }

                if (degree[maxNode] > 0)
                {
                    Vc[maxNode] = 1;

                    for (int i = 0; i < numNodes; i++)
                    {
                        adjMatrix[i, maxNode] = adjMatrix[maxNode, i] = 0;
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
