using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage;

    private Collider pCollider;
    private Rigidbody rb;
    private CustomGravity cGravity;
    private T_Health health;

    public SpriteRenderer renderer;

    private bool onWait;

    // Start is called before the first frame update
    void Start()
    {
        pCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        cGravity = GetComponent<CustomGravity>();
        health = GetComponent<T_Health>();
    }

    public void TriggerON()
    {
        if (!onWait)
        {
            pCollider.isTrigger = true;
            cGravity.Gravitation(false);
            onWait = true;

            health.Invencivel(true);
            renderer.color = Color.red;
       
        }
    }

    public void TriggerOFF()
    {
        if (onWait)
        {
            pCollider.isTrigger = false;
            cGravity.Gravitation(true);
            onWait = false;

            health.Invencivel(false);
            renderer.color = Color.white;

            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            
            T_Health otherHealth = other.GetComponent<T_Health>();
            otherHealth.TakeSimpleDamage(damage);

            Debug.Log("enemy encountered!");
        }
    }

}
