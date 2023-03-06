using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
  private int currentHealth;
  public UnityEvent onDeath;

  private void Die()
  {
    onDeath.Invoke();
    Destroy(gameObject);
  }
}
