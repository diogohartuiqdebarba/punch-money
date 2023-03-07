using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySale : MonoBehaviour
{
  private GameObject enemyStackParent;
  private PlayerController playerController;

  void Start()
  {
    enemyStackParent = GameObject.FindWithTag("EnemyStackParent");
    playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      if (enemyStackParent != null)
      {
        foreach (Transform child in enemyStackParent.transform)
        {
          EnemyHealth enemyHealth = child.GetComponent<EnemyHealth>();
          enemyHealth.Die();
          int money = Random.Range(10, 26);
          playerController.AddMoney(money);
        }
      }
    }
  }
}
