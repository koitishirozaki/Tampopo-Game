using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public float gravityScale = 1f;
    public static float globalGravity = -9.81f;
    Rigidbody rb;

    private bool isGravityON;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        if(isGravityON)
        {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }

    public void Gravitation(bool parameter)
    {
        if(parameter) isGravityON = true;
        else if(!parameter) isGravityON = false;
    }


}
