using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Diagnostics;


namespace MISmain
{
    public class GreedyC1
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
                for (int j = 0; j < numNodes; j++)
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

        public bool IsHighestDegreeNeighbors(int v)
        {
            bool isHighest = true;

            if (degree[v] ==0)
                isHighest = false;
            for (int u = 0; u < numNodes; u++)
                if (adjMatrix[u, v] == 1)
                    if (degree[u] > degree[v])
                        isHighest = false;
            return isHighest;
        }


        public void MaxIndSet()
        {
            int maxNode;
            Array.Clear(Vc, 0, Vc.Length);

            stopWatch.Start();
            do
            {
                for (int i = 0; i < numNodes; i++)
                    degree[i] = Degree(i);

                maxNode = 0;

                for (int i = 0; i < numNodes; i++)
                {
                    if (IsHighestDegreeNeighbors(i))
                    {
                        maxNode = i;
                        break;
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
