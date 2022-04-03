using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class WhiteTile : MonoBehaviour
{
    [SerializeField]
    GameObject flag;

    [SerializeField]
    GameObject player;
 
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Is called when user releases mouse button
    void OnMouseUp()
    {
        Vector3 tilePosition = this.gameObject.transform.position;
        if (ClickHandler.MouseCount == 0)
        {
            // Place the player on tile after first click
            Vector3 player_position = this.gameObject.transform.position;
            player_position.z = -1;
            Instantiate<GameObject>(player, player_position, Quaternion.identity);

            ClickHandler.ClickOnTile(tilePosition);
        }
        else if (ClickHandler.MouseCount == 1)
        {
            // Place flag on tile after second click
            Vector3 flagPosition = this.gameObject.transform.position;
            flagPosition.z = -2;
            Instantiate<GameObject>(flag, flagPosition, Quaternion.identity);

            ClickHandler.ClickOnTile(tilePosition);
        }
    }

}
