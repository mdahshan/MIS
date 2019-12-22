using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Collections;
using System.Diagnostics;

namespace MISmain
{
    public struct Node
    {
        public int cost;
        public int previous;
        public byte[] excNodes;
    }

    public class BMA
    {
        private int[][] edges;
        private List<int> nodeEdges;

        public Node[,] nodes;
        public ArrayList path;
        Stopwatch stopWatch;

        public TimeSpan elapsedTime;
        public int cost;
        Node nextNode; //used as temporary variable

        public void Initialize()
        {
            stopWatch = new Stopwatch();
            edges = new int[Network.numNodes][];

            for (int i = 0; i < Network.numNodes; i++)
                Network.adjMatrix[i, i] = 1; // we need diagonals to be 1

            nodes = new Node[Network.numNodes, Network.numNodes];
            path = new ArrayList();
            nodeEdges = new List<int>();

            for (int i = 0; i < Network.numNodes; ++i)
            {
                for (int j = 0; j < Network.numNodes; ++j)
                    if (Network.adjMatrix[i, j] == 1 && i != j)
                        nodeEdges.Add(j);
                edges[i] = new int[nodeEdges.Count];
                nodeEdges.CopyTo(edges[i]);
                nodeEdges.Clear();
            }

            for (int i = 0; i < Network.numNodes; ++i)
            {
                nodes[i, 0].excNodes = new byte[Network.numNodes]; //initialize excNodes

                for (int j = 0; j < Network.numNodes; ++j)
                {
                    nodes[i, j].excNodes = new byte[Network.numNodes]; //initialize excNodes

                    nodes[i, 0].excNodes[j] = Network.adjMatrix[i, j]; //nodes connected are excluded
                    nodes[i, 0].cost += Network.adjMatrix[i, j];  // cost = count of excNodes[] == 1

                    nodes[i, j].previous = -1;
                }

                for (int j = 1; j < Network.numNodes; ++j)
                    nodes[i, j].cost = Network.INF;
            }

        }

        public void Clean()
        {
            nodes = null;
            edges = null;
            nextNode.excNodes = null;
        }

        public void MaxIndSet()
        {
            int l, n1, n2, t;
            Node nextNode;
            int breakLevel = -1;
            int breakNode = -1;
            bool isResume;
            nextNode = new Node();
            nextNode.excNodes = new byte[Network.numNodes];

            stopWatch.Start();
            for (l = 0; l < Network.numNodes - 1; ++l)
            {
                isResume = true;

                for (n1 = 0; n1 < Network.numNodes; n1++)
                    if ((nodes[n1, l].cost < Network.INF)) // n1 is unreachable in this level l
                    {
                        for (n2 = 0; n2 < Network.numNodes; n2++)
                        {
                            if ((nodes[n1, l].excNodes[n2] == 0)) //n2 is not in exc list of n1 in level l
                            {
                                //nextNode.cost = 0; nextNode.previous = 0;
                                //for (int i = 0; i < numNodes; ++i)
                                //{
                                //    nextNode.excNodes[i] = (byte) (nodes[n1, l].excNodes[i] | adjMatrix[n2, i]);
                                //    nextNode.cost += nextNode.excNodes[i];
                                //}
                                
                                //combine excluded nodes for n1 and n2 when added to MIS
                                Array.Copy(nodes[n1, l].excNodes, nextNode.excNodes, Network.numNodes);
                                nextNode.excNodes[n2] = 1;
                                nextNode.cost = nodes[n1, l].cost + 1;

                                for(int i = 0; i < edges[n2].Length; i++)
                                    if(nextNode.excNodes[edges[n2][i]] == 0) 
                                    {
                                        nextNode.excNodes[edges[n2][i]] = 1; //exclude ith neighbor of n2
                                        nextNode.cost++; //cost increases with number of excluded nodes
                                    }

                                nextNode.previous = n1;

                                if (nodes[n2, l + 1].cost > nextNode.cost)
                                {
                                    nodes[n2, l + 1].cost = nextNode.cost;
                                    nodes[n2, l + 1].previous = nextNode.previous;
                                    Array.Copy(nextNode.excNodes, nodes[n2, l + 1].excNodes, Network.numNodes);

                                    isResume = false; //at least one node added to independet set
                                    breakNode = n2;
                                }
                            }
                        }
                    }

                if (isResume)
                {
                    breakLevel = l;
                    cost = breakLevel + 1;
                    break;
                }
            }
            stopWatch.Stop();
            elapsedTime = stopWatch.Elapsed;

            t = breakNode;
            path.Add(t);
            
            while (t != -1)
            {
                t = nodes[t, breakLevel].previous;
                if (t != -1)
                    path.Add(t);

                breakLevel -= 1;
            }

            Clean();
        }

    }
}
