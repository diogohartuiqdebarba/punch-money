using UnityEngine;

public class EnemyAI : MonoBehaviour
{
  public float moveSpeed = 2f;
  public float changeDirectionInterval = 5f;
  private Vector3 moveDirection;
  private float timeSinceLastDirectionChange;
  private Rigidbody rb;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    moveDirection = GetRandomDirection();
  }

  void FixedUpdate()
  {
    rb.velocity = moveDirection * moveSpeed;
    timeSinceLastDirectionChange += Time.fixedDeltaTime;
    if (timeSinceLastDirectionChange >= changeDirectionInterval)
    {
      moveDirection = GetRandomDirection();
      timeSinceLastDirectionChange = 0f;
    }
  }

  private Vector3 GetRandomDirection()
  {
    return new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
  }
}

