using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 moveInput;
    public int inputX;
    public int inputY;
    [Header("Jump")]
    public bool jumpInput;
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
    private void Update()
    {
        MoveInput();
        JumpInput();
        
    }
}
