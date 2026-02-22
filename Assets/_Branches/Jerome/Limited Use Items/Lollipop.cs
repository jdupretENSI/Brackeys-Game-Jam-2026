using UnityEngine;

public class Lollipop : MonoBehaviour
{
    [Header("Bridge Settings")]
    [SerializeField] private float bridgeLength = 5f; // Distance to check for ground
    [SerializeField] private float bridgeHeight = 1f; // Height of the bridge above ground
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float bridgeDuration = 5f;
    
    private Transform player;
    private GameObject bridgeObject;
    
    // For gizmo visualization
    private Vector2 playerPosition;
    private Vector2 checkEndPoint;
    private RaycastHit2D hit;
    private bool hasValidGround = false;
    
    private void Start()
    {
        // Find the player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        // Try to create the bridge
        TryCreateBridge();
    }
    
    private void TryCreateBridge()
    {
        if (player == null) return;
        
        playerPosition = player.position;
        float direction = Mathf.Sign(player.localScale.x);
        
        // Calculate the end point to check for ground
        checkEndPoint = new Vector2(
            playerPosition.x + (direction * bridgeLength),
            playerPosition.y
        );
        
        // Raycast from player position forward to check for ground
        hit = Physics2D.Raycast(checkEndPoint, Vector2.down, bridgeHeight * 2f, groundLayer);
        
        if (hit.collider != null)
        {
            hasValidGround = true;
            // Valid surface found at the end point
            CreateBridgeAtMidpoint(hit.point);
        }
        else
        {
            hasValidGround = false;
            // No valid surface, destroy the lollipop
            Debug.Log("No valid surface for bridge");
            Destroy(gameObject);
        }
    }
    
    private void CreateBridgeAtMidpoint(Vector2 endGroundPoint)
    {
        if (player == null) return;
        
        // Calculate midpoint between player and end point
        Vector2 midpoint = new Vector2(
            (player.position.x + endGroundPoint.x) / 2f,
            (player.position.y + endGroundPoint.y) / 2f + bridgeHeight
        );
        
        // Create a simple bridge visual (you can replace this with your actual bridge prefab)
        bridgeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bridgeObject.name = "LollipopBridge";
        bridgeObject.transform.position = midpoint;
        
        // Scale the bridge to span the distance
        float distance = Vector2.Distance(player.position, endGroundPoint);
        bridgeObject.transform.localScale = new Vector3(distance, 0.2f, 1f);
        
        // Rotate to align with the ground angle if needed
        bridgeObject.transform.right = (endGroundPoint - (Vector2)player.position).normalized;
        
        // Add a collider so the player can walk on it
        BoxCollider2D boxCollider = bridgeObject.AddComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(1f, 0.2f);
        
        // Optional: Add a sprite renderer if you have a bridge sprite
        SpriteRenderer spriteRenderer = bridgeObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 0.5f, 0.8f); // Lollipop pink color
        }
        
        // Destroy the bridge after duration
        Destroy(bridgeObject, bridgeDuration);
        
        // Destroy the lollipop object
        Destroy(gameObject);
    }
    
    // Draw gizmos to visualize the raycast
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        
        // Draw the horizontal line from player to check point
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerPosition, checkEndPoint);
        
        // Draw the vertical raycast line
        if (hasValidGround)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(checkEndPoint, hit.point);
            
            // Draw a sphere at the ground hit point
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hit.point, 0.2f);
            
            // Draw the midpoint where bridge will be placed
            if (player != null)
            {
                Vector2 midpoint = new Vector2(
                    (player.position.x + hit.point.x) / 2f,
                    (player.position.y + hit.point.y) / 2f + bridgeHeight
                );
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(midpoint, 0.2f);
            }
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(checkEndPoint, checkEndPoint + Vector2.down * (bridgeHeight * 2f));
        }
        
        // Draw the player position
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(playerPosition, 0.2f);
    }
}