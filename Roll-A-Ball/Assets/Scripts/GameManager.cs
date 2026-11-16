using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum Gamemode
{
    Survival, Endless
}

public class GameManager : MonoBehaviour
{
    [Header("Player variables")]
    [SerializeField] private GameObject player;
    private PlayerState playerState;
    private Rigidbody playerRb;

    [SerializeField] private GameObject ground;
    private GyroscopeInput gyro;

    [Header("Menu variables")]
    [SerializeField] private GameObject pauseMenu;
    public bool gamePaused;
    Scene currentScene;
    public GameObject pauseIcon;
    public GameObject playIcon;

    [Header("Start variables")]
    public TextMeshProUGUI countdownText;
    public bool gameStarted;

    [Header("Gameplay variables")]
    public static Gamemode selectedGamemode;

    public TextMeshProUGUI survivalTimerText;
    private float survivalTime;
    public GameObject livesUI;

    public GameObject WinScreen;
    private bool hasWon;

    public GameObject LoseScreen;
    private bool hasLost;

    private bool gameFinished;
    private float returnTimer;

    public GameObject settingsScreen;

    public int currentCoins;
    public TextMeshProUGUI coinsText;
    private bool coinsAdded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        survivalTimerText.text = "";

        switch (selectedGamemode)
        {
            case Gamemode.Survival:
                survivalTime = 30f;
                break;
            case Gamemode.Endless:
                survivalTime = 0f;
                break;
        }

        gameStarted = false;
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);

        playerState = player.GetComponent<PlayerState>();

        currentCoins = playerState.GetCoins();
        coinsText.text = "Coins: 0";
        coinsAdded = false;

        currentScene = SceneManager.GetActiveScene();

        pauseMenu.SetActive(false);
        playIcon.SetActive(false);
        pauseIcon.SetActive(true);
        countdownText.text = "";

        playerRb = player.GetComponent<Rigidbody>();
        playerRb.useGravity = false;

        gameFinished = false;
        returnTimer = 0;

        gyro = ground.GetComponent<GyroscopeInput>();
        Debug.Log($"Gyro component found: {gyro}");

        StartCoroutine(Countdown());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted && !gameFinished)
        {
            currentCoins = playerState.GetCoins();
            coinsText.text = $"Coins: {currentCoins}";

            switch (selectedGamemode)
            {
                case Gamemode.Survival:
                    if (survivalTime <= 0)
                    {
                        survivalTime = 0;
                        survivalTimerText.text = "SURVIVED!";

                        // Game won - show victory screen
                        if (!hasWon)
                        {
                            Won();
                        }
                    }
                    else if (survivalTime > 0 && playerState.currentHearts <= 0 && !hasLost)
                    {
                        // Game lost - show lost screen
                        Lost();
                    }
                    else
                    {
                        survivalTime -= Time.deltaTime;
                        survivalTime = Mathf.Max(0, survivalTime);  // Clamps survival time to 0
                        survivalTimerText.text = $"SURVIVE: {survivalTime.ToString("0.00")}";
                    }
                        break;

                // Use for when Endless gamemode is selected
                case Gamemode.Endless:
                    if (playerState.currentHearts <= 0 && !hasLost)
                    {
                        Lost();
                    }
                    else
                    {
                        // Time counts upwards, try to survive as long as possible
                        survivalTime += Time.deltaTime;
                        survivalTimerText.text = $"CURRENT TIME: {survivalTime.ToString("0.00")}";
                    }
                        break;
            }
        }

        if (gameFinished)
        {
            returnTimer += Time.deltaTime;

            if (returnTimer >= 5f)
            {
                SceneManager.LoadScene("MainMenu");
                returnTimer = 0f;
            }
        }
    }

    public void TogglePause()
    {
        gamePaused = !gamePaused;

        if (gamePaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            survivalTimerText.gameObject.SetActive(false);
            livesUI.SetActive(false);

            pauseIcon.SetActive(false);
            playIcon.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            survivalTimerText.gameObject.SetActive(true);
            livesUI.SetActive(true);

            pauseIcon.SetActive(true);
            playIcon.SetActive(false);
        }
    }

    public void Settings()
    {
        settingsScreen.SetActive(true);
    }
    public void Back()
    {
        if (settingsScreen.activeSelf)
        {
            settingsScreen.SetActive(false);
        }
    }

    public void Restart()
    {
        gamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentScene.name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    IEnumerator Countdown()
    {
        //Debug.Log("Countdown started");

        countdownText.text = "Please hold device comfortably\nGyro initialising...";

        while (!gyro.gyroReady)
        {
            yield return null;
        }

        //Debug.Log("gyroReady detected by GameManager");

        countdownText.text = "3";
        yield return new WaitForSeconds(1f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        countdownText.text = "";
        playerRb.useGravity = true;
        gameStarted = true;
    }

    public void Won()
    {
        // Prevents calling the function twice
        if (gameFinished) return;

        gameFinished = true;
        hasWon = true;

        WinScreen.SetActive(true);
        playerRb.useGravity = false;
        playerState.Respawn();
        SaveCoins();
    }
    public void Lost()
    {
        // Prevents calling the function twice
        if (gameFinished) return;

        gameFinished = true;
        hasLost = true;

        LoseScreen.SetActive(true);
        SaveCoins();
    }

    private void SaveCoins()
    {
        if (coinsAdded) return;

        // Send the number of coins collected this game to the PlayerPrefsManager
        PlayerPrefsManager.CoinsToAdd(currentCoins);
        //Debug.Log("Coins added...");

        // Call the UpdateCoins() function within PlayerPrefsManager
        PlayerPrefsManager.UpdateCoins();
        //Debug.Log("PlayerPref coins updated...");

        coinsAdded = true;
    }
}
