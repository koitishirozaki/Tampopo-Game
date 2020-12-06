using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage;

    public float noAtkWindow = 0.5f;
    public float atkWindow = 1f;


    private Collider pCollider;
    private Rigidbody rb;
    private CustomGravity cGravity;
    private PlayerMovement pMovement;
    private T_Health health;

    public List<GameObject> hitStuff = new List<GameObject>();

    public SpriteRenderer renderer;

    private bool onWait;
    public bool isTriggerON;

    // Start is called before the first frame update
    void Start()
    {
        pMovement = GetComponent<PlayerMovement>();
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
            isTriggerON = true;
        }
    }

    public void TriggerOFF()
    {


        if (onWait)
        {
            if(hitStuff.Count > 0)
            {
                // ATK WINDOW AS USUAL
                pMovement.timerActive = true;
                pMovement.maxTimer = atkWindow; 
                
                hitStuff.Clear();
            }
            else
            {
                // NO ATK WINDOW
                pMovement.timerActive = true;
                pMovement.maxTimer = noAtkWindow;
            }
            
            pCollider.isTrigger = false;
            cGravity.Gravitation(true);
            onWait = false;

            Debug.Log("trigger should turn off");
            health.Invencivel(false);
            renderer.color = Color.white;

            isTriggerON = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            hitStuff.Add(other.gameObject);
            T_Health otherHealth = other.GetComponent<T_Health>();
            otherHealth.TakeSimpleDamage(damage);

            Debug.Log("enemy encountered!");
        }

        if(other.CompareTag("NPC"))
        {
            // do stuff if NPC
        }

        if(other.CompareTag("Object"))
        {
            hitStuff.Add(other.gameObject);
            // do stuff if interactable object
        }


    }

}
