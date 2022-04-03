using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class ClickHandler
{
    static int mouseCount = 0;
    static AStar aStar;

    // On-click listener
    public static void ClickOnTile(Vector3 tilePosition)
    {
        if (mouseCount == 0)
        {
            aStar.SetPlayerTile(tilePosition);
        }
        else if (mouseCount == 1)
        {
            aStar.SetFlagTile(tilePosition);
        }
        mouseCount++;
    }

    public static int MouseCount
    {
        get
        {
            return mouseCount;
        }

        set
        {
            mouseCount = value;
        }
    }

    public static void Initialize(GameObject aStarGameObj)
    {
        aStar = aStarGameObj.GetComponent<AStar>();
    }
}
