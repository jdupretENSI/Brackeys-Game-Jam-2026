using UnityEngine;

public class Gloves : MonoBehaviour
{
    [Header("Glove Settings")]
    [SerializeField] private float _gloveDuration = 0.3f;
    [SerializeField] private float _gloveRange = 1.5f;
    [SerializeField] private LayerMask _enemyLayer;
    
    [Header("Normal Knockback")]
    [SerializeField] private float _normalKnockbackForce = 10f;
    [SerializeField] private float _normalUpwardForce = 2f;
    
    [Header("Combo Knockback (4th hit)")]
    [SerializeField] private float _comboKnockbackForce = 25f;
    [SerializeField] private float _comboUpwardForce = 5f;
    [SerializeField] private float _comboStunDuration = 1f;
    
    private Transform player;
    private Animator animator;
    private Collider2D gloveCollider;
    private float timer;
    private bool hasHit = false;
    
    // Static combo counter shared across all glove instances
    private static int _comboCount = 0;
    private static float _comboResetTimer = 0f;
    private const float ComboWindow = 1.5f; // Time window to maintain combo

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gloveCollider = GetComponent<Collider2D>();
        
        // Make sure collider is trigger
        if (gloveCollider)
        {
            gloveCollider.isTrigger = true;
        }
    }

    private void Start()
    {
        // Find the player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj)
        {
            player = playerObj.transform;
        }
        
        // Position the glove in front of the player
        PositionGlove();
        
        // Set the glove to destroy itself after duration
        timer = _gloveDuration;
    }

    private void Update()
    {
        // Update combo reset timer
        if (_comboResetTimer > 0)
        {
            _comboResetTimer -= Time.deltaTime;
            if (_comboResetTimer <= 0)
            {
                _comboCount = 0;
                Debug.Log("Combo reset");
            }
        }
        
        // Destroy after duration
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void PositionGlove()
    {
        if (!player) return;
        
        // Position glove in front of player based on facing direction
        float direction = Mathf.Sign(player.localScale.x);
        transform.position = player.position + new Vector3(direction * _gloveRange, 0.5f, 0);
        
        // Face the glove in the direction of the punch
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return; // Prevent multiple hits from same glove
        
        // Check if the collider belongs to an enemy
        if (!IsEnemy(other.gameObject)) return;
        hasHit = true;
            
        // Update combo
        UpdateCombo();
            
        // Apply knockback to enemy
        ApplyKnockback(other.gameObject);
            
        // Trigger hit animation/effect
        if (animator)
        {
            animator.SetTrigger("Hit");
        }
            
        Debug.Log($"Glove hit! Combo: {_comboCount}");
    }

    private bool IsEnemy(GameObject obj)
    {
        // Check if the object is on the enemy layer
        return ((1 << obj.layer) & _enemyLayer) != 0;
    }

    private void UpdateCombo()
    {
        // Reset combo timer on hit
        _comboResetTimer = ComboWindow;
        
        // Increment combo
        _comboCount++;
        
        // Wrap combo after 4 (or keep at 4 for max power)
        if (_comboCount > 4)
        {
            _comboCount = 1; // Reset to 1 for next cycle
        }
    }

    private void ApplyKnockback(GameObject enemy)
    {
        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
        if (!enemyRb) return;
        
        // Calculate knockback direction (away from player)
        Vector2 knockbackDirection = (enemy.transform.position - player.position).normalized;
        knockbackDirection.y = 0.5f; // Add some upward angle
        
        float knockbackForce;
        float upwardForce;
        float stunDuration = 0f;
        
        // Check if this is the 4th hit (combo)
        if (_comboCount == 4)
        {
            knockbackForce = _comboKnockbackForce;
            upwardForce = _comboUpwardForce;
            stunDuration = _comboStunDuration;
            
            Debug.Log("COMBO! SUPER KNOCKBACK!");
            
            // Optional: Add screen shake or special effect here
        }
        else
        {
            knockbackForce = _normalKnockbackForce;
            upwardForce = _normalUpwardForce;
        }
        
        // Apply the knockback force
        Vector2 knockback = knockbackDirection * knockbackForce;
        knockback.y = upwardForce; // Override Y for consistent upward launch
        
        enemyRb.linearVelocity = Vector2.zero; // Reset current velocity
        enemyRb.AddForce(knockback, ForceMode2D.Impulse);
        
        // Optional: Add hit effect
        SpawnHitEffect(enemy.transform.position, _comboCount == 4);
    }

    private void SpawnHitEffect(Vector3 position, bool isCombo)
    {
        // You can implement particle effects here
        // For now, just log it
        Debug.Log(isCombo ? "COMBO HIT EFFECT!" : "Normal hit effect");
    }

    private void OnDestroy()
    {
        // Clean up if needed
    }

    public static void UseGloves()
    {
        // This is called from the Inventory system
        Debug.Log("Gloves use requested - spawning glove in front of player");
    }
}