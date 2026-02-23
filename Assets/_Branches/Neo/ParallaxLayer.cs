using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f;

    private Transform cam;
    private Vector3 previousCamPos;
    private float spriteWidth;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.bounds.size.x;
    }

    void Update()
    {
        // Calcul du décalage parallax
        float parallaxX = (previousCamPos.x - cam.position.x) * parallaxFactor;
        transform.Translate(new Vector3(parallaxX, 0, 0));

        // 🔥 SEUL CHANGEMENT : viewZone plus LARGE (fix superposition)
        float camX = cam.position.x;
        float viewZone = spriteWidth * 1.8f;  // ← ÉTALÉ de 1.5 à 2.5

        if (transform.position.x > camX + viewZone)
        {
            transform.position = new Vector3(
                transform.position.x - (spriteWidth * 3),  // ← 2→3
                transform.position.y,
                transform.position.z
            );
        }
        else if (transform.position.x < camX - viewZone)
        {
            transform.position = new Vector3(
                transform.position.x + (spriteWidth * 3),  // ← 2→3
                transform.position.y,
                transform.position.z
            );
        }

        previousCamPos = cam.position;
    }
}