using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebugStuff : MonoBehaviour
{

    public Text playerSpeed;
    public Text canAtk;
    public Text oncd;
    public Text timer;

    public PlayerMovement pMov;
    public Rigidbody pRb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeed.text = pRb.velocity.magnitude.ToString();
        oncd.text = pMov.onCooldown.ToString();
        canAtk.text = pMov.canAttack.ToString();
        timer.text = pMov.maxTimer.ToString();
    }

    public void HalfTime()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0.3f;
        }
        else Time.timeScale = 1f;
    }
}
