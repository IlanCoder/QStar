using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour {

    public List<Node> path;
    public LayerMask obstacleLayer;
    public Vector3 gridSize;
    public float nodeSize;
    Vector3 nodeBoxSize;
    float nodeRadius;
    Vector2 nodesInGrid;
    Node[,] grid;  

    void Start() {
        nodeRadius = nodeSize / 2;
        nodeBoxSize = new Vector3(nodeRadius, nodeRadius, nodeRadius);
        CreateAndSetGrid();
    }

    void CreateAndSetGrid() {
        nodesInGrid.x = Mathf.Round(gridSize.x / nodeSize);
        nodesInGrid.y = Mathf.Round(gridSize.z / nodeSize);
        Vector3 startingCorner = new Vector3(0, 0, 0);
        startingCorner.x = transform.position.x - gridSize.x / 2;
        startingCorner.z = transform.position.z - gridSize.z / 2;
        grid = new Node[(int)nodesInGrid.x, (int)nodesInGrid.y];
        for(int gridPosX = 0; gridPosX < nodesInGrid.x; ++gridPosX) {
            for (int gridPosY = 0; gridPosY < nodesInGrid.y; ++gridPosY) {
                Vector3 nodeGridPosition = CalculateNodePosition(startingCorner, gridPosX, gridPosY);
                bool walkable = !CheckNodeObstacleCollision(nodeGridPosition);
                grid[gridPosX, gridPosY] = new Node(walkable, nodeGridPosition, gridPosX, gridPosY);
            }
        }
    }

    void Update() {
        UpdateGrid();
    }

    void UpdateGrid() {
        for (int gridPosX = 0; gridPosX < nodesInGrid.x; ++gridPosX) {
            for (int gridPosY = 0; gridPosY < nodesInGrid.y; ++gridPosY) {
                Node node = grid[gridPosX, gridPosY];
                bool newWalkable = !CheckNodeObstacleCollision(node.position);
                node.walkable = newWalkable;
            }
        }
    }

    bool CheckNodeObstacleCollision(Vector3 position) {
        return (Physics.CheckBox(position, nodeBoxSize, Quaternion.identity, obstacleLayer));
    }

    Vector3 CalculateNodePosition(Vector3 startingCoordinate, int posX, int posZ) {
        Vector3 nodePosition = startingCoordinate;
        nodePosition.x += posX * nodeSize + nodeRadius;
        nodePosition.z += posZ * nodeSize + nodeRadius;
        return nodePosition;
    }

    public Node GetObjectNodePosition(Vector3 playerPos) {
        Vector2 playerGridPos;
        playerGridPos.x = Mathf.Clamp01((playerPos.x + gridSize.x / 2) / gridSize.x);
        playerGridPos.x = Mathf.Round((nodesInGrid.x - 1) * playerGridPos.x);
        playerGridPos.y = Mathf.Clamp01((playerPos.z + gridSize.z / 2) / gridSize.z);
        playerGridPos.y = Mathf.Round((nodesInGrid.y - 1) * playerGridPos.y);
        return grid[(int)playerGridPos.x, (int)playerGridPos.y];
    }

    public List<Node> GetNeighbours(Node node) {
        List<Node> neighboursList = new List<Node>();
        for(int nodeRelativeX = -1; nodeRelativeX <= 1; ++nodeRelativeX) {
            for (int nodeRelativeY = -1; nodeRelativeY <= 1; ++nodeRelativeY) {
                if (nodeRelativeX == 0 && nodeRelativeY == 0) {
                    continue;
                }
                int checkX = (int)node.gridPosition.x + nodeRelativeX;
                int checkY = (int)node.gridPosition.y + nodeRelativeY;

                if(checkX >=0 && checkX < nodesInGrid.x) {
                    if(checkY >= 0 && checkY < nodesInGrid.y) {
                        neighboursList.Add(grid[checkX, checkY]);
                    }
                }
            }           
        }
        return neighboursList;
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, gridSize);
        if (grid != null) {
            foreach (Node node in grid) {
                Gizmos.color = node.walkable ? Color.green : Color.red;
                if (path != null) {
                    if (path.Contains(node)) {
                        Gizmos.color = Color.blue;
                    }
                }
                Gizmos.DrawCube(node.position, nodeBoxSize);
            }
        }
    }
}
