using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
  [SerializeField] private float moveSpeed = 2f;
  [SerializeField] private float changeDirectionInterval = 5f;
  [SerializeField] private float rotationSpeed = 12f;
  private Vector3 moveDirection;
  private float timeSinceLastDirectionChange;
  private Rigidbody rb;
  private Animator animator;
  private Quaternion targetRotation;
  private enum BLOCKED_STATE { No, Player, Others };
  private struct BlockedInfo
  {
    public BLOCKED_STATE blockedState;
    public RaycastHit hit;
  }
  private float delayToChangeDirectionAfterStuck = 0.0f;
  private float delayToStuck = 0.0f;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    animator = GetComponent<Animator>();
    GetRandomDirection();
  }

  void FixedUpdate()
  {
    rb.velocity = moveDirection * moveSpeed;
    timeSinceLastDirectionChange += Time.fixedDeltaTime;
    if (timeSinceLastDirectionChange >= changeDirectionInterval)
    {
      GetRandomDirection();
    }
    BlockedInfo blockedInfo = IsBlocked();
    switch (blockedInfo.blockedState)
    {
      case BLOCKED_STATE.No:
        // Do nothing
        break;
      case BLOCKED_STATE.Player:
        // Stop and rotate towards the player
        moveDirection = Vector3.zero;
        Debug.Log("49: ZERO!");
        targetRotation = Quaternion.LookRotation(blockedInfo.hit.transform.position - transform.position, Vector3.up);
        break;
      case BLOCKED_STATE.Others:
        // Get stuck for 3 seconds
        if (delayToChangeDirectionAfterStuck >= 3.0f)
        {
          GetRandomDirection();
          delayToStuck = 3.0f;
          delayToChangeDirectionAfterStuck = 0f;
        }
        else
        {
          if (delayToStuck <= 0.0f)
          {
            moveDirection = Vector3.zero;
            Debug.Log("64: ZERO!");
            delayToChangeDirectionAfterStuck += Time.fixedDeltaTime;
          }
          else
          {
            delayToStuck -= Time.fixedDeltaTime;
          }
        }
        break;
    }
    if (rb.velocity.magnitude < 0.01f)
    {
      animator.SetBool("isWalking", false);
    }
    else
    {
      animator.SetBool("isWalking", true);
    }
    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
  }

  private void GetRandomDirection()
  {
    Vector3 newDirection = Vector3.zero;
    float randomValue = Random.Range(0f, 1f);
    if (randomValue > 0.1f)
    {
      newDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }
    moveDirection = newDirection;
    targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
    timeSinceLastDirectionChange = 0f;
  }

  private BlockedInfo IsBlocked()
  {
    int numRaysAround = 8;
    float angleIncrement = 360f / numRaysAround;
    float raycastDistance = 1f;
    RaycastHit hit;
    Vector3 raycastOrigin = transform.position + Vector3.up * 0.5f;
    for (int i = 0; i < numRaysAround; i++)
    {
      Quaternion raycastRotation = Quaternion.AngleAxis(angleIncrement * i, Vector3.up);
      Vector3 direction = raycastRotation * transform.forward;
      if (Physics.Raycast(raycastOrigin, direction, out hit, raycastDistance))
      {
        if (hit.collider.CompareTag("Player"))
        {
          return new BlockedInfo { blockedState = BLOCKED_STATE.Player, hit = hit };
        }
        else
        {
          return new BlockedInfo { blockedState = BLOCKED_STATE.Others, hit = hit };
        }
      }
    }
    return new BlockedInfo { blockedState = BLOCKED_STATE.No, hit = default(RaycastHit) };
  }
}
