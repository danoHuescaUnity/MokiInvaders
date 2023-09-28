using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private const string ENEMY = "Enemy";
    private const string BOUNDARY = "Boundary";

    [SerializeField]
    private float beamSpeed = 1.0f;

    private void Awake()
    {
        GameManager.OnGameOver += OnGameOver;
    }

    void Update()
    {
        transform.Translate(Vector2.up * beamSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ENEMY)
        {
            collision.gameObject.GetComponent<EnemyController>().Destroy();
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == BOUNDARY)
        {
            Destroy(gameObject);
        }

    }

    private void OnGameOver()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= OnGameOver;
    }
}
