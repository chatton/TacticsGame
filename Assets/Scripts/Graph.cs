using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{

    private Dictionary<Tile, List<Tile>> graph;
    private Dictionary<Tuple<int, int>, Tile> tileCoords;

    public Graph(Dictionary<Tile, List<Tile>> tiles)
    {
        graph = new Dictionary<Tile, List<Tile>>(tiles);
        tileCoords = new Dictionary<Tuple<int, int>, Tile>();
        foreach (var item in graph)
        {
            // keep track of the co-ordinates of each tile.
            tileCoords[new Tuple<int, int>(item.Key.x, item.Key.y)] = item.Key;
        }

        foreach (var coord in tileCoords)
        {
            var up = new Tuple<int, int>(coord.Value.x, coord.Value.y + 1);
            var down = new Tuple<int, int>(coord.Value.x, coord.Value.y - 1);
            var left = new Tuple<int, int>(coord.Value.x - 1, coord.Value.y);
            var right = new Tuple<int, int>(coord.Value.x + 1, coord.Value.y);
            var adjacencyList = graph[coord.Value];
            addIfPresent(up, adjacencyList);
            addIfPresent(down, adjacencyList);
            addIfPresent(left, adjacencyList);
            addIfPresent(right, adjacencyList);
        }

    }

    internal List<Tile> AllTiles()
    {
        return new List<Tile>(graph.Keys);
    }

    private void addIfPresent(Tuple<int, int> coord, List<Tile> adjList)
    {
        if (tileCoords.ContainsKey(coord))
        {
            adjList.Add(tileCoords[coord]);
        }
    }


    public List<Tile> BFS(Tile startingTile, int depth)
    {
        Queue<Tile> q = new Queue<Tile>();
        ISet<Tile> visited = new HashSet<Tile>();
        startingTile.depth = 0;
        q.Enqueue(startingTile);
        visited.Add(startingTile);

        while (q.Count > 0)
        {
            Tile t = q.Dequeue();
            int currentDepth = t.depth;
            if (currentDepth == depth)
            {
                if (currentDepth == depth)
                {
                    return new List<Tile>(visited);
                }
            }
            foreach (var nextTile in graph[t])
            {
                if (visited.Contains(nextTile))
                {
                    continue;
                }
                nextTile.depth = currentDepth + 1;
                visited.Add(nextTile);
                q.Enqueue(nextTile);
            }
        }
        return new List<Tile>(visited);
    }

    public List<Tile> BFSPath(Tile startingTile, Tile destinationTile, int depth)
    {

        Queue<Tile> q = new Queue<Tile>();
        ISet<Tile> visited = new HashSet<Tile>();
        startingTile.depth = 0;
        q.Enqueue(startingTile);
        visited.Add(startingTile);

        while (q.Count > 0)
        {
            Tile t = q.Dequeue();
            int currentDepth = t.depth;
            if (currentDepth > depth)
            {
                return new List<Tile>();
            }
            if (t == destinationTile)
            {
                return reconstructPath(t);
            }

            foreach (var nextTile in graph[t])
            {
                if (visited.Contains(nextTile))
                {
                    continue;
                }
                nextTile.parent = t;
                nextTile.depth = currentDepth + 1;
                visited.Add(nextTile);
                q.Enqueue(nextTile);
            }
        }
        return new List<Tile>();
    }

    private List<Tile> reconstructPath(Tile t)
    {

        var path = new List<Tile>();
        Tile current = t;
        while (current.parent != null)
        {
            path.Add(current);
            current = current.parent;
        }
        return path;
    }
}
