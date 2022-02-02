using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine.Events;

public class GameSession : MonoBehaviour
{
    public int playerLives = 3;
    public int score = 0;
    public int health = 100;
    [SerializeField] float waitForRespawn = 0.5f;

    UnityEvent OnPlayerKilled;
    UnityEvent OnScoreGain;

    private void Awake() 
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1) 
        {
            Destroy(gameObject); 
        }
        else 
        {
            DontDestroyOnLoad(gameObject); 
        }      
    }
 
    private void Start()
    {
       
        OnPlayerKilled = StatsHandler.Instance.OnPlayerKilled;
        OnScoreGain = StatsHandler.Instance.OnScoreGain;
        
    }

    public void ProcessPlayerDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            FindObjectOfType<Player>().Die();
            StartCoroutine(TakeLife());
            health = 100;
        }
        
    }

    public void AddToScore(int pointsToAdd) 
    {
        score += pointsToAdd;
        OnScoreGain.Invoke();
    }

    public void AddToHealth(int healthToAdd)
    {
        if (health == 200)
        {
            playerLives++;
        }
        else
        {
            health = Mathf.Clamp(health + healthToAdd, 0, 200);                 
        }
    }

    IEnumerator TakeLife()
    {
        yield return new WaitForSecondsRealtime(waitForRespawn);
        playerLives--;
        OnPlayerKilled.Invoke();
        if (playerLives < 0)
        {
            ResetGameSession();
        }
        else
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }      
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject); 
    }
}
