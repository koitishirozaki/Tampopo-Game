using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class PlayerController : MonoBehaviour
{

    [Foldout("Movement", true)]
    public float swipeMultiplier;
    public float autoWalkSpeed;
    public float maxDistance;
    [ReadOnly] public bool autoWalk;
    public bool swipeHappened;

    [Foldout("Attack", true)]
    public int damage;
    public float minSpeedAtk;
    public float cooldown = 2f;
    public float noAtkWindow = 0.5f;
    public float atkWindow = 1f;
    [ReadOnly] public bool timerActive;
    [ReadOnly] public float maxTimer;
    [ReadOnly] public bool onCooldown;
    [ReadOnly] public bool canAttack;
    private bool onWait;
    [ReadOnly] public bool isTriggerON;
    [ReadOnly] public List<GameObject> hitStuff = new List<GameObject>();

    [Foldout("Rotation", true)]
    [ReadOnly] public float Angle;
    [ReadOnly] public float currentAngle;
    [ReadOnly] public float distanceSwiped;

    #region CHECKING FOR GROUND VARIABLES
    [Foldout("GROUND stuff", true)]
    [ReadOnly] public bool isGrounded;
    public Transform groundChecker;
    public float groundHeight = 0.01f;
    [Range(0, 1)] public float checkRateForGround = 0.5f;
    public float heightOffset = 0.05f;
    public LayerMask groundMask;
    #endregion

    [Foldout("References", true)]
    private LeanRoll leanRoll;
    private Rigidbody rb;
    private Animator pAnimator;
    private Collider pCollider;
    private CustomGravity customGravity;
    private SpriteRenderer spriteRenderer;
    private T_Health health;
    public Text playerstateText;

    // Start is called before the first frame update
    void Start()
    {
        pAnimator = GetComponentInChildren<Animator>();
        leanRoll = GetComponent<LeanRoll>();
        rb = GetComponent<Rigidbody>();
        customGravity = GetComponent<CustomGravity>();
        health = GetComponent<T_Health>();
        pCollider = GetComponent<Collider>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        autoWalk = false;
        canAttack = true;

        InvokeRepeating("GroundCheck", 0, checkRateForGround);
    }

    private void Update()
    {

        ResetCombo(timerActive);

        if (onCooldown) canAttack = false;
        else canAttack = true;
    }

    void FixedUpdate()
    {
        if (autoWalk) AutoWalk();
        else
        {
            pAnimator.SetBool("isWalking", false);
            pAnimator.SetBool("idle", true);
        }

        // Attack
        if (rb.velocity.magnitude > minSpeedAtk && canAttack) TriggerON();
        else if (rb.velocity.magnitude < minSpeedAtk && isTriggerON) TriggerOFF();
    }

    #region TIMER
    void ResetCombo(bool whatever)
    {
        if (whatever)
        {
            maxTimer -= Time.deltaTime;

            playerstateText.text = "WINDOW";
            playerstateText.color = Color.cyan;

            if (maxTimer <= 0)
            {
                timerActive = false;
                canAttack = false;
                maxTimer = 0;

                StartCoroutine(Cooldown());
            }
        }
    }

    IEnumerator Cooldown()
    {
        playerstateText.text = "COOLDOWN";
        playerstateText.color = Color.red;

        onCooldown = true;
        yield return new WaitForSecondsRealtime(cooldown);

        onCooldown = false;
        canAttack = true;

        playerstateText.text = "CAN ATTACK";
        playerstateText.color = Color.green;
    }
    #endregion

    #region MOVEMENT
    public void OnDistance(float distance)
    {
        distanceSwiped = distance;
        rb.velocity = Vector3.zero;

        // Dashing
        if (distanceSwiped != 0)
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
    }

    void AutoWalk()
    {
        Vector3 vector_autoWalk = transform.position + transform.forward * autoWalkSpeed;
        rb.MovePosition(vector_autoWalk);

        pAnimator.SetBool("isWalking", true);
        pAnimator.SetBool("idle", false);
    }

    public void Stop(LeanFinger leanFinger)
    {
        if (leanFinger.Tap)
        {
            pAnimator.SetBool("idle", true);

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

            if (Angle < 0) spriteRenderer.flipX = true;
            else spriteRenderer.flipX = false;

            float cameraAngle = Camera.main.transform.eulerAngles.y;
            float productAngle = cameraAngle + currentAngle;
            Quaternion lookAt = Quaternion.Euler(0, productAngle, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, 500 * Time.deltaTime);
            currentAngle = 0;
        }
    }
    #endregion

    
    #region GROUND CHECKER FUNCTION
    void GroundCheck()
    {

        if (Physics.Raycast(new Vector3(groundChecker.position.x, groundChecker.position.y + heightOffset, groundChecker.position.z), Vector3.down, groundHeight + heightOffset, groundMask))
        {
            isGrounded = true;
        }
        else isGrounded = false;


        if (!isGrounded)
        {
            customGravity.gravityScale = 5f;
        }
        else customGravity.gravityScale = 1f;
    }
    #endregion

    #region ATTACK
    public void TriggerON()
    {
        if (!onWait)
        {
            pAnimator.SetTrigger("Dashing");
            pAnimator.SetBool("idle", false);

            pCollider.isTrigger = true;

            onWait = true;

            health.Invencivel(true);
            spriteRenderer.color = Color.red;
            isTriggerON = true;
        }
    }

    public void TriggerOFF()
    {
        if (onWait)
        {
            if (hitStuff.Count > 0)
            {
                // ATK WINDOW AS USUAL
                timerActive = true;
                maxTimer = atkWindow;

                hitStuff.Clear();
            }
            else
            {
                // NO ATK WINDOW
                timerActive = true;
                maxTimer = noAtkWindow;
            }

            pCollider.isTrigger = false;
            onWait = false;

            Debug.Log("trigger should turn off");
            health.Invencivel(false);
            spriteRenderer.color = Color.white;

            isTriggerON = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && canAttack)
        {
            hitStuff.Add(other.gameObject);
            T_Health otherHealth = other.GetComponent<T_Health>();
            otherHealth.TakeSimpleDamage(damage);

            Debug.Log("enemy encountered!");
        }

        if (other.CompareTag("NPC"))
        {
            // do stuff if NPC
        }

        if (other.CompareTag("Object"))
        {
            hitStuff.Add(other.gameObject);
            // do stuff if interactable object
        }


    }

    #endregion
}
