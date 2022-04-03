using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class AStar : MonoBehaviour
{
    [SerializeField] 
    GameObject cannotFindPath;
    // Tilemap support
    Tilemap tilemap;
    Vector3Int playerTilePosition;
    Vector3Int flagTilePosition;

    // Get distance between 2 tiles, diagonals included
    int GetDistance(Vector3Int tilePosition1, Vector3Int tilePosition2)
    {
        int dstX = Mathf.Abs(tilePosition1.x - tilePosition2.x);
        int dstY = Mathf.Abs(tilePosition1.y - tilePosition2.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
   
    // Retrace the path from the start to the goa
    List<Vector3Int> RetracePath(Vector3Int[,] parentArray)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Vector3Int currentTilePosition = flagTilePosition;
        // Loop while the current tile is not the same as the player tile
        while (currentTilePosition != playerTilePosition)
        {
            path.Add(currentTilePosition);
            currentTilePosition = parentArray[currentTilePosition.x, currentTilePosition.y];
        }
        path.Add(playerTilePosition);
        path.Reverse();
        print($"Path count is is {path.Count}");
        return path;
    }

    List<Vector3Int> CalculateShortestPath()
    {
        // Initialize variables
        int[,] gArray = new int[15, 15];
        int[,] hArray = new int[15, 15];
        Vector3Int[,] parentArray = new Vector3Int[15, 15];

        //GenerateHValues();

        // Set current tile as the player tile position
        List<Vector3Int> openList = new List<Vector3Int>();
        List<Vector3Int> closedList = new List<Vector3Int>();

        // Add the start tile position (playerTilePosition) to the openList
        openList.Add(playerTilePosition);

        // run A Star while open list is not empty
        while (openList.Count > 0)
        {
            // Get the first object in openList as currentTilePosition
            Vector3Int currentTilePosition = openList[0];
            // Loop through the tiles in openList
            for (int i = 1; i < openList.Count; i++)
            {
                int whiteTileAtIndexi_f = gArray[openList[i].x, openList[i].y] + hArray[openList[i].x, openList[i].y];
                int whiteTileAtIndexi_h = hArray[openList[i].x, openList[i].y];
                int currentWhiteTile_f = gArray[currentTilePosition.x, currentTilePosition.y] + hArray[currentTilePosition.x, currentTilePosition.y];
                int currentWhiteTile_h = hArray[currentTilePosition.x, currentTilePosition.y];

                // Update currentTilePosition if find a tile with lower f cost or a tile with equal f and lower h
                if (whiteTileAtIndexi_f < currentWhiteTile_f || whiteTileAtIndexi_f == currentWhiteTile_f && whiteTileAtIndexi_h < currentWhiteTile_h)
                {
                    currentTilePosition = openList[i];
                }
            }
            // Remove current tile position from open list and add it to closed list
            openList.Remove(currentTilePosition);
            closedList.Add(currentTilePosition);
            // If the current tile is the goal, retrace the path
            if (currentTilePosition == flagTilePosition)
            {
                return RetracePath(parentArray);
            }
            // For each neighbor tile to the current tile
            foreach (Vector3Int neighborTilePosition in GetNeighborTilePositions(currentTilePosition))
            {
                // If the neighbor is in closed list or neighbor is a black tile, skip
                if (closedList.Contains(neighborTilePosition) || tilemap.GetTile<Tile>(neighborTilePosition).gameObject.tag == "BlackTile")
                { 
                    continue; 
                }
                // Calculate cost from current tile to neighbor
                // Update g score for current tile and neighbor tile
                int currentWhiteTile_g = gArray[currentTilePosition.x, currentTilePosition.y];
                int neighborWhiteTile_g = gArray[neighborTilePosition.x, neighborTilePosition.y];
                int newMovementCostToNeighbour = currentWhiteTile_g + GetDistance(currentTilePosition, neighborTilePosition);
                
                // If new path to neighbor is shorter or openList does not contain neighbor
                if (newMovementCostToNeighbour < neighborWhiteTile_g || !openList.Contains(neighborTilePosition))
                {
                    // calculate the g and h for neighbor tile
                    gArray[neighborTilePosition.x, neighborTilePosition.y] = newMovementCostToNeighbour;
                    hArray[neighborTilePosition.x, neighborTilePosition.y] = GetDistance(neighborTilePosition, flagTilePosition);
                    parentArray[neighborTilePosition.x, neighborTilePosition.y] = currentTilePosition;
                    // If the neighbor tile is not in openList, add to openList
                    if (!openList.Contains(neighborTilePosition))
                        openList.Add(neighborTilePosition);
                }
            }
            // If the open list is empty, show message that path cannot be found
            if (openList.Count == 0) 
            {
                Instantiate(cannotFindPath, new Vector3(0, 0, -4), Quaternion.identity);
            }
        }
        return new List<Vector3Int>();
    }

    List<Vector3Int> GetNeighborTilePositions(Vector3Int tileCellCoordinates)
    {
        List<Vector3Int> neighborTilePositions = new List<Vector3Int>();
        print($"Current tile is {tileCellCoordinates}");

        for (int x = tileCellCoordinates.x - 1; x < tileCellCoordinates.x + 2; x++)
        {
            if (x < 0 || x > 14) { continue; } // Skip if x is less than 0 or greater than 14

            for (int y = tileCellCoordinates.y - 1; y < tileCellCoordinates.y + 2; y++)
            {
                if (x == tileCellCoordinates.x && y == tileCellCoordinates.y) { continue; } // Skip
                if (y < 0 || y > 14) { continue; } // Skip if y is less than 0 or greater than 14

                Vector3Int neighborTileCellCoordinates = new Vector3Int(x, y, 0);
                Tile neighborTile = tilemap.GetTile<Tile>(neighborTileCellCoordinates);
                WhiteTile neighbor = neighborTile.gameObject.GetComponent<WhiteTile>();
                if (neighborTile.gameObject.tag == "Black Tile") { continue; }  // Exclude black tiles
                else { print($"Neighbors: {neighborTileCellCoordinates}"); }

                neighborTilePositions.Add(neighborTileCellCoordinates);
            }
        }
        return neighborTilePositions;
    }

    public void SetPlayerTile(Vector3 tilePosition)
    {
        tilemap = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();
        playerTilePosition = tilemap.WorldToCell(tilePosition);
    }

    public void SetFlagTile(Vector3 tilePosition)
    {
        flagTilePosition = tilemap.WorldToCell(tilePosition);
        MovePlayer();
    }

    void MovePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        List<Vector3Int> shortestPath = CalculateShortestPath();
        player.GetComponent<Player>().Move(shortestPath);
    }
}
