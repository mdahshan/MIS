using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Collections;

namespace MISmain
{
    public class MaxSet
    {
        public int cost;
        public byte[] incNodes;
    }

    public class DepthFirst
    {
        private Node startNode;
        public MaxSet maxSet;
        public ArrayList path;

        Stopwatch stopWatch;
        public TimeSpan elapsedTime;

        public void Initialize()
        {
            stopWatch = new Stopwatch();
            startNode = new Node();
            path = new ArrayList();
            startNode.excNodes = new byte[Network.numNodes]; //initialize excNodes

            for (int i = 0; i < Network.numNodes; i++)
                Network.adjMatrix[i, i] = 0; // we need diagonals to be 0

        }

        public void Clean()
        {
            maxSet.incNodes = null;
            path.Clear();
        }

        public void MaxIndSet()
        {
            stopWatch.Start();

            maxSet = DepthMaxSet(startNode);
            for(int i=0; i< Network.numNodes; i++)
                if (maxSet.incNodes[i] !=0 )
                    path.Add(i);

            stopWatch.Stop();
            elapsedTime = stopWatch.Elapsed;
        }

        private MaxSet DepthMaxSet(Node node)
        {
            int i=0;
            Node currentNode = node;
            currentNode.excNodes = new byte[Network.numNodes];
            Array.Copy(node.excNodes, currentNode.excNodes, Network.numNodes);
            
            MaxSet maxSet = new MaxSet(); //assuming all incNodes = 0
            maxSet.incNodes = new byte[Network.numNodes];

            while ((i < Network.numNodes) && (currentNode.excNodes[i] != 0)) //find first i with excNodes not 0
                i++;

            if (i >= Network.numNodes)
                return maxSet;

            //else:
            // Now we have node.excNodes[i] == 0
            currentNode.excNodes[i] = 1;

            //while ((maxSet.incNodes[i++] == 1) && (i < Network.numNodes))
                //maxSet.cost++;ri

            MaxSet y = DepthMaxSet(currentNode);

            for (int j = 0; j < Network.numNodes; j++)
                currentNode.excNodes[j] |= Network.adjMatrix[i, j]; //bitwise OR

            MaxSet x = DepthMaxSet(currentNode); // excluded i and its neighbors

            if ((x.cost + 1) > y.cost)
            {
                maxSet.cost = x.cost + 1;
                maxSet.incNodes = x.incNodes;
                maxSet.incNodes[i] = 1;
            }
            else
            {
                maxSet.cost = y.cost;
                maxSet.incNodes = y.incNodes;
            }

            return maxSet;
        }

    }
}
