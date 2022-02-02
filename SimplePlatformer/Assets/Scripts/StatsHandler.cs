using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using System.IO;
using System;

public class StatsHandler 
{
    public readonly UnityEvent OnEnemyKilled = new UnityEvent();
    public readonly UnityEvent OnPlayerKilled = new UnityEvent();
    public readonly UnityEvent OnArrowShot = new UnityEvent();
    public readonly UnityEvent OnHealthLost = new UnityEvent();
    public readonly UnityEvent OnJump = new UnityEvent();
    public readonly UnityEvent OnScoreGain = new UnityEvent();

    private static StatsHandler instance = null;
    public Stats Stats => stats;
    public static StatsHandler Instance
    { 
        get 
        {
            if (instance == null)
            {
                instance = new StatsHandler();
            }
            return instance;
        } 
    }

    Stats stats;
    string path;

    private StatsHandler()
    {
        path = Environment.CurrentDirectory + @"/Assets/Resources/";
      
        OnEnemyKilled.AddListener(() => stats.EnemiesKilled++);
        OnPlayerKilled.AddListener(() => { stats.PlayerDeaths++; WriteSaves(); });
        OnArrowShot.AddListener(() => stats.ArrowsShot++);
        OnHealthLost.AddListener(() => stats.HealthLost += 10);
        OnJump.AddListener(() => stats.Jumps++);
        OnScoreGain.AddListener(() => stats.Score += 100);

        ReadSaves();
    }
   
    private void Start()
    {
            
    }

    private void ReadSaves()
    {        
        stats = JsonUtility.FromJson<Stats>(File.ReadAllText(path + "stats.json"));       
    }

    public void WriteSaves()
    {     
        string toJason = JsonUtility.ToJson(stats);       
        if (stats != null)
        {
            File.WriteAllText(path + "stats.json", JsonUtility.ToJson(stats));           
        }       
    }

    private void OnApplicationQuit()
    {
        WriteSaves();       
    }
}
