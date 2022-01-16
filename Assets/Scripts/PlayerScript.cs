using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

  public float playerSpeed;

  public float mouseSensitivity;
  public float rightGamepadSensitivity;

  public CharacterController controller;
  public Camera cam;
  public PlayerInput input;

  private Vector3 moveVec = Vector2.zero;
  private Vector2 lookVec = Vector2.zero;

  private float xRotation = 0f;

  void Awake()
  {
    this.controller = this.GetComponent<CharacterController>();
    this.cam = this.GetComponentInChildren<Camera>();
    this.input = this.GetComponent<PlayerInput>();
  }

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    /*
      We are performing vector addition on the normalized "left/right" vector of the player (x)
      and the normalized forward/backward vector of the player (z)
      ➡ + ⬆ = ↗ 
      This determines the *direction* we want to move *relative to the rotation of the player*
      Then, we multiply a fixed movement speed to determine the vector's magnitude
    */
    Vector3 move = this.transform.right * moveVec.x + this.transform.forward * moveVec.z;
    this.controller.Move(move * this.playerSpeed * Time.deltaTime);


    float mouseX = lookVec.x * Time.deltaTime;
    float mouseY = lookVec.y * Time.deltaTime;

    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90, 90);

    this.cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    this.transform.Rotate(Vector3.up * mouseX);
  }

  void OnMove(InputValue value)
  {
    Vector2 inputVec = value.Get<Vector2>();
    // Map input 2d input vec to world 3d vec
    // This essentially maps the up/down (y) inputs to forward/back (z) in world space
    this.moveVec = new Vector3(inputVec.x, 0, inputVec.y);
  }

  void OnLook(InputValue value)
  {
    // up down (y) on the mouse properly correspond to world up down (y), no need to map
    this.lookVec = value.Get<Vector2>();

    float lookSensitivity = this.input.currentControlScheme == "Gamepad" ? this.rightGamepadSensitivity : this.mouseSensitivity;
    this.lookVec = this.lookVec * lookSensitivity;
  }

  void OnJump(InputValue value)
  {
    // this.rb.AddForce(new Vector3(0, 100, 0));
  }

  void OnFire(InputValue value)
  {
    // this.rb.AddForce(new Vector3(0, 0, 1000));
  }

  void OnControlsChanged()
  {
    // if (this.input is null)
    // {
    //   this.lookSensitivity = this.mouseSensitivity; // Null on initial check
    //   return;
    // }
    // switch (this.input.currentControlScheme)
    // {
    //   case "Keyboard&Mouse":
    //     this.lookSensitivity = this.mouseSensitivity;
    //     break;
    //   case "Gamepad":
    //     this.lookSensitivity = this.rightGamepadSensitivity;
    //     break;
    //   default:
    //     Debug.LogError($"Unhandled control scheme {this.input.currentControlScheme}");
    //     this.lookSensitivity = this.mouseSensitivity;
    //     break;
    // }
  }
}

