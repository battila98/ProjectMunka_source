using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowTimer;
    [SerializeField] float arrowDestroyTimer = 3f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy component))
        {
            Destroy(gameObject);
        }      
    }
    void Update()
    {
        StartCoroutine(DestroyArrow());      
    }
    IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
