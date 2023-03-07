using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PunchController : MonoBehaviour
{
  [SerializeField] private Image punchImage;
  private float punchForce = 100f;
  private float punchDuration = 0.1f;
  private bool isPunching = false;
  private Rigidbody punchTarget;
  private Rigidbody characterRb;
  private Coroutine punchFXTask;
  private PlayerController playerController;

  private void Start()
  {
    punchImage.enabled = false;
    characterRb = GetComponentInParent<Rigidbody>();
    playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
  }

  private void OnTriggerEnter(Collider other)
  {
    EnemyAI enemyScript = other.GetComponent<EnemyAI>();
    if (other.CompareTag("Enemy") && playerController.CanStack() && !isPunching && enemyScript.enabledAI)
    {
      isPunching = true;
      punchTarget = other.GetComponent<Rigidbody>();
      playerController.enemiesStacked++;
      StartCoroutine(PunchRoutine(other.gameObject));
      enemyScript.enabledAI = false;
    }
  }

  private IEnumerator PunchRoutine(GameObject enemy)
  {
    punchFXTask = StartCoroutine(PunchFX(enemy));
    Vector3 direction = punchTarget.transform.position - transform.position;
    direction.y = 0f;
    direction.Normalize();
    punchTarget.AddForce(direction * punchForce, ForceMode.Impulse);
    StartCoroutine(RotateEnemy(enemy));
    yield return new WaitForSeconds(punchDuration);
    enemy.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    StartCoroutine(BlinkRoutine(enemy));
    isPunching = false;
  }

  private IEnumerator RotateEnemy(GameObject enemy)
  {
    Vector3 rotation = new Vector3(-90f, 0f, 0f);
    float duration = 0.6f;
    float elapsedTime = 0f;
    Quaternion startRotation = enemy.transform.rotation;
    while (elapsedTime < duration)
    {
      elapsedTime += Time.deltaTime;
      float zRotation = Mathf.PingPong(elapsedTime * 10f, 360f); // Change the z rotation each 0.1 seconds
      Quaternion targetRotation = Quaternion.Euler(rotation.x, rotation.y, zRotation);
      enemy.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
      yield return null;
    }
  }

  private IEnumerator BlinkRoutine(GameObject enemy)
  {
    for (int i = 0; i < 9; i++)
    {
      enemy.SetActive(false);
      yield return new WaitForSeconds(0.1f);
      enemy.SetActive(true);
      yield return new WaitForSeconds(0.1f);
    }
    EnemyStack enemyStack = enemy.GetComponent<EnemyStack>();
    enemyStack.Stack();
  }

  private IEnumerator PunchFX(GameObject enemy)
  {
    if (punchFXTask != null)
    {
      StopCoroutine(punchFXTask);
    }
    Vector3 originalScale = new Vector3(1, 1, 1);
    punchImage.transform.localScale = originalScale;
    punchImage.color = Color.white;
    SetPunchFXPosition(enemy);
    punchImage.enabled = true;
    float scaleDuration = 1f;
    float scaleElapsedTime = 0f;
    float maxScale = 4.2f;
    while (scaleElapsedTime < scaleDuration)
    {
      scaleElapsedTime += Time.deltaTime;
      float t = Mathf.Clamp01(scaleElapsedTime / scaleDuration);
      float scale = Mathf.Lerp(1f, maxScale, t);
      punchImage.transform.localScale = originalScale * scale;
      Color imageColor = punchImage.color;
      imageColor.a = Mathf.Lerp(1f, 0f, t);
      punchImage.color = imageColor;
      yield return null;
    }
    punchImage.transform.localScale = originalScale;
    punchImage.enabled = false;
  }

  private void SetPunchFXPosition(GameObject enemy)
  {
    Vector3 playerPosition = enemy.transform.position;
    Vector3 screenPosition = Camera.main.WorldToScreenPoint(playerPosition);
    RectTransform canvasRectTransform = punchImage.canvas.GetComponent<RectTransform>();
    Vector2 localPosition;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, Camera.main, out localPosition);
    localPosition.x /= canvasRectTransform.rect.width;
    localPosition.y /= canvasRectTransform.rect.height;
    punchImage.rectTransform.localPosition = localPosition;
  }
}
