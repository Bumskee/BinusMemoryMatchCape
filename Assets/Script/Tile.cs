using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public MeshRenderer FrontPlane;
    public bool revealed = false;
    public float rotationSpeed = 0.5f;

    private GameManager gm;
    private Quaternion targetRotation;

    public int materialId;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("/GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaterial(Material mat, int matId)
    {
        FrontPlane.material = mat;
        materialId = matId;
    }

    public IEnumerator Turn()
    {
        gm.isRevealing = true;

        if (revealed)
        {
            targetRotation = Quaternion.Euler(0, 0, 180);
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        
        if (!revealed)
        {
            targetRotation = Quaternion.Euler(0, 0, 0);
            gameObject.layer = LayerMask.NameToLayer("Default");
        }

        revealed = !revealed;

        float step = 0f;

        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
            step += Time.deltaTime * rotationSpeed;

            yield return null;
        }

        gm.isRevealing = false;
        gm.CheckCorrectness();
    }
}
