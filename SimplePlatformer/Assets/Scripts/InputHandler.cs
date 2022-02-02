using UnityEngine;

class InputHandler : MonoBehaviour
{
    [SerializeField] float shootTimer; 
    [SerializeField] float timeBetweenShots = 1f;

    Movement movement;

    void Start()
    {
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        shootTimer += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && shootTimer > timeBetweenShots) 
        {
            movement.FireBow();
            shootTimer = 0f;
        }
    }

}

