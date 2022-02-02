using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    // Config  
    [SerializeField] Vector2 KnockUp = new Vector2(0f, 15f);
    
    [SerializeField] AudioClip jumpSFX;

    // State
    bool isAlive = true;

    //Cashed comp. refs.
    public Animator myAnimator;

    Movement movement;
    UnityEvent OnHealthLost;

    void Start()
    {
        movement = GetComponent<Movement>();        
        myAnimator = GetComponent<Animator>();       
        OnHealthLost = StatsHandler.Instance.OnHealthLost;
    }

    void Update()
    {
        if (!isAlive) { return; }
        movement.Run();
        movement.FlipSprite();
        movement.horizontalInput = Input.GetAxis("Horizontal");
        movement.JumpV2();
        movement.ClimbLadder();
        movement.Catapulted();
       
    }

    public void Die()
    {
        myAnimator.SetTrigger("Dying");
        GetComponent<Rigidbody2D>().velocity = KnockUp;
        isAlive = false;      
    }

    void OnCollisionEnter2D(Collision2D collision)
    {        
        if (collision.gameObject.layer == 13 || collision.gameObject.layer == 12) 
        {
            FindObjectOfType<GameSession>().ProcessPlayerDamage(10);
            OnHealthLost.Invoke();
            GetComponent<Rigidbody2D>().velocity = KnockUp;
        }
    }
 
}
