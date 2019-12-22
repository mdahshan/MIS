using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace MISmain
{
    public struct Edge
    {
        public int 
            u, // node number of one side of the edge
            v, // node number of the other side of the edge
            w; // weight of the edge
    }

    public class BellmanFord
    {
        //Source: http://compprog.wordpress.com/2007/11/29/one-source-shortest-path-the-bellman-ford-algorithm/

        const int INF = 1000;
        public int numNodes, numEdges;
        public Edge[] edges;
        public int[] distance;

        public BellmanFord()
        {
            edges = new Edge[1024];
        }

        public void ReadAdjacencyMatrix(string fileName)
        {
            try
            {
                using (TextReader reader = File.OpenText(fileName))
                {
                    numNodes = int.Parse(reader.ReadLine());
                    distance = new int[numNodes];

                    int i, j, w;
                    numEdges = 0;
                    
                    for (i = 0; i < numNodes; ++i)
                    {
                        string line = reader.ReadLine();
                        string[] weights = line.Split(' ');

                        for (j = 0; j < numNodes; ++j)
                        {
                            w = int.Parse(weights[j]);
                            if (w != 0)
                            {
                                edges[numEdges].u = i;
                                edges[numEdges].v = j;
                                edges[numEdges].w = w;
                                ++numEdges;
                            }
                        }
                    }


                }

            }
            catch (IOException e)
            {
                Console.WriteLine(e.GetType().Name);
            }

        }

        public void ShortestPath(int s)
        {
            int i, j;

            for (i = 0; i < numNodes; ++i)
                distance[i] = INF;

            distance[s] = 0;

            for (i = 0; i < numNodes - 1; ++i)
                for (j = 0; j < numEdges; ++j)
                    if (distance[edges[j].u] + edges[j].w < distance[edges[j].v])
                        distance[edges[j].v] = distance[edges[j].u] + edges[j].w;
 
        }

    }
}
