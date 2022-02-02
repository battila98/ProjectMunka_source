using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] AudioClip healthPickupSFX;
    [SerializeField] int healthGain = 30;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<GameSession>().AddToHealth(healthGain);
        Destroy(gameObject);
    }
}
