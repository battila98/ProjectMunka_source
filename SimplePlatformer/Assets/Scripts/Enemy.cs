using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    UnityEvent OnEnemyKilled;

    private void Start()
    {
        OnEnemyKilled = StatsHandler.Instance.OnEnemyKilled;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.StartsWith("Arrow"))
        {
            OnEnemyKilled.Invoke();
            Destroy(gameObject);
        }
    }
}
