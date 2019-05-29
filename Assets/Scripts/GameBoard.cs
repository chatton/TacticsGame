using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameBoard : MonoBehaviour
{
    private Graph graph;
    public Color clearColour;


    private Dictionary<Tuple<Tile,Tile, int>, List<Tile>> cache;
    private Dictionary<Tuple<Tile, int>, List<Tile>> movementRangeCache;

    void Start()
    {
        graph = buildGraphFromChildren();
        cache = new Dictionary<Tuple<Tile, Tile, int>, List<Tile>>();
        movementRangeCache = new Dictionary<Tuple<Tile, int>, List<Tile>>();
    }

    private Graph buildGraphFromChildren()
    {
        var tiles = new Dictionary<Tile, List<Tile>>();
        foreach (Transform cube in transform)
        {
            int x = (int)Math.Floor(cube.position.x);
            int z = (int)Math.Floor(cube.position.z);
            Tile tile = cube.GetComponent<Tile>();
            tile.x = x;
            tile.y = z;
            tiles[tile] = new List<Tile>(); // empty adjacency list
        }
        return new Graph(tiles);
    }

    public void Clear()
    {
        graph.AllTiles().ForEach(t =>
        {
            t.GetComponent<MeshRenderer>().material.color = clearColour;
        });
    }

    private List<Tile> getGraphBfsPath(Tile from, Tile to, int move)
    {
        var tuple = new Tuple<Tile, Tile, int>(from, to, move);
        if (cache.ContainsKey(tuple))
        {
            print("using cache!");
            return cache[tuple];
        }
        cache[tuple] = graph.BFSPath(from,to, move);
        return cache[tuple];
    }

    private List<Tile> getGraphBfs(Tile tile,  int move)
    {
        var tuple = new Tuple<Tile, int>(tile, move);
        if (movementRangeCache.ContainsKey(tuple))
        {
            print("using movementRangeCache!");
            return movementRangeCache[tuple];
        }
        movementRangeCache[tuple] = graph.BFS(tile, move);
        return movementRangeCache[tuple];
    }

    public void HighlightPath(Tile fromTile, Tile to, int move)
    {
        if (fromTile == null || to == null)
        {
            return;
        }

        print("HighlightPath");
        List<Tile> tiles = graph.BFSPath(fromTile, to, move);
        foreach (var item in tiles)
        {
            item.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        print(tiles.Count);
    }

    public void HighlightMovementRangeFromTile(Tile tile, int move, Material mat)
    {

        if (tile == null)
        {
            return;
        }
        print("HighlightMovementRangeFromTile");
        List<Tile> tiles = getGraphBfs(tile, move);
        foreach (var item in tiles)
        {
            //item.GetComponent<MeshRenderer>().material.color = colour;
            item.GetComponent<MeshRenderer>().material = mat;
        }

    }

    internal List<Tile> GetPathBetween(Tile fromTile, Tile toTile, int movement)
    {
        print("GetPathBetween");
        return graph.BFSPath(fromTile, toTile, movement);
        //return grap
    }


    public void DrawPath(List<Tile> path, Color colour)
    {

        if (path == null || path.Count == 0)
        {
            return;
        }
        print("DrawPath");
        path.ForEach(item => item.GetComponent<MeshRenderer>().material.color = colour);
    }
}
