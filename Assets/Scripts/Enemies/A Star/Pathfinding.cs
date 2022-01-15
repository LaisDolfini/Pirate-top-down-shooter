using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PirateTopDown.Pathfind
{
    public class Pathfinding : MonoBehaviour
    {
        private Grid _gridRef;
        private Transform _start;
        [HideInInspector] public Transform end;
        public List<Node> pathList = new List<Node>();

        void Start()
        {
            _start = transform;
        }

        void Update()
        {
            FindPath(_start.position, end.position);
        }

        public void Setup(Grid gridRef, Transform target)
        {
            _gridRef = gridRef;
            end = target;
        }

        void FindPath(Vector3 s, Vector3 e)
        {
            Node startNode = _gridRef.FromWorldPoint(s);
            Node targetNode = _gridRef.FromWorldPoint(e);

            List<Node> opened = new List<Node>();
            HashSet<Node> closed = new HashSet<Node>();

            opened.Add(startNode);

            while (opened.Count > 0)
            {
                Node current = opened[0];
                for (int i = 1; i < opened.Count; i++)
                {
                    if ((opened[i].FCost < current.FCost) || ((opened[i].FCost == current.FCost) && opened[i].ihCost < current.ihCost))
                    {
                        current = opened[i];
                    }

                }
                opened.Remove(current);
                closed.Add(current);

                if (current == targetNode)
                {
                    GetFinalPath(startNode, targetNode);
                }

                foreach(Node neighborNode in _gridRef.GetNeighboringNodes(current))
                {
                    if(!neighborNode.isWall || closed.Contains(neighborNode))
                    {
                        continue;
                    }
                    int moveCost = current.igCost + GetDistance(current, neighborNode);

                    if((moveCost < neighborNode.igCost) || !opened.Contains(neighborNode))
                    {
                        neighborNode.igCost = moveCost;
                        neighborNode.ihCost = GetDistance(neighborNode, targetNode);
                        neighborNode.parentNode = current;

                        if(!opened.Contains(neighborNode))
                        {
                            opened.Add(neighborNode);
                        }
                    }
                }
            }
        }

        void GetFinalPath(Node start, Node end)
        {
            List<Node> finalPath = new List<Node>();
            Node current = end;

            while(current != start)
            {
                finalPath.Add(current);
                current = current.parentNode;
            }

            finalPath.Reverse();
            pathList = finalPath;
            _gridRef.finalPath = finalPath;
        }

        int GetDistance(Node a, Node b)
        {
            int x = Mathf.Abs(a.iGridX - b.iGridX);
            int y = Mathf.Abs(a.iGridY - b.iGridY);

            return x + y;
        }
    }
}
