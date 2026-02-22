using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Santé")]
    public int maxHealth = 3;

    [Header("Respawn")]
    public Transform[] checkpoints = new Transform[0];

    private int currentHealth;
    private int currentCheckpointIndex = 0;
    private bool isDead = false;
    private SpriteRenderer sprite;

    void Start()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
        Debug.Log($"❤️ {currentHealth}/{maxHealth} vies");
    }

    // ❌ SUPPRIMÉ Update() chute !

    public void TakeDamage(int damage = 1)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"⚡ VIE PERDUE ! {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            isDead = true;
            SceneManager.LoadScene("MainMenu");
            return;
        }

        Respawn();
        StartCoroutine(InvincibilityFlash());
    }

    void Respawn()
    {
        if (checkpoints.Length > currentCheckpointIndex)
        {
            transform.position = checkpoints[currentCheckpointIndex].position;
            Debug.Log($"🔄 RESPAWN {currentCheckpointIndex + 1}");
        }
    }

    public void SetCheckpoint(int index)
    {
        currentCheckpointIndex = index;
        Debug.Log($"✅ CHECKPOINT {index + 1}");
    }

    IEnumerator InvincibilityFlash()
    {
        float timer = 0;
        while (timer < 2f)
        {
            sprite.enabled = !sprite.enabled;
            timer += Time.deltaTime;
            yield return null;
        }
        sprite.enabled = true;
    }
}
