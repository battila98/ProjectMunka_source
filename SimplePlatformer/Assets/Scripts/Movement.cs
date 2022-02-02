using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{    
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float jumpVelocity = 17.5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float fallMultiplier = 2.26f;
    [SerializeField] float lowJumpMultiplier = 1.8f;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float arrowSpeedY = 0.05f;
    [SerializeField] float arrowSpeedX = 43f;
    
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    BoxCollider2D myFeet;
    UnityEvent OnArrowShot;
    UnityEvent OnJump;

    float gravityScaleAtStart;
    public float horizontalInput; 
    float faceDirection = 1f;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;        
        OnArrowShot = StatsHandler.Instance.OnArrowShot;
        OnJump = StatsHandler.Instance.OnJump;

    }
    void Update()
    {
        if (horizontalInput > 0)
        {
            faceDirection = 1f;
        }
        else if (horizontalInput < 0)
        {
            faceDirection = -1f;
        }
    }

    public void FlipSprite()
    {       
        transform.localScale = new Vector2(faceDirection, transform.localScale.y);
    }

    public void FireBow()
    {
        if (Input.GetButtonDown("Fire1"))
        {  
            StartCoroutine(ShootArrow());           
            OnArrowShot.Invoke();
        }
    }

    IEnumerator ShootArrow()
    {
        myAnimator.SetBool("Shooting", true);
        yield return new WaitForSecondsRealtime(0.25f);
        Vector2 startingArrowPosition = new Vector2(transform.position.x + faceDirection * 0.75f, transform.position.y - 0.2f); 
        GameObject arrow = Instantiate(arrowPrefab, startingArrowPosition, Quaternion.identity)
            as GameObject;
        arrow.name = "Arrow";
        arrow.transform.localScale = new Vector2(faceDirection, transform.localScale.y);
        arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(arrowSpeedX * faceDirection, arrowSpeedY);
        yield return new WaitForSecondsRealtime(0.25f);
        myAnimator.SetBool("Shooting", false);
    }

    public void Run()
    {            
        
        Vector2 playerVelocity = new Vector2(horizontalInput * runSpeed, myRigidBody.velocity.y); 
        myRigidBody.velocity = playerVelocity;
      
        bool playerRunning = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerRunning);
    }

    public void ClimbLadder()
    {
        myRigidBody.gravityScale = gravityScaleAtStart;
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            myAnimator.SetBool("Climbing", false);
            return;
        }
        if (myRigidBody.velocity.y > climbSpeed) 
        {
            return;
        }
        float climbingDirection = Input.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, climbingDirection * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon; 
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
        JumpV2();
    }

    void IsFalling()
    {
        if (myRigidBody.velocity.y < 0)
        {
            myRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (myRigidBody.velocity.y > 0.08 && !Input.GetButton("Jump"))
        {
            myRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime; 
        }
    }

    public void JumpV2()
    {
        IsFalling();
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) && 
            !myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing")) &&
            !myFeet.IsTouchingLayers(LayerMask.GetMask("Projectile")))
        {
            return;
        }
        if (Input.GetButtonDown("Jump"))
        {
            myRigidBody.velocity = Vector2.up * jumpVelocity;
            OnJump.Invoke();
        }       
    }
    
    public void Catapulted()
    {
        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Bouncy")))
        {
            StartCoroutine(FindObjectOfType<Bouncy>().PushUp());
        }
    }
}
