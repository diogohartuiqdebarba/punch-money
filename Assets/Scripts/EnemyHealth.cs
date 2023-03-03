using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
  public int maxHealth = 100;

  private int currentHealth;

  public UnityEvent onDeath;

  void Start()
  {
    currentHealth = maxHealth;
  }

  public void TakeDamage(int damage = 100)
  {
    currentHealth -= damage;
    if (currentHealth <= 0)
    {
      currentHealth = 0;
      Die();
    }
  }

  private void Die()
  {
    onDeath.Invoke();
    Destroy(gameObject);
  }
}
