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
  public bool isWalking = false;
  private int level = 1;
  private int money = 0;

  private Renderer playerRenderer;
  private Image moneyImage;
  private Text moneyText;
  private Button buyLevelButton;

  private bool isScaling;

  private void Start()
  {
    playerRenderer = transform.GetChild(0).GetComponent<Renderer>();
    moneyImage = GameObject.Find("MoneyImage").GetComponent<Image>();
    moneyText = GameObject.Find("MoneyText").GetComponent<Text>();
    buyLevelButton = GameObject.Find("BuyLevelButton").GetComponent<Button>();
    buyLevelButton.gameObject.SetActive(false);
    UpdateMoneyUI();
    isScaling = false;
  }

  private void Update()
  {
    if (money >= 50)
    {
      if (!isScaling) StartCoroutine(ScaleMoneyImage());
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
      isScaling = false;
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

  private IEnumerator ScaleMoneyImage()
  {
    isScaling = true;
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

    // Finalize
    moneyText.enabled = false;
    buyLevelButton.gameObject.SetActive(true);
    yield return new WaitForSeconds(0.65f);
  }

}
