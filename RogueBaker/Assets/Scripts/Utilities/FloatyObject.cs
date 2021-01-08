using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FloatyObject : MonoBehaviour
{
    float originalY;

    public float floatStrength = 1;
    // Start is called before the first frame update
    void Start()
    {
        this.originalY = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x,
            originalY + ((float)Math.Sin(Time.time) * floatStrength),
            transform.position.z);
    }
}
