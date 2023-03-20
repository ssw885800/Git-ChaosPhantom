using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSwitch : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float WaitTime;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            WaitTime = 0.5f;
        }
        if (Input.GetKey(KeyCode.S)) 
        {
            if(WaitTime<=0) 
            {
                effector.rotationalOffset = 180f;
                WaitTime = 0.5f;
            }
            else 
            {
                WaitTime -= Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.Space)) 
        {
            effector.rotationalOffset = 0f;
        }
    }
}
