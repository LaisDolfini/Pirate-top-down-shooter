using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PirateTopDown.Pathfind
{
    public class Grid : MonoBehaviour
    {
        Transform _startPosition;
        public LayerMask wallMask;
        public Vector2 vGridWorldSize;
        [Range(0, 10)] public float nodeRadius;
        [Range(0, 1)] public float distanceNode;

        Node[,] _nodeMat;
        public List<Node> finalPath;

        float _nodeDiameter;
        int _gridSizeX, _gridSizeY;

        void Awake()
        {
            _startPosition = transform;
        }

        private void Start()
        {
            _nodeDiameter = nodeRadius * 2;
            _gridSizeX = Mathf.RoundToInt(vGridWorldSize.x / _nodeDiameter);
            _gridSizeY = Mathf.RoundToInt(vGridWorldSize.y / _nodeDiameter);
            CreateGrid();
        }

        void CreateGrid()
        {
            _nodeMat = new Node[_gridSizeX, _gridSizeY];
            Vector3 bottomLeft = transform.position - Vector3.right * vGridWorldSize.x / 2 - Vector3.up * vGridWorldSize.y / 2;

            for (int x = 0; x < _gridSizeX; x++)
            {
                for(int y = 0; y < _gridSizeY; y++)
                {
                    Vector3 worldPoint = bottomLeft + Vector3.right * (x * _nodeDiameter + nodeRadius) + Vector3.up * (y * _nodeDiameter + nodeRadius);
                    bool wall = true;

                    if(Physics2D.OverlapCircle(worldPoint, nodeRadius, wallMask))
                    {
                        wall = false;
                    }

                    _nodeMat[x, y] = new Node(wall, worldPoint, x, y);
                }
            }
        }

        public List<Node> GetNeighboringNodes(Node neighborNode)
        {
            List<Node> neighbroList = new List<Node>();
            int checkX;
            int checkY;

            checkX = neighborNode.iGridX + 1;
            checkY = neighborNode.iGridY;
            if((checkX >= 0) && (checkX < _gridSizeX))
            {
                if((checkY >= 0) && (checkY < _gridSizeY))
                {
                    neighbroList.Add(_nodeMat[checkX, checkY]);
                }
            }

            checkX = neighborNode.iGridX - 1;
            checkY = neighborNode.iGridY;
            if ((checkX >= 0) && (checkX < _gridSizeX))
            {
                if ((checkY >= 0) && (checkY < _gridSizeY))
                {
                    neighbroList.Add(_nodeMat[checkX, checkY]);
                }
            }

            checkX = neighborNode.iGridX;
            checkY = neighborNode.iGridY + 1;
            if ((checkX >= 0) && (checkX < _gridSizeX))
            {
                if ((checkY >= 0) && (checkY < _gridSizeY))
                {
                    neighbroList.Add(_nodeMat[checkX, checkY]);
                }
            }

            checkX = neighborNode.iGridX;
            checkY = neighborNode.iGridY - 1;
            if ((checkX >= 0) && (checkX < _gridSizeX))
            {
                if ((checkY >= 0) && (checkY < _gridSizeY))
                {
                    neighbroList.Add(_nodeMat[checkX, checkY]);
                }
            }

            return neighbroList;
        }

        public Node FromWorldPoint(Vector3 worldPos)
        {
            float xPos = ((worldPos.x + vGridWorldSize.x / 2) / vGridWorldSize.x);
            float yPos = ((worldPos.y + vGridWorldSize.y / 2) / vGridWorldSize.y);

            xPos = Mathf.Clamp01(xPos); 
            yPos = Mathf.Clamp01(yPos);

            int x = Mathf.RoundToInt((_gridSizeX - 1) * xPos); 
            int y = Mathf.RoundToInt((_gridSizeY - 1) * yPos);

            return _nodeMat[x, y];
        }

    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(vGridWorldSize.x, vGridWorldSize.y, 1));

            if(_nodeMat != null)
            {
                foreach(Node n in _nodeMat)
                {
                    if(n.isWall)
                    {
                        Gizmos.color = Color.white;
                    }
                    else
                    {
                        Gizmos.color = Color.yellow;
                    }

                    if(finalPath != null)
                    {
                        if(finalPath.Contains(n))
                        {
                            Gizmos.color = Color.red;
                        }
                    }

                    Gizmos.DrawWireCube(n.vPosition, Vector3.one * (_nodeDiameter - distanceNode));
                }
            }
        }
    #endif
    }
}
