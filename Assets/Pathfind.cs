using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfind : MonoBehaviour {

    public Transform seeker;
    public Transform target;
    GridMap grid;
    List<Node> openSet;
    HashSet<Node> closedSet;
    Node[] nodePath;
    int arrayPos;

    void Awake() {
        grid = GetComponent<GridMap>();
        StartCoroutine("movePlayer");
    }

    void Update() {
        FindPath(seeker.position, target.position);
        
    }

	void FindPath(Vector3 startingPos, Vector3 targetPos) {
        Node startNode = grid.GetObjectNodePosition(startingPos);
        Node targetNode = grid.GetObjectNodePosition(targetPos);
        openSet = new List<Node>();
        closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        while (openSet.Count > 0) {
            Node currentNode = openSet[0];
            currentNode = FindCheapestNode(currentNode);
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (currentNode == targetNode) {
                RetracePath(startNode, targetNode);
                return;
            }
            CalculateNeighbours(currentNode, targetNode);
        }
    }

    void RetracePath(Node beginNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != beginNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        nodePath = path.ToArray();
        grid.path = path;
    }

    int GetNodeDistance(Node nodeA, Node nodeB) {
        Vector2 distance = nodeA.gridPosition - nodeB.gridPosition;
        distance.x = Mathf.Abs(distance.x);
        distance.y = Mathf.Abs(distance.y);
        int manhattan = (int)distance.x + (int)distance.y;
        manhattan += Random.Range(-manhattan / 2, manhattan / 2);
        return manhattan;
    }

    Node FindCheapestNode(Node node) {
        for (int setNode = 1; setNode < openSet.Count; ++setNode) {
            if (openSet[setNode].GetfCost() < node.GetfCost() ||
                openSet[setNode].GetfCost() == node.GetfCost() &&
                openSet[setNode].hCost < node.hCost) {
                node = openSet[setNode];
            }
        }
        return node;
    }

    void CalculateNeighbours(Node node, Node target) {
        foreach (Node neighbour in grid.GetNeighbours(node)) {
            if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                continue;
            }
            int neighbourMovementCost = node.gCost + GetNodeDistance(node, neighbour);
            if (neighbourMovementCost < neighbour.gCost || !openSet.Contains(neighbour)) {
                neighbour.gCost = neighbourMovementCost;
                neighbour.hCost = GetNodeDistance(neighbour, target);
                neighbour.parent = node;
                if (!openSet.Contains(neighbour)) {
                    openSet.Add(neighbour);
                }
            }
        }
    }

    IEnumerator movePlayer() {
        while (true) {
            if (nodePath != null && nodePath.Length > 0) {
                seeker.position = new Vector3(nodePath[nodePath.Length - 1].position.x,
                    seeker.position.y,
                    nodePath[nodePath.Length - 1].position.z);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
