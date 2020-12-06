using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackForce;
    public int contactDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject playerObject = other.gameObject;

            Rigidbody target = playerObject.GetComponent<Rigidbody>();
            T_Health health = playerObject.GetComponent<T_Health>();
            PlayerMovement pMove = playerObject.GetComponent<PlayerMovement>();

            if (!health.EuSouINVENCIVEL)
            {
                if (target != null)
                {
                    Vector3 distanceB2PlayerObject = transform.position - target.position;
                    target.AddForce(-distanceB2PlayerObject * knockbackForce * 1000);
                    pMove.autoWalk = false;
                }
                if (health != null)
                {
                    health.TakeSimpleDamage(contactDamage);
                }

                StartCoroutine(health.InvencivelTime(0.3f));
            }
            
        }

    }

}
