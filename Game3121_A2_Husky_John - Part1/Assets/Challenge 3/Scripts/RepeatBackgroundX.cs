using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RepeatBackgroundX : MonoBehaviour
{
    
    private float3 startPos;
    private float repeatWidth;

    private void Start()
    {
        startPos = transform.position; // Establish the default starting position 
        repeatWidth = GetComponent<BoxCollider>().size.x / 2; // Set repeat width to half of the background
    }

    private void Update()
    {
        
        float3 currentPosition = transform.position;

        if (currentPosition.x < startPos.x - repeatWidth)
        {
            //convert it back
            transform.position = new float3(startPos.x, currentPosition.y, currentPosition.z);
        }
    }

 
}


