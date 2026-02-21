using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField] private float _throwSpeed = 10f;
    [SerializeField] private float _groundCheckDistance = 1f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _teleportCooldown = 0.5f; // Prevent instant re-teleport
    
    private Transform player;
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;
    
    private bool isPlaced = false;
    private bool canTeleport = true;
    private float lastTeleportTime;
    
    private static Portal activePortal = null; // Only one portal can exist at a time

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // If there's already an active portal, destroy it
        if (activePortal != null && activePortal != this)
        {
            Destroy(activePortal.gameObject);
        }
        activePortal = this;
    }

    private void Start()
    {
        // Find the player (assuming the player has the "Player" tag)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        // Start in throwing mode
        rb.bodyType = RigidbodyType2D.Dynamic;
        ThrowPortal();
    }

    private void Update()
    {
        if (isPlaced) return;
        
        // Check for mouse click to place the portal
        if (Input.GetMouseButtonDown(0))
        {
            TryPlacePortal();
        }
    }

    private void ThrowPortal()
    {
        // Get the direction the player is facing
        float direction = player != null ? Mathf.Sign(player.localScale.x) : 1f;
        rb.linearVelocity = new Vector2(direction * _throwSpeed, 0);
    }

    private void TryPlacePortal()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.down, _groundCheckDistance, _groundLayer);
        
        if (hit.collider != null)
        {
            // Valid surface found, place the portal
            PlacePortal(hit.point);
        }
    }

    private void PlacePortal(Vector2 position)
    {
        // Stop movement
        rb.bodyType = RigidbodyType2D.Static;
        transform.position = position;
        isPlaced = true;
        
        // Enable the collider for teleportation
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPlaced) return;
        
        if (other.CompareTag("Player") && canTeleport)
        {
            TeleportPlayer(other.transform);
        }
    }

    private void TeleportPlayer(Transform playerTransform)
    {
        // Set cooldown
        canTeleport = false;
        lastTeleportTime = Time.time;
        
        // Store the portal's position before destruction
        Vector3 portalPosition = transform.position;
        
        // Teleport the player to the portal's position
        // You might want to add a slight offset so the player doesn't get stuck in the portal
        playerTransform.position = portalPosition + new Vector3(0, 0.5f, 0);
        
        // Add some visual feedback (you can add particle effects or sounds here)
        Debug.Log("Player teleported!");
        
        // Destroy the portal after use
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Clear the static reference if this was the active portal
        if (activePortal == this)
        {
            activePortal = null;
        }
    }

    public static void UsePortal()
    {
        // This is called from the Inventory system
        // The actual portal creation will be handled by the Inventory or Player script
        Debug.Log("Portal use requested - spawning portal in front of player");
    }
}