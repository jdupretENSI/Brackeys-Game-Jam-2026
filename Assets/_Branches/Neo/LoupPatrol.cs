using UnityEngine;

public class LoupPatrol : MonoBehaviour
{
    [Header("Patrouille")]
    public Transform pointA;  // Gauche
    public Transform pointB;  // Droite
    public float speed = 2f;

    private bool goingRight = true;
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Va vers point cible
        Vector3 target = goingRight ? pointB.position : pointA.position;

        // Mouvement fluide
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        // Flip sprite direction
        sprite.flipX = !goingRight;

        // Arrivé ? Inverse !
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            goingRight = !goingRight;
        }
    }
}
