using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject settingsScreen;
    public GameObject shopScreen;
    public GameObject gamemodesScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shopScreen.SetActive(false);
        settingsScreen.SetActive(false);
        gamemodesScreen.SetActive(false);
    }

    public void Play()
    {
        gamemodesScreen.SetActive(true);
    }

    public void PlaySurvival()
    {
        GameManager.selectedGamemode = Gamemode.Survival;
        SceneManager.LoadScene("Gameplay");
    }

    public void PlayEndless()
    {
        GameManager.selectedGamemode = Gamemode.Endless;
        SceneManager.LoadScene("Gameplay");
    }

    public void Settings()
    {
        settingsScreen.SetActive(true); 
    }

    public void OpenShop()
    {
        shopScreen.SetActive(true);
    }

    public void CloseAll()
    {
        settingsScreen.SetActive(false);
        shopScreen.SetActive(false);
    }
}
