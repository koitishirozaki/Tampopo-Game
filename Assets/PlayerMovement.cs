using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public enum PlayerState
{
    ATTACK,
    COOLDOWN,
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState playerState;

    [Foldout("Movement", true)]
    public float swipeMultiplier;
    public float autoWalkSpeed;
    public float maxDistance;
    [ReadOnly] public bool autoWalk;

    [Foldout("Attack", true)]
    public float minSpeedAtk;

    // TIMER
    [ReadOnly] public bool timerActive;
    [ReadOnly] public float timeCount;

    public float atkWindow = 1f;
    public float cooldown = 2f;

    [ReadOnly] public bool hardCooldown;
    [ReadOnly] public bool canAttack;


    [Foldout("Rotation", true)]
    [ReadOnly] public float Angle;
    [ReadOnly] public float currentAngle;
    [ReadOnly] public float distanceSwiped;


    public Text playerSpeed;
    private LeanRoll leanRoll;
    private Rigidbody rb;
    private PlayerAttack pAttk;
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        leanRoll = GetComponent<LeanRoll>();
        rb = GetComponent<Rigidbody>();
        pAttk = GetComponent<PlayerAttack>();

        autoWalk = false;
        canAttack = true;
        timeCount = 0;
    }

    private void Update()
    {
        playerSpeed.text = rb.velocity.magnitude.ToString();

        if(timerActive)
        timeCount += Time.deltaTime;

        // Attack Window
        if (timeCount > atkWindow)
        {
            timerActive = false;
            timeCount = 0;

            Debug.Log("cant attack window");
            hardCooldown = true;
             
        }
        if(hardCooldown)
        {
            canAttack = false;

            timeCount += Time.deltaTime;

            if (timeCount > cooldown)
            {
                canAttack = true;
                hardCooldown = false;

                timeCount = 0;
            }
        }

    }

    void FixedUpdate()
    {

        if (autoWalk) AutoWalk();

        // Attack
        if (rb.velocity.magnitude > minSpeedAtk && canAttack) pAttk.TriggerON();
        else if (rb.velocity.magnitude < minSpeedAtk) pAttk.TriggerOFF();
    }

    
    public void OnDistance(float distance)
    {
        distanceSwiped = distance;
        rb.velocity = Vector3.zero;

    
        // Dashing
        if (distanceSwiped != 0 && canAttack)
        {
            StartCoroutine(Dash());
        }
    }

    public IEnumerator Dash()
    {
        distanceSwiped *= swipeMultiplier;
        if (distanceSwiped > maxDistance)
            distanceSwiped = maxDistance;

        Vector3 vectorApplied = transform.position + transform.forward * distanceSwiped;

        rb.AddForce(vectorApplied);

        yield return new WaitForSeconds(0.2f);
        distanceSwiped = 0;
        autoWalk = true;

        timerActive = true;
    }

    
    void AutoWalk()
    {
        Vector3 vector_autoWalk = transform.position + transform.forward * autoWalkSpeed;
        rb.MovePosition(vector_autoWalk);
    }
    

    public void Stop(LeanFinger leanFinger)
    {
        if (leanFinger.Tap)
        {
            autoWalk = false;
            canAttack = true;
            rb.velocity = Vector3.zero;
        }
    }

    public void RotatePlayer(Vector2 delta)
    {
        if (delta.sqrMagnitude > 0.0f)
        {
            Angle = Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg;
            var factor = LeanTouch.GetDampenFactor(-1, Time.deltaTime);
            currentAngle = Mathf.LerpAngle(currentAngle, Angle, factor);

            if(Angle < 0) spriteRenderer.flipX = true;
            else spriteRenderer.flipX = false;

            float cameraAngle = Camera.main.transform.eulerAngles.y;
            float productAngle = cameraAngle + currentAngle;
            Quaternion lookAt = Quaternion.Euler(0, productAngle, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, 500 * Time.deltaTime);
            currentAngle = 0;
        }

    }

    

}
