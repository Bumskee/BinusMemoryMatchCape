using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // initialization purposes
    public GameObject tilePrefab;
    public Material[] materials;
    public Transform[] tilesPosition;
    public UIController uiController;

    // stores the gameObject (needed later for destroying when restarting the game)
    private GameObject[] tileGameObjects = new GameObject[12];

    // stores the state of the game
    private GameObject firstSelected = null;
    private GameObject secondSelected = null;
    private int correct = 0;
    private bool gameFinished = false;

    // referenced in another script 
    public bool isRevealing = false;

    // timer
    public float maxTime;
    private float currTime;

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
        };

        // pencet tilenya
        if (Input.GetMouseButtonDown(0) && !gameFinished)
        {
            ClickTile();
        }

        // update timer and check if not all tiles have been opened yet
        if (currTime > 0 && correct != tileGameObjects.Length / 2)
        {
            currTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(currTime / 60);
            int seconds = Mathf.FloorToInt(currTime % 60);
            uiController.updateTimer(minutes, seconds);
        }
        else
        {
            uiController.endGame(maxTime - currTime, correct);
            gameFinished = true;
        }
    }

    public void ChangeLayer(GameObject tileParent, string layerName) 
    {
        tileParent.gameObject.layer = LayerMask.NameToLayer(layerName);
        foreach (Transform child in tileParent.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }

    public void CheckCorrectness()
    {
        if (secondSelected)
        {
            // if correct
            if (
                firstSelected.GetComponentInParent<Tile>().materialId
                ==
                secondSelected.GetComponentInParent<Tile>().materialId)
            {
                correct++;
                ChangeLayer(secondSelected.transform.parent.gameObject, "Ignore Raycast");
            }

            // if not correct
            else
            {
                StartCoroutine(firstSelected.GetComponentInParent<Tile>().Turn());
                StartCoroutine(secondSelected.GetComponentInParent<Tile>().Turn());
                ChangeLayer(firstSelected.transform.parent.gameObject, "Default");
                ChangeLayer(secondSelected.transform.parent.gameObject, "Default");
            }

            // set both to null again
            firstSelected = secondSelected = null;
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
                // assign first and second
                if (!firstSelected)
                {
                    firstSelected = hit.transform.gameObject;
                    StartCoroutine(firstSelected.GetComponentInParent<Tile>().Turn());
                    ChangeLayer(firstSelected.transform.parent.gameObject, "Ignore Raycast");
                } 
                else
                {
                    secondSelected = hit.transform.gameObject;
                    StartCoroutine(secondSelected.GetComponentInParent<Tile>().Turn());
                    ChangeLayer(secondSelected.transform.parent.gameObject, "Ignore Raycast");
                }
            }
        }
    }

    public void SpawnTiles()
    {
        // init
        gameFinished = false;
        currTime = maxTime;
        correct = 0;

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
            newObject.GetComponent<Tile>().SetMaterial(materials[matIndex], matIndex);
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
