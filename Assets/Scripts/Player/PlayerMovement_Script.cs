using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Script : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D boxc;
    private Animator anim;
    private SpriteRenderer sr;
    private AudioSource aud;
    
    public AudioClip jumpSound;
    public AudioClip deathSound;
    
    [SerializeField] public ParticleSystem dust;
    [SerializeField] private LayerMask layerMaskPlatform;

    [Header("Physics")]
    public float gravityScale = 2.0f;
    public float maxSpeed = 6f;
    public float currentMoveSpeed;
    public float extraHeight = 0.05f;
    public float jumpForce = 7f;
    public float xMoveSpeed = 300.0f;

    [Header("Jump Variables")]
    public bool jumpStart = false;
    public bool jumpStop = false;
    public bool grounded = false;
    public bool isGravity;
    public bool timerStart = false;
    public float timerValue = 0.3f;
    public float timer;

    [Header("Horizontal Movement")]
    public bool turnedLeft = false;
    public bool turnedRight = false;
    public bool xMovingBool = false;
    public float xMovingAxisFloat = 0.0f;
    public bool xMovingLeft = false;
    public bool xMovingRight = false;

    [Header("Statuses")]
    public bool isDying = false;
    public bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        aud = GetComponent<AudioSource>();

        timer = timerValue;
    }

    void Update()
    {
        if (!isDead)
        {
            SetMovementBooleans();
            IsGrounded();
            SetAnimator();
        }
        else
        {
            CommenceDying();
        }
    }

    private void FixedUpdate()
    {
        if (jumpStart)
        {
            StartJump();
        }
        if (jumpStop)
        {
            StopJump();
        }
       
        if(xMovingRight)
        {
            MoveRight();
            FlipSpriteRight();

        } else if (xMovingLeft)
        {
            MoveLeft();
            FlipSpriteLeft();

        } else if (!xMovingBool)
        {
            StopMoving();
        }
    }

    //Sets basic booleans based on user inputs, and ticks down jump cooldowns.
    public void SetMovementBooleans()
    {
        xMovingAxisFloat = Input.GetAxis("Horizontal");

        if (xMovingAxisFloat > 0)
        {

            xMovingBool = true;
            xMovingLeft = false;
            xMovingRight = true;
        }
        else if (xMovingAxisFloat < 0)
        {

            xMovingBool = true;
            xMovingLeft = true;
            xMovingRight = false;
        }
        else
        {

            xMovingBool = false;
            xMovingLeft = false;
            xMovingRight = false;
        }

        if (Input.GetButton("Jump") && IsGrounded())
        {
            jumpStart = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            jumpStop = true;
        }

        //Jump extension timer
        if (timerStart)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                jumpStop = true;
            }
        }
    }

    //Sets horizontal movement to 0
    public void StopMoving() {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    //Sets character velocity to X value from -1 to 1, adjusted for speed and gradual speed up.
    public void MoveRight()
    {
        rb.velocity = new Vector2(xMovingAxisFloat * xMoveSpeed * Time.deltaTime, rb.velocity.y);
        if (rb.velocity.x >= maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }

    }

    //Sets character velocity to X value from -1 to 1, adjusted for speed and gradual speed up. Yes it's the same as the previous one. Don't ask.
    public void MoveLeft()
    {
        rb.velocity = new Vector2(xMovingAxisFloat * xMoveSpeed * Time.deltaTime, rb.velocity.y);
        if (rb.velocity.x <= (maxSpeed * -1))
        {
            rb.velocity = new Vector2(maxSpeed * -1, rb.velocity.y);
        }
        currentMoveSpeed = rb.velocity.x;
    }

    //Flips the sprite component on its X axis. Also sets booleans.
    private void FlipSpriteLeft()
    {
        sr.flipX = true;
        //transform.localScale = new Vector3(-1, 1, 1);
        turnedLeft = true;
        if(turnedRight && grounded)
        {
            KickDust();
            turnedRight = false;
        }
    }

    //Flips the sprite component on its X axis. Also sets booleans.
    private void FlipSpriteRight()
    {
        sr.flipX = false;
        //transform.localScale = new Vector3(1, 1, 1);
        turnedRight = true;
        if (turnedLeft && grounded)
        {
            KickDust();
            turnedLeft = false;
        }
    }

    //Propels the character up on the Y axis by applying Force Impulse. Disables effect of gravity. Enables the timer boolean.
    public void StartJump()
    {
        aud.PlayOneShot(jumpSound, 0.5f);
        timerStart = true;
        KickDust();
        rb.gravityScale = 0;
        isGravity = false;
        rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
        jumpStart = false;
    }

    //Restores gravity.
    public void StopJump()
    {
        timerStart = false;
        rb.gravityScale = gravityScale;
        isGravity = true;
        jumpStop = false;
        timer = timerValue;
    }

    //Throws a BoxCast to check for a Ground Layer Object below the character
    private bool IsGrounded()
    {
        RaycastHit2D raycast_hit = Physics2D.BoxCast(boxc.bounds.center, boxc.bounds.size, 0f, Vector2.down, extraHeight, layerMaskPlatform);

        if (raycast_hit.collider != null)
        {
            grounded = true;
            DebugRaycasts(Color.green, extraHeight);
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
        else
        {
            if (grounded)
            {
                grounded = false;
                KickDust();
            }
            DebugRaycasts(Color.red, extraHeight);
        }

        return raycast_hit.collider != null;
    }

    private void SetAnimator()
    {
        if(rb.velocity.y < -0.2f)
        {
            SetAnimatorJumping(false);
            SetAnimatorFalling(true);
        }
        else if (!grounded)
        {
            SetAnimatorJumping(true);
        }
        else if (xMovingBool)
        {
            SetAnimatorFalling(false);
            SetAnimatorJumping(false);
            SetAnimatorRunning(xMovingBool);
        }
        else
        {
            SetAnimatorFalling(false);
            SetAnimatorJumping(!grounded);
            SetAnimatorRunning(false);
        }
    }

    //Sets a boolean for the "running" animator property, according the character's moving state.
    private void SetAnimatorRunning(bool boolean)
    {
        anim.SetBool("running", boolean); 
    }

    private void SetAnimatorJumping(bool boolean)
    {
        anim.SetBool("jumping", boolean);
    }

    private void SetAnimatorFalling(bool boolean)
    {
        anim.SetBool("falling", boolean);
    }

    public void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "platform")
        {
            transform.parent = other.transform;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "platform")
        {
            transform.parent = null;
        }
    }

    //Throws the dust effect
    private void KickDust()
    {
        dust.Play();
    }

    private void CommenceDying()
    {
        rb.gravityScale = 0;
        xMovingAxisFloat = 0;
        rb.velocity = new Vector2(0,0);
        anim.SetBool("running", false);
    }

    //Debugging tools
    private void DebugRaycasts(Color rayColor, float extra_height)
    {
        Debug.DrawRay(boxc.bounds.center + new Vector3(boxc.bounds.extents.x, 0), Vector3.down * (boxc.bounds.extents.y + extra_height), rayColor);
        Debug.DrawRay(boxc.bounds.center - new Vector3(boxc.bounds.extents.x, 0), Vector3.down * (boxc.bounds.extents.y + extra_height), rayColor);
        Debug.DrawRay(boxc.bounds.center - new Vector3(boxc.bounds.extents.x, boxc.bounds.extents.y + extra_height), Vector2.right * (boxc.bounds.extents.x * 2), rayColor);
    }
    private void OnGUI()
    {
        GUIStyle font_size = new GUIStyle(GUI.skin.GetStyle("label"));
        font_size.fontSize = 18;

        GUI.Label(new Rect(10, 10, 400, 50), "Grounded: " + IsGrounded().ToString(), font_size);
        GUI.Label(new Rect(10, 50, 400, 50), "X_Moving: " + xMovingLeft.ToString(), font_size);
    }
}







//if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
//{
//    xMovingAxisFloat = 1;
//    xMovingBool = true;
//    xMovingLeft = false;
//    xMovingRight = true;
//}
//else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
//{
//    xMovingAxisFloat = -1;
//    xMovingBool = true;
//    xMovingLeft = true;
//    xMovingRight = false;
//}
//else
//{
//    xMovingAxisFloat = 0;
//    xMovingBool = false;
//    xMovingLeft = false;
//    xMovingRight = false;
//} 
