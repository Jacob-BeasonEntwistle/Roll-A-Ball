using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    // Screen/panel references
    public GameObject settingsScreen;
    public GameObject shopScreen;
    public GameObject powerUpSelectionScreen;
    public GameObject gamemodesScreen;

    // Button references
    public GameObject shopButton;
    public GameObject playButton;
    public GameObject settingsButton;

    public bool magnetsAvailable;
    public bool magnetEquipped;
    public GameObject magnetCheck;

    public bool extraLifeAvailable;
    public bool extraLifeEquipped;
    public GameObject extraLifeCheck;

    public TextMeshProUGUI errorMessage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Hide all extra screens
        shopScreen.SetActive(false);
        settingsScreen.SetActive(false);
        powerUpSelectionScreen.SetActive(false);
        gamemodesScreen.SetActive(false);

        RefreshPowerUpState();
    }

    public void Play()
    {
        RefreshPowerUpState();

        // Show the power-up screen and hide all buttons
        powerUpSelectionScreen.SetActive(true);
        shopButton.SetActive(false);
        playButton.SetActive(false);
        settingsButton.SetActive(false);
    }

    // Refreshes the power up states
    private void RefreshPowerUpState()
    {
        // Checks if the player has any power ups and sets their availability
        magnetsAvailable = PlayerPrefs.GetInt(PlayerPrefsManager.Magnets, 0) > 0;
        extraLifeAvailable = PlayerPrefs.GetInt(PlayerPrefsManager.ExtraLives, 0) > 0;

        // Power ups are equipped based on whether they are available and whether they are selected
        magnetEquipped = magnetsAvailable && PlayerPrefs.GetInt(PlayerPrefsManager.MagnetEquippedKey, 0) == 1;
        extraLifeEquipped = extraLifeAvailable && PlayerPrefs.GetInt(PlayerPrefsManager.ExtraLifeEquippedKey, 0) == 1;

        // If power ups are not available, sets their values in PlayerPrefs to false (0)
        if (!magnetsAvailable)
        {
            PlayerPrefsManager.SetMagnetEquipped(false);
        }

        if (!extraLifeAvailable)
        {
            PlayerPrefsManager.SetExtraLifeEquipped(false);
        }

        // Changes colour to reflect whether power-up is equipped or not
        magnetCheck.GetComponent<RawImage>().color = magnetEquipped ? Color.green : Color.white;
        extraLifeCheck.GetComponent<RawImage>().color = extraLifeEquipped ? Color.green : Color.white;
    }

    public void MagnetSelected()
    {
        if (!magnetsAvailable)
        {
            errorMessage.text = "You don't own any magnets, head to the shop to purchase some or continue...";
            return;
        }
        
        magnetEquipped = !magnetEquipped;

        if (magnetEquipped)
        {
            PlayerPrefsManager.SetMagnetEquipped(true);
            magnetCheck.GetComponent<RawImage>().color = Color.green;
        }
        else
        {
            PlayerPrefsManager.SetMagnetEquipped(false);
            magnetCheck.GetComponent<RawImage>().color = Color.white;
        }
    }
    public void ExtraLifeSelected()
    {
        if (!extraLifeAvailable)
        {
            errorMessage.text = "You don't own any extra lives, head to the shop to purchase some or continue...";
            return;
        }

        extraLifeEquipped = !extraLifeEquipped;

        if (extraLifeEquipped)
        {
            PlayerPrefsManager.SetExtraLifeEquipped(true);
            extraLifeCheck.GetComponent<RawImage>().color = Color.green;
        }
        else
        {
            PlayerPrefsManager.SetExtraLifeEquipped(false);
            extraLifeCheck.GetComponent<RawImage>().color = Color.white;
        }
    }

    public void ContinueAfterSelection()
    {
        // Hide the power-up selection screen and show the gamemodes
        powerUpSelectionScreen.SetActive(false);
        gamemodesScreen.SetActive(true);
    }

    public void PlaySurvival()
    {
        // Set the gamemode to survival and load gameplay scene
        GameManager.selectedGamemode = Gamemode.Survival;
        SceneManager.LoadScene("Gameplay");
    }

    public void PlayEndless()
    {
        // Set the gamemode to endless and load gameplay scene
        GameManager.selectedGamemode = Gamemode.Endless;
        SceneManager.LoadScene("Gameplay");
    }

    public void Settings()
    {
        // Show the settings screen
        settingsScreen.SetActive(true); 
    }

    public void OpenShop()
    {
        // Show the shop screen
        shopScreen.SetActive(true);
    }

    public void CloseAll()
    {
        // Hide all visible screens
        settingsScreen.SetActive(false);
        shopScreen.SetActive(false);
        powerUpSelectionScreen.SetActive(false);
        gamemodesScreen.SetActive(false);

        // Show all main menu buttons
        shopButton.SetActive(true);
        playButton.SetActive(true);
        settingsButton.SetActive(true);

        errorMessage.text = "";
    }
}
