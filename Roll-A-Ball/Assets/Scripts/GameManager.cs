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

    [Header("Start variables")]
    public TextMeshProUGUI countdownText;

    Scene currentScene;

    public bool gameStarted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameStarted = false;

        currentScene = SceneManager.GetActiveScene();

        pauseMenu.SetActive(false);
        countdownText.text = "";

        playerRb = player.GetComponent<Rigidbody>();
        playerRb.useGravity = false;

        gyro = ground.GetComponent<GyroscopeInput>();
        Debug.Log($"Gyro component found: {gyro}");

        StartCoroutine(Countdown());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TogglePause()
    {
        gamePaused = !gamePaused;

        if (gamePaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else
        {
            gamePaused = false;
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
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
}
