using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackForce;
    public int contactDamage;

    private GameObject playerObject;
    private Rigidbody targetRb;
    private T_Health targetHealth;
    private PlayerController pMove;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CacheComponents(other);

            if (!targetHealth.EuSouINVENCIVEL)
            {
                if (targetRb != null)
                {
                    Vector3 distanceB2PlayerObject = transform.position - targetRb.position;
                    targetRb.AddForce(-distanceB2PlayerObject * knockbackForce * 1000);
                    pMove.autoWalk = false;

                    targetHealth.TakeSimpleDamage(contactDamage);
                }

                StartCoroutine(targetHealth.InvencivelTime(1f));
            }
            
        }

    }

    void CacheComponents(Collider other)
    {
        playerObject = other.gameObject;

        targetRb = playerObject.GetComponent<Rigidbody>();
        targetHealth = playerObject.GetComponent<T_Health>();
        pMove = playerObject.GetComponent<PlayerController>();

    }

}
