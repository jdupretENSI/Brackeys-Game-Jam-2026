using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _moveSpeed, _jumpHeight;
    [SerializeField] private InputActionReference _move;
    
    private Vector2 _moveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMove(InputValue value)
    {
        _rb.linearVelocity = value.Get<Vector2>() *  _moveSpeed;
    }

    void OnJump()
    {
        _rb.AddForce(Vector2.up * _jumpHeight);
    }
}
