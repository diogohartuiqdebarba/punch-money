using UnityEngine;
using System.Collections;

public class EnemyStack : MonoBehaviour
{

  private float stackOffset = 1f;
  private GameObject player;
  private GameObject enemyParent;
  private bool isStacked = false;

  void Start()
  {
    player = GameObject.FindWithTag("Player");
    enemyParent = GameObject.FindWithTag("EnemyStackParent");
  }

  void Update()
  {
    if (isStacked && transform.parent != enemyParent.transform)
    {
      transform.position = enemyParent.transform.position + (enemyParent.transform.up * stackOffset);
      transform.rotation = enemyParent.transform.rotation;
      transform.SetParent(enemyParent.transform);
      float totalMass = 0f;
      foreach (Transform child in enemyParent.transform)
      {
        if (child.CompareTag("Enemy"))
        {
          totalMass += child.GetComponent<Rigidbody>().mass;
        }
      }
      Transform bottomEnemy = enemyParent.transform.GetChild(0);
      Rigidbody bottomRb = bottomEnemy.GetComponent<Rigidbody>();
      float forceMagnitude = totalMass * 0.5f; // adjust this value to control the movement
      bottomRb.AddForce(enemyParent.transform.forward * forceMagnitude, ForceMode.Impulse);
    }
  }

  public void Stack()
  {
    isStacked = true;
    GetComponent<Collider>().enabled = false;
    Rigidbody rb = GetComponent<Rigidbody>();
    rb.isKinematic = true;
    rb.useGravity = false;
    // Stack enemies on top of each other in the enemyStackParent
    Transform enemyStack = enemyParent.transform;
    Vector3 stackPosition = enemyStack.position + (enemyStack.up * (stackOffset * enemyStack.childCount));
    transform.SetParent(enemyStack);
    transform.position = stackPosition;
  }

}
