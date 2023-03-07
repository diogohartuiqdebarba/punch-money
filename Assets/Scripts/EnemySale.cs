using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySale : MonoBehaviour
{
  [SerializeField] private Renderer moneyPlaceRenderer;
  private GameObject enemyStackParent;
  private PlayerController playerController;
  private Color originalColor;

  void Start()
  {
    originalColor = moneyPlaceRenderer.material.color;
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
          playerController.AddMoney(10);
          playerController.enemiesStacked = 0;
        }
      }
    }
  }

  public void BlinkMoneySalePlace()
  {
    StartCoroutine(BlinkCoroutine(1.5f));
  }

  IEnumerator BlinkCoroutine(float blinkDuration)
  {
    float blinkTime = 0.5f;
    Color blinkColor = Color.red;
    float timer = blinkDuration;
    while (timer > 0)
    {
      moneyPlaceRenderer.material.color = blinkColor;
      yield return new WaitForSeconds(blinkTime / 2);
      moneyPlaceRenderer.material.color = originalColor;
      yield return new WaitForSeconds(blinkTime / 2);
      timer -= blinkTime;
    }
  }

}
