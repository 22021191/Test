using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 moveInput;
    public float inputX;
    public float inputY;
    [Header("Jump")]
    public bool jumpInput;
    PlayerController controller;
    private void Awake()
    {
        controller = new PlayerController();
        controller.Enable();
        controller.Movement.Move.performed += tmp =>
        {
            inputX=tmp.ReadValue<float>();
        };
        controller.Movement.Direction.performed += tmp =>
        {
            inputY = tmp.ReadValue<float>();
        };
        moveInput = new Vector2(inputX, inputY);
        controller.Movement.Jump.performed += tmp =>
        {
            jumpInput = true;
        };
    }
    public void MoveInput()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputX = (int)(moveInput * Vector2.right).normalized.x;
        inputY = (int)(moveInput * Vector2.up).normalized.y;

    }
    public void JumpInput()
    {
        jumpInput = (Input.GetKeyDown(KeyCode.K) || Input.GetButton("Jump"));

    }
    /*private void Update()
    {
        //MoveInput();
        JumpInput();
        
    }
*/
}
