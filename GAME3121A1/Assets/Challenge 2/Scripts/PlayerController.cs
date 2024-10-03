using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject dogPrefab;

    public float actuationDelay;

    private float elaspedTime;
    void Update()
    {
        elaspedTime += Time.deltaTime;
        if (elaspedTime > actuationDelay)
            // Spacebar spawns dog
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
                elaspedTime = 0;
            }
        }

    }
}
