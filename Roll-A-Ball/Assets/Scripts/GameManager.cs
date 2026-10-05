using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player variables")]
    [SerializeField] private GameObject player;

    [Header("Menu variables")]
    [SerializeField] private GameObject pauseMenu;
    public bool gamePaused;

    [Header("Start variables")]
    public float countdownInterval = 1.0f;
    public TextMeshProUGUI countdownText;

    Scene currentScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        pauseMenu.SetActive(false);
        countdownText.text = null;
        player.GetComponent<Rigidbody>().useGravity = false;

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

    IEnumerator Countdown()
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(countdownInterval);

        countdownText.text = "2";
        yield return new WaitForSeconds(countdownInterval);

        countdownText.text = "1";
        yield return new WaitForSeconds(countdownInterval);

        countdownText.text = "GO!";
        yield return new WaitForSeconds(countdownInterval);

        countdownText.text = null;
        player.GetComponent<Rigidbody>().useGravity = true;
    }
}
