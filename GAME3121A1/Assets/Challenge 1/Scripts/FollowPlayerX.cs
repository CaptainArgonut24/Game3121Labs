using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerX : MonoBehaviour
{
    public GameObject plane;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(50, -3, 0);
        transform.rotation = Quaternion.Euler(0, -90, 0);
        offset = transform.position - plane.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(plane.transform.position.x + offset.x,
            plane.transform.position.y + offset.y,
            plane.transform.position.z + offset.z);
    }
}
