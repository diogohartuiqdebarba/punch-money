using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
  [SerializeField] private Rigidbody playerRigidbody;
  [SerializeField] private FixedJoystick joystick;
  [SerializeField] private Animator animator;
  [SerializeField] private float moveSpeed;

  private GameObject enemyStackParent;
  public bool isWalking = false;
  private int level = 1;
  private int money = 0;
  private int stackCapacity = 5;
  public int enemiesStacked = 0;

  private Renderer playerRenderer;
  private Image moneyImage;
  private Text moneyText;
  private Button buyLevelButton;
  private Text buyLevelButtonText;

  private bool isScalingMoneyImage;
  private bool isScalingBuyLevelButton;
  private EnemySale moneyPlace;

  public void AddMoney(int amount)
  {
    money += amount;
    UpdateMoneyUI();
  }

  public void BuyLevel()
  {
    if (money >= 50)
    {
      money -= 50;
      LevelUp();
      UpdateMoneyUI();
      moneyText.enabled = true;
      buyLevelButton.gameObject.SetActive(false);
      isScalingMoneyImage = false;
      isScalingBuyLevelButton = false;
      StackCapacityUp();
    }
  }

  public bool CanStack()
  {
    if (enemiesStacked < stackCapacity)
    {
      return true;
    }
    if (!isScalingMoneyImage && !buyLevelButton.gameObject.activeSelf)
    {
      moneyPlace.BlinkMoneySalePlace();
      StartCoroutine(ScaleMoneyImage());
    }
    else if (!isScalingBuyLevelButton && buyLevelButton.gameObject.activeSelf)
    {
      StartCoroutine(ScaleAndShineButton(buyLevelButton));
    }
    return false;
  }

  private void Start()
  {
    playerRenderer = transform.GetChild(0).GetComponent<Renderer>();
    moneyImage = GameObject.Find("MoneyImage").GetComponent<Image>();
    moneyPlace = GameObject.Find("MoneyPlace").GetComponent<EnemySale>();
    moneyText = GameObject.Find("MoneyText").GetComponent<Text>();
    buyLevelButton = GameObject.Find("BuyLevelButton").GetComponent<Button>();
    buyLevelButtonText = buyLevelButton.transform.GetChild(0).GetComponent<Text>();
    UpdateLevelButtonTextUI();
    buyLevelButton.gameObject.SetActive(false);
    UpdateMoneyUI();
    isScalingMoneyImage = false;
    enemyStackParent = GameObject.FindWithTag("EnemyStackParent");
  }

  private void Update()
  {
    if (money >= 50)
    {
      if (!isScalingMoneyImage && !buyLevelButton.gameObject.activeSelf)
      {
        moneyPlace.BlinkMoneySalePlace();
        StartCoroutine(ScaleMoneyImage());
      }
    }
    else
    {
      moneyText.enabled = true;
      buyLevelButton.gameObject.SetActive(false);
    }
  }

  private void FixedUpdate()
  {
    playerRigidbody.velocity = new Vector3(joystick.Horizontal * moveSpeed, playerRigidbody.velocity.y, joystick.Vertical * moveSpeed);
    if (joystick.Horizontal != 0 || joystick.Vertical != 0)
    {
      transform.rotation = Quaternion.LookRotation(playerRigidbody.velocity);
      isWalking = true;
      animator.SetBool("isWalking", isWalking);
    }
    else
    {
      isWalking = false;
      animator.SetBool("isWalking", isWalking);
    }
  }

  private void LevelUp()
  {
    level++;
    float t = (float)level / 10f;
    Color color = Color.Lerp(Color.blue, Color.red, t);
    Transform child = transform.GetChild(0);
    Renderer renderer = child.GetComponent<Renderer>();
    if (renderer != null)
    {
      renderer.material.color = color;
    }
  }

  private void UpdateMoneyUI()
  {
    moneyText.text = money.ToString();
  }

  private void UpdateLevelButtonTextUI()
  {
    buyLevelButtonText.text = "Buy Level Up " + level.ToString();
  }

  private IEnumerator ScaleMoneyImage()
  {
    isScalingMoneyImage = true;
    float scaleFactor = 1.5f;
    float duration = 0.5f;
    Vector3 originalScale = moneyImage.transform.localScale;
    Vector3 targetScale = originalScale * scaleFactor;

    // Scale up
    float t = 0f;
    while (t < duration)
    {
      t += Time.deltaTime;
      float normalizedTime = t / duration;
      moneyImage.transform.localScale = Vector3.Lerp(originalScale, targetScale, normalizedTime);
      yield return null;
    }

    // Wait
    yield return new WaitForSeconds(0.5f);

    // Scale down
    t = 0f;
    while (t < duration)
    {
      t += Time.deltaTime;
      float normalizedTime = t / duration;
      moneyImage.transform.localScale = Vector3.Lerp(targetScale, originalScale, normalizedTime);
      yield return null;
    }

    UpdateLevelButtonTextUI();
    moneyText.enabled = false;
    buyLevelButton.gameObject.SetActive(true);
    yield return new WaitForSeconds(0.65f);
    isScalingMoneyImage = false;
  }

  private void StackCapacityUp()
  {
    stackCapacity++;
  }

  IEnumerator ScaleAndShineButton(Button button)
  {
    isScalingBuyLevelButton = true;
    Vector3 originalScale = new Vector3(1, 1, 1);
    button.transform.localScale = originalScale;
    float scaleDuration = 0.5f;
    float scaleElapsedTime = 0f;
    float maxScale = 1.2f;
    while (scaleElapsedTime < scaleDuration)
    {
      scaleElapsedTime += Time.deltaTime;
      float t = Mathf.Clamp01(scaleElapsedTime / scaleDuration);
      float scale = Mathf.Lerp(0.5f, maxScale, t);
      button.transform.localScale = originalScale * scale;
      yield return null;
    }
    button.transform.localScale = originalScale;
    isScalingBuyLevelButton = false;
  }

}
