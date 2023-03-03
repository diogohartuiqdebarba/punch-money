using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
  public GameObject enemyPrefab;
  public int numEnemiesToSpawn = 5;
  public float spawnDelay = 1f;

  private int numEnemiesAlive;

  void Start()
  {
    StartCoroutine(SpawnEnemies(numEnemiesToSpawn));
  }

  void OnEnemyDeath()
  {
    numEnemiesAlive--;
    if (numEnemiesAlive <= 0)
    {
      numEnemiesToSpawn++;
      SpawnEnemies(numEnemiesToSpawn);
    }
  }

  IEnumerator SpawnEnemies(int numEnemies)
  {
    numEnemiesAlive = numEnemies;
    for (int i = 0; i < numEnemies; i++)
    {
      Vector3 randomPos = new Vector3(Random.Range(-6f, 6f), 0.48f, Random.Range(-6f, 6f));
      Instantiate(enemyPrefab, randomPos, Quaternion.identity)
          .GetComponent<EnemyHealth>().onDeath.AddListener(OnEnemyDeath);
      yield return new WaitForSeconds(spawnDelay);
    }
  }
}
