using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject tilePrefab;
    public Material[] materials;
    public Transform[] tilesPosition;

    private GameObject[] tileGameObjects = new GameObject[12];

    public Transform testPos;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(tileGameObjects.Length);
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
            tileGameObjects[i] = Instantiate(newObject, tilesPosition[i].position, Quaternion.identity);
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
