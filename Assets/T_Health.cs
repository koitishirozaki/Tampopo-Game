using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Health : MonoBehaviour
{
    public int maxHealth = 100;
    [ReadOnly] public bool EuSouINVENCIVEL;

    [ReadOnly] public int currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0) Dead();
    }

    public void TakeSimpleDamage(int damage)
    {
        if(!EuSouINVENCIVEL)
        {
        currentHealth -= damage;
        }
    }

    public void Invencivel(bool ehmesmo)
    {
        EuSouINVENCIVEL = ehmesmo;
    }

    void Dead()
    {
        Debug.Log("dis guy is ded");
        gameObject.SetActive(false);
    }

}
