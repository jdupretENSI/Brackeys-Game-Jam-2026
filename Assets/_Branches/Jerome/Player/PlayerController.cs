using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask _groundLayer;
    
    [Header("Movement Feel")]
    [SerializeField] private float _acceleration = 50f;
    [SerializeField] private float _deceleration = 50f;
    [SerializeField] private float _airControlMultiplier = 0.7f;
    
    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event _footStep;
    [SerializeField] private AK.Wwise.Event _hurt;
    [SerializeField] private AK.Wwise.Event _jump;
    [SerializeField] private AK.Wwise.Event _land;
    
    private Vector2 _moveInput;
    private bool _isGrounded;

    void OnPlayFootstep()
    {
        _footStep.Post(gameObject);
    }

    void OnPlayHurt()
    {
        _hurt.Post(gameObject);
    }

    void OnPlayJump()
    {
        _jump.Post(gameObject);
    }

    void OnPlayLand()
    {
        _land.Post(gameObject);
    }
    
    // Called automatically by the Input System when using "Send Messages" behavior
    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }
    
    // Called automatically by the Input System when using "Send Messages" behavior
    void OnJump()
    {
        if (!_isGrounded) return;
        
        // Reset vertical velocity for consistent jump height
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0f);
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
    
    void FixedUpdate()
    {
        CheckGrounded();
        ApplyMovement();
    }
    
    private void ApplyMovement()
    {
        // Calculate target horizontal velocity
        float targetVelocityX = _moveInput.x * _moveSpeed;
        
        // Apply air control multiplier if not grounded
        float acceleration = _isGrounded ? _acceleration : _acceleration * _airControlMultiplier;
        float deceleration = _isGrounded ? _deceleration : _deceleration * _airControlMultiplier;
        
        // Smoothly change velocity based on input
        if (_moveInput.x != 0)
        {
            // Accelerate
            _rb.linearVelocity = new Vector2(
                Mathf.MoveTowards(_rb.linearVelocity.x, targetVelocityX, acceleration * Time.fixedDeltaTime),
                _rb.linearVelocity.y
            );
        }
        else
        {
            // Decelerate when no input
            _rb.linearVelocity = new Vector2(
                Mathf.MoveTowards(_rb.linearVelocity.x, 0, deceleration * Time.fixedDeltaTime),
                _rb.linearVelocity.y
            );
        }
        
        // // Optional: Flip sprite based on movement direction
        // if (_moveInput.x != 0)
        // {
        //     Vector3 scale = transform.localScale;
        //     scale.x = Mathf.Sign(_moveInput.x);
        //     transform.localScale = scale;
        // }
    }
    
    private void CheckGrounded()
    {
        // Raycast to check if grounded
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, 
            Vector2.down, 
            _groundCheckDistance, 
            _groundLayer
        );
        
        _isGrounded = hit.collider;
        
        // Debug visualization
        Debug.DrawRay(transform.position, Vector2.down * _groundCheckDistance, 
            _isGrounded ? Color.green : Color.red);
    }
    
    // Optional: Visualize ground check in Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _groundCheckDistance);
    }
}