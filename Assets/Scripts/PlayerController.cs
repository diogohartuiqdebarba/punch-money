using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
  [SerializeField] private Rigidbody playerRigidbody;
  [SerializeField] private FixedJoystick joystick;
  [SerializeField] private Animator animator;

  [SerializeField] private float moveSpeed;
  public bool isWalking = false;

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
}
