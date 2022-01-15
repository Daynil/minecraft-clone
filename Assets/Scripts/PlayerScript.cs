using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

  public float playerSpeed;
  public float mouseSensitivity;

  public CharacterController controller;
  public Camera cam;

  private Vector2 moveVec = Vector2.zero;
  private Vector2 lookVec = Vector2.zero;

  private float xRotation = 0f;

  // Start is called before the first frame update
  void Start()
  {
    // this.rb = this.GetComponent<Rigidbody>();
    this.controller = this.GetComponent<CharacterController>();
    this.cam = this.GetComponentInChildren<Camera>();
  }

  // Update is called once per frame
  void Update()
  {
    Vector3 move = this.transform.right * moveVec.x + this.transform.forward * moveVec.y;
    this.controller.Move(move * this.playerSpeed * Time.deltaTime);

    float mouseX = lookVec.x * this.mouseSensitivity * Time.deltaTime;
    float mouseY = lookVec.y * this.mouseSensitivity * Time.deltaTime;

    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90, 90);

    this.cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    this.transform.Rotate(Vector3.up * mouseX);
  }

  void OnMove(InputValue value)
  {
    this.moveVec = value.Get<Vector2>();
    // this.rb.AddForce(value.Get<Vector2>() * 100);
  }

  void OnLook(InputValue value)
  {
    this.lookVec = value.Get<Vector2>();
  }

  void OnJump(InputValue value)
  {
    // this.rb.AddForce(new Vector3(0, 100, 0));
  }

  void OnFire(InputValue value)
  {
    // this.rb.AddForce(new Vector3(0, 0, 1000));
  }
}

