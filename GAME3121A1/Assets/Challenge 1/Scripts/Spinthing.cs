using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinthing : MonoBehaviour
{

    /* Round and round,
       Let the city turn
       Party in the hills
       We can party in the burbs
       Roof on fire
       "Let it burn"
       Champagne in my hand
       I am not concerned
     */

    [SerializeField] float RaRSpeed;

    // Start is called before the first frame update
    void Start()
    {
        print("spin");
    }

    // Update is called once per frame
    void Update()
    {
        // makes it go round and round 
        transform.Rotate(Vector3.forward * RaRSpeed * Time.deltaTime);
    }
}
