using UnityEngine;

public class Lollipop : MonoBehaviour
{
    [Header("Bridge Settings")]
    [SerializeField] private GameObject _bridgePrefab;
    [SerializeField] private float _maxBridgeLength = 5f;
    [SerializeField] private float _groundCheckDistance = 1f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _bridgeDuration = 5f;
    [SerializeField] private float _bridgeWidth = 2f;
    
    [Header("Bridge Appearance")]
    [SerializeField] private Color _validPlacementColor = Color.green;
    [SerializeField] private Color _invalidPlacementColor = Color.red;
    
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private bool isValidPlacement = false;
    private Vector3 bridgePosition;
    private float bridgeLength;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Find the player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj)
        {
            player = playerObj.transform;
        }
        
        // Position the lollipop in front of the player
        PositionLollipop();
    }

    private void Update()
    {
        if (!player) return;
        
        // Update lollipop position to follow mouse
        UpdateLollipopPosition();
        
        // Check for placement
        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceBridge();
        }
        
        // Update visual feedback
        UpdateVisualFeedback();
    }

    private void PositionLollipop()
    {
        // Place lollipop slightly in front of player
        float direction = Mathf.Sign(player.localScale.x);
        transform.position = player.position + new Vector3(direction * 1f, 0.5f, 0);
    }

    private void UpdateLollipopPosition()
    {
        // Get mouse position in world space
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Calculate direction from player to mouse
        Vector2 direction = (mousePosition - (Vector2)player.position).normalized;
        
        // Limit the direction to horizontal mostly (can add slight vertical)
        direction.y = Mathf.Clamp(direction.y, -0.3f, 0.3f);
        direction.Normalize();
        
        // Calculate potential bridge end position
        Vector2 potentialEndPosition = (Vector2)player.position + direction * _maxBridgeLength;
        
        // Raycast to find ground at the potential end position
        RaycastHit2D groundHit = Physics2D.Raycast(potentialEndPosition, Vector2.down, _groundCheckDistance, _groundLayer);
        
        if (groundHit.collider)
        {
            // Found ground on the other side
            isValidPlacement = true;
            
            // Calculate bridge position and length
            bridgePosition = (player.position + (Vector3)potentialEndPosition) / 2f;
            bridgePosition.y = groundHit.point.y + 0.1f; // Slightly above ground
            
            bridgeLength = Vector2.Distance(player.position, groundHit.point);
            
            // Position the lollipop at the mouse position for visual feedback
        }
        else
        {
            isValidPlacement = false;
        }

        transform.position = mousePosition;

        // Keep lollipop within reasonable distance
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer > _maxBridgeLength)
        {
            transform.position = player.position + (Vector3)direction * _maxBridgeLength;
        }
    }

    private void TryPlaceBridge()
    {
        if (!isValidPlacement || !_bridgePrefab) return;
        
        CreateBridge();
        Destroy(gameObject); // Destroy the lollipop item
    }

    private void CreateBridge()
    {
        // Instantiate the bridge
        GameObject bridge = Instantiate(_bridgePrefab, bridgePosition, Quaternion.identity);
        
        // Scale the bridge to fit the gap
        Transform bridgeTransform = bridge.transform;
        bridgeTransform.localScale = new Vector3(bridgeLength, _bridgeWidth, 1);
        
        // Add Bridge component to handle duration and cleanup
        Bridge bridgeComponent = bridge.AddComponent<Bridge>();
        bridgeComponent.Initialize(_bridgeDuration);
        
        Debug.Log($"Bridge created! Length: {bridgeLength}, Duration: {_bridgeDuration} seconds");
    }

    private void UpdateVisualFeedback()
    {
        if (spriteRenderer)
        {
            spriteRenderer.color = isValidPlacement ? _validPlacementColor : _invalidPlacementColor;
        }
    }

    public static void UseLollipop()
    {
        // This is called from the Inventory system
        Debug.Log("Lollipop use requested - spawning lollipop in front of player");
    }
}

// Bridge component for automatic destruction
public class Bridge : MonoBehaviour
{
    private float duration;
    private float timer;

    public void Initialize(float bridgeDuration)
    {
        duration = bridgeDuration;
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        // Optional: Add visual feedback that bridge is about to expire
        if (timer > duration * 0.8f)
        {
            // Flash or fade out
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr)
            {
                float alpha = Mathf.PingPong(Time.time * 5f, 1f);
                sr.color = new Color(1f, 1f, 1f, alpha);
            }
        }
        
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}