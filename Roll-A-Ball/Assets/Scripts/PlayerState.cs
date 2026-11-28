using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR;

public class PlayerState : MonoBehaviour
{
    [Header("Health System")]
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject UIHeartsArea;
    [SerializeField] public List<GameObject> hearts;
    [SerializeField] private List<Animator> heartAnimators;
    [SerializeField] private int currentHearts;
    [SerializeField] private bool hasExtraLife;
    private bool isDead;

    [Header("Invincibility")]
    private float timer;
    private float invincibilityDuration = 3f;
    private bool isInvincible;

    [Header("Respawning")]
    private Vector3 respawnPoint;
    private bool isFlashing;

    [Header("Player Info")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Renderer render;
    public int coinsCollected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the default values
        currentHearts = 3;

        // Create the standard 3 hearts
        for (int i = 0; i < currentHearts; i++)
        {
            CreateHeart();
        }

        if (PlayerPrefs.GetInt(PlayerPrefsManager.ExtraLives, 0) > 0 &&
            PlayerPrefs.GetInt(PlayerPrefsManager.ExtraLifeEquippedKey, 0) == 1)
        {
            ActivateExtraLife();
        }
        else
        {
            PlayerPrefsManager.SetExtraLifeEquipped(false);
        }

        isInvincible = false;
        isDead = false;

        // Set the respawn point and place the player at the spawn point
        respawnPoint = new Vector3(0f, 1.5f, 0f);
        transform.position = respawnPoint;

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (render == null)
        {
            render = GetComponent<Renderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the player runs out of hearts and is not yet dead, make the player dead
        if (currentHearts <= 0 && !isDead)
        {
            Dead();
            return;
        }

        // While the player is invincible...
        if (isInvincible)
        {
            // If the player is not flashing with invincibility
            if (!isFlashing)
            {
                // Flash player visually
                StartCoroutine(Flashing());
            }

            // Count upwards on the timer
            timer += Time.deltaTime;

            // If the timer reaches the invincibility cooldown duration
            if (timer >= invincibilityDuration)
            {
                // The player is no longer invincible
                isInvincible = false;
            }
        }
        else
        {
            // Ensure the player is visible
            render.enabled = true;
            isFlashing = false;

            // If the player falls off the board and falls further than y = -5, initiate TakeDamage()
            if (transform.position.y <= -5.0f)
            {
                TakeDamage();
            }
        }
    }

    // When the player collides with an object
    public void OnTriggerEnter(Collider other)
    {
        // If the player is not dead
        if (!isDead)
        {
            // If the player is invincible, ignore the collision - early exit
            if (isInvincible) return;

            // If the player collides with a laser, deal damage
            if (other.gameObject.CompareTag("Laser"))
            {
                Handheld.Vibrate();
                TakeDamage();
                Debug.Log($"Hearts left: {currentHearts}");
            }
            else if (other.gameObject.CompareTag("ExtraLife"))
            {
                ActivateExtraLife();
                Destroy(other.gameObject);
            }
        }
    }

    // Handles taking damage
    public void TakeDamage()
    {
        // If the player has less than 0 hearts or is dead, early exit
        if (currentHearts <= 0 || isDead) return;

        // Otherwise play the damage anim
        heartAnimators[currentHearts - 1].SetTrigger("DamageTrigger");

        // Take away 1 heart
        currentHearts--;

        // Enable invincibilty and respawn the player
        EnableInvincibility();
        Respawn();
    }

    private void CreateHeart(bool extra = false)
    {
        // Get references for both the current heart and the animator attached
        GameObject heart = Instantiate(heartPrefab, UIHeartsArea.transform);
        Animator anim = heart.GetComponent<Animator>();

        // If the created heart is the extra life
        if (extra)
        {
            // Spawn the extra heart at index 0 to maintain visual order
            heart.transform.SetSiblingIndex(0);
            hearts.Insert(0, heart);
            heartAnimators.Insert(0, anim);
        }
        else
        {
            // Otherwise create hearts as normal
            hearts.Add(heart);
            heartAnimators.Add(anim);
        }
    }

    // Used as read-only access for external scripts
    public int GetCurrentHearts()
    {
        return currentHearts;
    }

    // Activate the extra life
    public void ActivateExtraLife()
    {
        // If player already has extra life, early exit and reward with 20 coins instead
        if (hasExtraLife)
        {
            AddCoins(20);
            return;
        }

        // Player has extra life
        hasExtraLife = true;
        // Increase number of hearts
        currentHearts++;

        // Create a heart with the condition that it is the extra life
        CreateHeart(true);
    }

    // Handles respawning the player
    public void Respawn()
    {
        // Make the player's current position the respawn position
        transform.position = respawnPoint;
        
        // If the rb is found, set the velocity to zero so it doesn't get flung with the same velocity
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    // Enables the player's invincibility
    public void EnableInvincibility()
    {
        // Resets the timer and enables invincibility
        timer = 0;
        isInvincible = true;
    }

    // Handles player death
    public void Dead()
    {
        // If the player isn't dead yet
        if (!isDead)
        {
            // Set the player to dead, destroy the player
            isDead = true;
            Destroy(gameObject);
        }
    }

    // Flashes the player between intervals
    IEnumerator Flashing()
    {
        isFlashing = true;

        while (isInvincible)
        {
            render.enabled = !render.enabled;
            yield return new WaitForSeconds(0.15f);
        }

        render.enabled = true;
        isFlashing = false;
    }

    public int GetCoins()
    {
        return coinsCollected;
    }
    public void AddCoins(int coinValue)
    {
        coinsCollected += coinValue;
    }
}
