using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EnemyStack : MonoBehaviour
{
  public UnityEvent onStack;
  private float stackOffset = 0.75f;
  private GameObject player;
  private GameObject enemyParent;
  private bool isStacked = false;
  private int stackIndex;
  private Quaternion rotation;
  private float stackTime = 0f;
  private float returnToCenterSpeed = 3f;
  private PlayerController playerController;
  private float amplitude = 2f;
  private float frequency = 1f;

  void Start()
  {
    player = GameObject.FindWithTag("Player");
    playerController = player.GetComponent<PlayerController>();
    enemyParent = GameObject.FindWithTag("EnemyStackParent");
  }

  void Update()
  {
    if (isStacked)
    {
      stackTime += Time.deltaTime;
      if (playerController.isWalking)
      {
        // I use Mathf.PingPong() to create a smooth oscillation between -1 and 1, 
        // and then apply Mathf.SmoothStep() to create a smooth transition between 
        // - amplitude and + amplitude based on the current stack index. 
        // The top of the stack will have a larger t value and thus a larger horizontalOffset, 
        // while the bottom of the stack will have a smaller t value and thus a smaller horizontalOffset.
        float t = Mathf.Clamp01((float)stackIndex / (float)enemyParent.transform.childCount);
        float horizontalOffset = amplitude * Mathf.SmoothStep(-1f, 1f, Mathf.PingPong(stackTime * frequency, 1f)) * t;
        transform.position = enemyParent.transform.position
            + (enemyParent.transform.forward * stackOffset * stackIndex)
            + (enemyParent.transform.right * horizontalOffset);
      }
      else
      {
        transform.position = Vector3.Lerp(
            transform.position,
            enemyParent.transform.position + (enemyParent.transform.forward * stackOffset * stackIndex),
            returnToCenterSpeed * Time.deltaTime
        );
      }
      transform.rotation = enemyParent.transform.rotation * rotation;
    }
  }

  public void Stack()
  {
    isStacked = true;
    stackTime = Time.time;
    GetComponent<Collider>().enabled = false;
    Rigidbody rb = GetComponent<Rigidbody>();
    rb.isKinematic = true;
    rb.useGravity = false;
    stackIndex = enemyParent.transform.childCount;
    float randomRotation = Random.Range(-10f, 10f);
    rotation = Quaternion.Euler(0f, 0f, randomRotation);
    transform.rotation = enemyParent.transform.rotation * rotation;
    transform.SetParent(enemyParent.transform);
    onStack.Invoke();
  }

  private float WaveFunction(float time, int stackPosition)
  {
    float amplitude = 2f - (stackPosition * 0.25f);
    float frequency = 1f + (stackPosition * 0.1f);
    float horizontalOffset = amplitude * Mathf.Sin(frequency * time);
    return horizontalOffset;
  }
}
