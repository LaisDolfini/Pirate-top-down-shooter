using UnityEngine;

public class Node
{
    public int iGridX;
    public int iGridY;

    public bool isWall;
    public Vector3 vPosition;
    public Node parentNode;
    public int igCost;
    public int ihCost;

    public int FCost
    {
        get
        {
            return igCost + ihCost;
        }
    }

    public Node(bool wall, Vector3 vPos, int gridX, int gridY)
    {
        isWall = wall;
        vPosition = vPos;
        iGridX = gridX;
        iGridY = gridY;
    }
}
