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

  private void FixedUpdate()
  {
    playerRigidbody.velocity = new Vector3(joystick.Horizontal * moveSpeed, playerRigidbody.velocity.y, joystick.Vertical * moveSpeed);
    if (joystick.Horizontal != 0 || joystick.Vertical != 0)
    {
      transform.rotation = Quaternion.LookRotation(playerRigidbody.velocity);
      animator.SetBool("isWalking", true);
    }
    else
    {
      animator.SetBool("isWalking", false);
    }
  }
}
