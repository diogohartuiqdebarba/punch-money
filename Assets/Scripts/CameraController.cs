using UnityEngine;

public class CameraController : MonoBehaviour
{
  [SerializeField] private float speed = 5.0f;
  [SerializeField] private Transform target;
  [SerializeField] private Vector3 offset = Vector3.up;

  [SerializeField] private bool lookAt = false;

  void Update()
  {
    transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
    if (lookAt) transform.LookAt(target.position);
  }
}