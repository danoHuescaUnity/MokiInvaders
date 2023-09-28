using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private const string BOUNDARY = "Boundary";
    private const string DOWNLIMIT = "DownLimit";

    public EnemySpawner spawner = null;

    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float displacement = 1.0f;
    [SerializeField]
    private GameObject explosionVFX = null;

    [SerializeField]
    private int scoreToAdd = 10;

    private bool isInitialized = true;

    private void Awake()
    {
        GameManager.OnGameOver += OnGameOver;
    }

    void Update()
    {
        if (isInitialized)//we only want to move if the game is running, on GameOver we stop
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == BOUNDARY)
        {
            //if any enemy collides with any boundary we want all the enemies to change direction and move downwards
            spawner.MoveEnemiesDownwards();
        }

        else if (collision.gameObject.tag == DOWNLIMIT)
        {
            //if any enemy collides with the downlimit we send the GameOver instruction
            GameManager.instance.GameOver(false);
        }
    }

    public void ChangeDirection()
    {
        //we move to the opposite direction and downwards once
        speed *= -1;
        transform.position = new Vector2(transform.position.x, transform.position.y - displacement);
    }

    public void Destroy()
    {
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        GameManager.instance.AddScore(scoreToAdd);
        spawner.RemoveEnemy(this);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        isInitialized = false;
    }
}
