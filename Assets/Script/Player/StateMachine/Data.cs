using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player Data/Base Data")]
public class Data : ScriptableObject
{
    [Header("Move State")]
    public float maxSpeed = 12f;
    public float pushSpeed=6f;
    public float acceleration = 30f;
    public float deceleration = 180f;
    public float _groundLinearDrag = 7f;

    [Header("Jump State")]
    public float jumpForce = 15f;

    [Header("Air State")]
    public float airLinearDrag = 2.5f;
    public float fallMultiplier = 5f;
    public float jumpMultiplier = 3f;
    public float coyoteTime = 0.2f;

    [Header("Collider Check")]
    public float groundRadius;
    public Vector3 _groundRaycastOffset;
    public float boxDistance;
    public LayerMask groundMask;
    public LayerMask boxMask;
    public LayerMask trapMask;
    public LayerMask reverseMask;
    public LayerMask keyMask;

}
