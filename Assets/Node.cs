using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public bool walkable;
    public Vector3 position;
    public int gCost;
    public int hCost;
    public Vector2 gridPosition;
    public Node parent;

    public Node(bool walk, Vector3 pos, int gridPosX, int gridPosY) {
        walkable = walk;
        position = pos;
        gridPosition = new Vector2(gridPosX, gridPosY);
    }

    public int GetfCost() {
        return gCost + hCost;
    }
}
