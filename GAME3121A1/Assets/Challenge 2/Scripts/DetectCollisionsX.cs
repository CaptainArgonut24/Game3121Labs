using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisionsX : MonoBehaviour
{
    public GameObject Dog;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dog"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }

    }

}
