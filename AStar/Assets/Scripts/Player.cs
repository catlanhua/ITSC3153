using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField]
    Sprite greenTileSprite;
    
    Tilemap tilemap;
    List<Vector3Int> shortestPath;
    Rigidbody2D rb2D;
    bool isMoving = false;
    const int speed = 2;     // Movement speed
   
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void Update()
    {

    }
 
    public void Move(List<Vector3Int> path)
    {
        shortestPath = path;
        if (shortestPath.Count > 1)
        {
            Velocity(shortestPath[1]);
            isMoving = true;
        }
        
    }

    // Sent each frame where another object is within a trigger collider attached to this object
    void OnTriggerStay2D(Collider2D coll)
    {
        if (isMoving)
        {
            Vector3Int collCellPosition = tilemap.WorldToCell(coll.gameObject.transform.position);
            if (shortestPath[0] == collCellPosition)
            {
                coll.gameObject.GetComponent<SpriteRenderer>().sprite = greenTileSprite;
                if (shortestPath.Count > 1)
                {
                    rb2D.velocity = Vector2.zero;
                    shortestPath.RemoveAt(0);
                    Velocity(shortestPath[0]);
                }
                else
                {
                    rb2D.velocity = Vector2.zero;
                    Vector3 flagTilePosition = coll.gameObject.transform.position;
                    flagTilePosition.z = this.gameObject.transform.position.z;
                    this.gameObject.transform.position = flagTilePosition;
                    isMoving = false;
                }
            }
        }
    }

    void Velocity(Vector3Int cellCoordinates)
    {
        Vector3 worldCoordinates = tilemap.GetCellCenterWorld(cellCoordinates);
        Vector2 force = new Vector2(worldCoordinates.x - transform.position.x, worldCoordinates.y - transform.position.y);
        rb2D.velocity = force * speed;
    }
}
