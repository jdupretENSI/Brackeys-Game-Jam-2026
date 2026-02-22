using UnityEngine;

public class Lollipop : MonoBehaviour
{
    [Header("Bridge Settings")]
    [SerializeField] private float _bridgeLength = 5f; // Distance to check for ground
    [SerializeField] private float _bridgeHeight = 1f; // Height of the bridge above ground
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _bridgeDuration = 5f;
    [SerializeField] private float _lollipopLifetime = 1.5f;
    
    private Transform player;
    private GameObject bridgeObject;
    private Inventory playerInventory;
    
    // For gizmo visualization
    private Vector2 playerPosition;
    private Vector2 checkEndPoint;
    private RaycastHit2D hit;
    private bool hasValidGround = false;
    private bool bridgeCreated = false;
    
    private void Start()
    {
        // Find the player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerInventory = playerObj.GetComponent<Inventory>();
        }
        
        // Store the player position at creation time
        if (player != null)
        {
            playerPosition = player.position;
        }
    }
    
    private void Update()
    {
        // Only try to create the bridge once
        if (!bridgeCreated && player != null)
        {
            TryCreateBridge();
        }
    }
    
    private void TryCreateBridge()
    {
        float direction = Mathf.Sign(player.localScale.x);
        
        // Calculate the end point to check for ground
        checkEndPoint = new Vector2(
            playerPosition.x + (direction * _bridgeLength),
            playerPosition.y
        );

        // Raycast from the end point downward to check for ground
        hit = Physics2D.Raycast(checkEndPoint, Vector2.down, _bridgeHeight * 2f, _groundLayer);
        
        if (hit.collider != null)
        {
            hasValidGround = true;
            // Valid surface found at the end point
            CreateBridgeAtMidpoint(hit.point);
            bridgeCreated = true;
            
            // Destroy the lollipop object after successful bridge creation
        }
        else
        {
            hasValidGround = false;
            // No valid surface, destroy the lollipop WITHOUT consuming an inventory use
            Debug.Log("No valid surface for bridge - lollipop not consumed");
            
            // Return the lollipop use to the inventory
            if (playerInventory != null)
            {
                playerInventory.AddLollipop();
            }
            
            // Destroy the lollipop object
        }

        Destroy(gameObject, _lollipopLifetime);
    }
    
    private void CreateBridgeAtMidpoint(Vector2 endGroundPoint)
    {
        if (player == null) return;
        
        // Calculate midpoint between player and end point
        Vector2 midpoint = new Vector2(
            (player.position.x + endGroundPoint.x) / 2f,
            (player.position.y + endGroundPoint.y) / 2f + _bridgeHeight
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
        
        // Destroy the bridge after duration
        Destroy(bridgeObject, _bridgeDuration);
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
                    (player.position.y + hit.point.y) / 2f + _bridgeHeight
                );
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(midpoint, 0.2f);
            }
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(checkEndPoint, checkEndPoint + Vector2.down * (_bridgeHeight * 2f));
        }
        
        // Draw the player position
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(playerPosition, 0.2f);
    }
}