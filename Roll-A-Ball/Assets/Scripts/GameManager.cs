using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Player variables")]
    [SerializeField] private GameObject player;
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
    public TextMeshProUGUI survivalTimerText;
    private float survivalTime;
    public GameObject livesUI;

    public GameObject WinScreen;
    private float winScreenTimer;
    private bool hasWon;

    public GameObject LoseScreen;
    private float loseScreenTimer;
    private bool hasLost;

    public GameObject settingsScreen;

    public int currentCoins;
    public TextMeshProUGUI coinsText;
    public bool coinsAdded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameStarted = false;
        WinScreen.SetActive(false);
        winScreenTimer = 0;
        LoseScreen.SetActive(false);
        loseScreenTimer = 0;

        currentCoins = 0;
        coinsText.text = "Coins: 0";
        coinsAdded = false;

        currentScene = SceneManager.GetActiveScene();

        pauseMenu.SetActive(false);
        playIcon.SetActive(false);
        pauseIcon.SetActive(true);
        countdownText.text = "";

        survivalTime = 30f;
        survivalTimerText.text = "";

        playerRb = player.GetComponent<Rigidbody>();
        playerRb.useGravity = false;

        gyro = ground.GetComponent<GyroscopeInput>();
        Debug.Log($"Gyro component found: {gyro}");

        StartCoroutine(Countdown());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            currentCoins = player.GetComponent<PlayerState>().GetCoins();
            coinsText.text = $"Coins: {currentCoins}";

            survivalTime -= Time.deltaTime;
            survivalTimerText.text = $"SURVIVE: {survivalTime.ToString("0.00")}";

            if (survivalTime <= 0)
            {
                survivalTime = 0;
                survivalTimerText.text = "SURVIVED!";
                
                // Game won - show victory screen
                if (WinScreen != null)
                {
                    if (!hasWon)
                    {
                        Won();
                        hasWon = true;
                        if (coinsAdded == false)
                        {
                            PlayerPrefsManager.CoinsToAdd(currentCoins);
                            Debug.Log("Coins added...");

                            PlayerPrefsManager.UpdateCoins();
                            Debug.Log("PlayerPref coins updated...");
                        }
                    }

                    winScreenTimer += Time.deltaTime;

                    if (winScreenTimer >= 5f)
                    {
                        winScreenTimer = 0f;
                        BackToMenu();
                    }
                }
            }
            if (survivalTime > 0 && player.GetComponent<PlayerState>().currentHearts <= 0)
            {
                if (!hasLost)
                {
                    Lost();
                    hasLost = true;
                }
                else
                {
                    loseScreenTimer += Time.deltaTime;

                    if (loseScreenTimer >= 5f)
                    {
                        player.GetComponent<PlayerState>().Dead();
                    }
                }
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
            gamePaused = false;
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
        if (settingsScreen.activeSelf == true)
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
        Debug.Log("Countdown started");

        countdownText.text = "Please hold device comfortably\nGyro initialising...";

        while (!gyro.gyroReady)
        {
            yield return null;
        }

        Debug.Log("gyroReady detected by GameManager");

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
        WinScreen.SetActive(true);
        playerRb.useGravity = false;
        player.gameObject.GetComponent<PlayerState>().Respawn();
    }
    public void Lost()
    {
        LoseScreen.SetActive(true);
    }
}
