using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{
    // Health System
    public int currentHearts;
    private int maxHearts = 3;

    // Invincibility after taking hits
    private float timer;
    private float invincibilityDuration = 3f;
    private bool isInvincible;

    // Respawning
    private Vector3 respawnPoint;
    private bool isFlashing;

    private bool isDead;

    // Player components
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Renderer render;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the default values
        currentHearts = maxHearts;
        isInvincible = false;
        isDead = false;

        respawnPoint = new Vector3(0f, 1.5f, 0f);
        transform.position = respawnPoint;

        if (rb == null)
        {
            rb = this.gameObject.GetComponent<Rigidbody>();
        }
        if (render == null)
        {
            render = this.gameObject.GetComponent<Renderer>();
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
            if (isFlashing == false)
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
                TakeDamage();
                Debug.Log($"Hearts left: {currentHearts}");
            }
        }
    }

    // Handles taking damage
    public void TakeDamage()
    {
        // If the player has more than 0 hearts and is not dead
        if (currentHearts > 0 && !isDead)
        {
            // Take away 1 heart
            currentHearts -= 1;

            // Enable invincibilty and respawn the player
            EnableInvincibility();
            Respawn();
        }
        // If the player has no hearts, the player is dead
        if (currentHearts <= 0)
        {
            Dead();
            return;
        }
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
        if (isDead == false)
        {
            // Set the player to dead, destroy the player and return to the main menu
            isDead = true;
            Destroy(gameObject);
            SceneManager.LoadScene("MainMenu");
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
}
