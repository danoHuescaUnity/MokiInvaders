using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy = null;
    [SerializeField]
    private float spawnRate = 1.0f;
    [SerializeField]
    private int enemyColums = 10;
    [SerializeField]
    private int enemyRows = 5;

    [SerializeField]
    private float horizontalOffset = 1.5f;
    [SerializeField]
    private float verticalOffset = 1f;
    [SerializeField]
    private float horizontalStart = -7f;
    [SerializeField]
    private float verticalStart = 3.5f;

    private List<EnemyController> spawnedEnemies = new List<EnemyController>();
    private bool isChanginDirection = false;

    private void Awake()
    {
        GameManager.OnInitialized += Initilize;
        GameManager.OnReset += OnReset;
    }

    private void Initilize()
    {
        SpawnEnemies();
    }

    private void OnReset()
    {
        if (spawnedEnemies.Count > 0)
        {
            foreach (EnemyController enemy in spawnedEnemies)
            {
                Destroy(enemy.gameObject);
            }
            spawnedEnemies.Clear();
        }
    }

    public void SpawnEnemies()
    {
        //We Iterate trouch each row and colum to instantiate each enemy
        for (int i = 0; i < enemyRows; i++)
        {
            for (int j = 0; j < enemyColums; j++)
            {
                //we add an offset for each instantiation
                EnemyController newEnemy = Instantiate(enemy, new Vector3(horizontalStart + (j * horizontalOffset), verticalStart - (i * verticalOffset), 0), Quaternion.identity).GetComponent<EnemyController>();
                spawnedEnemies.Add(newEnemy);//we store the enemies on a list
                newEnemy.spawner = this;// we asign the enemy spawner to each enemy
            }
        }
    }

    public void MoveEnemiesDownwards()
    {
        if (isChanginDirection)// we add this early return so we avoid run this function for each time an enemy collides with a boundary
        {
            return;
        }

        isChanginDirection = true;
        foreach (EnemyController enemy in spawnedEnemies)
        {
            enemy.ChangeDirection();
        }

        Invoke("ResetIsChangingDirectionFlag", 0.5f);//here we reset the flag so we can change direction again
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        //Removes the enemy from the list to avid missing reference, if the count on the list after removing the enemy is == 0 the player won
        spawnedEnemies.Remove(enemy);
        if (spawnedEnemies.Count == 0)
        {
            GameManager.instance.GameOver(true);
        }
    }

    private void ResetIsChangingDirectionFlag()
    {
        isChanginDirection = false;
    }

    private void OnDestroy()
    {
        GameManager.OnInitialized -= Initilize;
        GameManager.OnReset -= OnReset;
    }
}
