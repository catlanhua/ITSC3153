using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class TileGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject whiteTile;

    [SerializeField]
    GameObject blackTile;

    [SerializeField]
    GameObject greenTile;

    [SerializeField]
    GameObject aStar;

    // Tilemap support
    Tilemap tilemap;
    Tile tile;
 
    // CHANGE TO TRUE TO TEST UNREACHABLE PATH
    bool doTest = false;  

    /// <summary>
    /// Awake is called before Start
    /// </summary>
    void Awake()
    {
        // Initialize Tilemap
        InitializeTilemap();

        // Initialize Tilemap
        GenerateWorld();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate aStar Game Object
        ClickHandler.Initialize(Instantiate<GameObject>(aStar));
    }

    // Update is called for every frame
    void Update()
    {
        // Exit application on pressing the 'Esc' key
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Application.Quit();
        }

        // Restart application on pressing the 'Spacebar' key
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            SceneManager.LoadScene("scene0");
            ClickHandler.MouseCount = 0;
        }
    }

    /// <summary>
    /// Initializes the tilemap
    /// </summary>
    void InitializeTilemap()
    {
        tilemap = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();
        tilemap.size = new Vector3Int(15, 15, tilemap.size.z);

        tile = ScriptableObject.CreateInstance<Tile>();
        tile.gameObject = whiteTile;
    }

    /// <summary>
    /// Populates the world
    /// </summary>
    void GenerateWorld()
    {

        // Initialize white tile
        Tile whiteTile = ScriptableObject.CreateInstance<Tile>();
        Tile blackTile = ScriptableObject.CreateInstance<Tile>();
        whiteTile.gameObject = this.whiteTile;
        blackTile.gameObject = this.blackTile;

        for (int y=0; y < 15; y++)
        {
            for (int x = 0; x < 15; x++)
            {
                if (doTest & y == 7)
                {
                    GenerateTiles(1, blackTile, x, y);
                    continue;
                }
                
                float randomNumber = Random.value;
                if (randomNumber < 0.1)
                {
                    // Generates black tile
                    GenerateTiles(1, blackTile, x, y);
                }
                else
                {
                    GenerateTiles(1, whiteTile, x, y);
                }
            }
        }
    }

    /// Generates Tiles
    /// </summary>
    /// <param name="number">Number of tiles to generate</param>
    void GenerateTiles(int number, Tile tile, int x, int y)
    {
        tilemap.BoxFill(new Vector3Int(x, y, 0), tile, x, y, x, y);
    }

}
