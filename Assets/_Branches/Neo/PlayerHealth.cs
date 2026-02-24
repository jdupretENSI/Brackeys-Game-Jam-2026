using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Santé")]
    public int maxHealth = 3;

    [Header("PARALLAX")]
    public GameObject[] parallaxBackgrounds = new GameObject[3];

    private int currentHealth;
    private bool isDead = false;
    private bool isInvincible = false;
    private SpriteRenderer sprite;

    void Start()
    {
        currentHealth = maxHealth;
        sprite = GetComponentInChildren<SpriteRenderer>();
        SetParallaxBackground(currentHealth);
        Debug.Log("PlayerHealth ✅ PRÊT");
    }

    void Awake()
    {
        Debug.Log("PlayerHealth AWAKE sur " + gameObject.name + " ✅");
    }

    public void TakeDamage(int damage = 1)
    {
        if (isDead || isInvincible) return;

        currentHealth -= damage;
        Debug.Log($"⚡ {currentHealth}/{maxHealth}");
        SetParallaxBackground(currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("💀 GAME OVER");
            SceneManager.LoadScene("MainMenu");
            return;
        }

        // FLASH SIMPLE SANS RESPAWN
        StartCoroutine(InvincibilityFlash());
    }

    void SetParallaxBackground(int lives)
    {
        for (int i = 0; i < parallaxBackgrounds.Length; i++)
        {
            if (parallaxBackgrounds[i] != null)
                parallaxBackgrounds[i].SetActive(false);
        }

        int index = 3 - lives;
        if (index >= 0 && index < parallaxBackgrounds.Length && parallaxBackgrounds[index] != null)
        {
            parallaxBackgrounds[index].SetActive(true);
        }
    }

    IEnumerator InvincibilityFlash()
    {
        isInvincible = true;
        float timer = 0;
        while (timer < 2f)
        {
            if (sprite) sprite.enabled = !sprite.enabled;
            timer += Time.deltaTime;
            yield return null;
        }
        if (sprite) sprite.enabled = true;
        isInvincible = false;
    }
}
