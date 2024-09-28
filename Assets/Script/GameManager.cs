using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // initialization purposes
    public GameObject tilePrefab;
    public Material[] materials;
    public Transform[] tilesPosition;

    // stores the gameObject (needed later for destroying when restarting the game)
    private GameObject[] tileGameObjects = new GameObject[12];

    // stores the state of the game
    private enum clickStates { IDLE, FIRSTCLICKED};
    private clickStates clickState = clickStates.IDLE;
    private Tile firstSelected = null;
    private Tile secondSelected = null;

    // referenced in another script 
    public bool isRevealing = false;

    // Start is called before the first frame update
    void Start()
    {
        SpawnTiles();
    }

    // Update is called once per frame
    void Update()
    {
        // debug doang ngetes spawnTile di runtime nanti hapus aja 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SpawnTiles();
        }


        if (Input.GetMouseButtonDown(0))
        {
            ClickTile();
        }
    }

    void ClickTile()
    {
        // if any card is in the process of revealing do nothing
        if (isRevealing)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // checks if the raycast hit something in the world
        if (Physics.Raycast(ray, out hit))
        {
            // check if the hit object tag is a CardTile
            if (hit.transform.CompareTag("CardTile"))
            {
                // adjust the position and rotation
                StartCoroutine(hit.transform.GetComponentInParent<Tile>().Clicked());
            }

        }
    }

    void SpawnTiles()
    {
        // checks if tileGameObjects has gameObject element if it does, delete every instance of gameObjects
        if (tileGameObjects[0])
        {
            for (int i = 0; i < 12; i++)
            {
                Destroy(tileGameObjects[i]);
            }
        }

        // shuffle the position of the tiles
        ShuffleTilesPosition(tilesPosition);

        // instantiate a new tile in the position of the tilePosition
        for (int i = 0; i < 12; i++)
        {
            GameObject newObject = tilePrefab;
            int matIndex = (int)Mathf.Floor(i / 2);
            newObject.GetComponent<Tile>().SetMaterial(materials[matIndex]);
            tileGameObjects[i] = Instantiate(newObject, tilesPosition[i].position, Quaternion.Euler(0, 0, 180));
        }
    }

    void ShuffleTilesPosition(Transform[] arr)
    {
        // Shuffle the array using Fisher-Yates shuffle
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Transform temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }
}
