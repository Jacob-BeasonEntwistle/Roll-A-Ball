using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public const string Coins = "Coins";
    public static int coins = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coins = PlayerPrefs.GetInt("Coins");
        Debug.Log($"Coins stored in PlayerPrefs: {coins}");
    }

    public static void CoinsToAdd(int collectedCoins)
    {
        coins += collectedCoins;
    }
    public static void UpdateCoins()
    {
        PlayerPrefs.SetInt("Coins", coins);
        coins = PlayerPrefs.GetInt("Coins");

        Debug.Log($"Coins stored in PlayerPrefs: {coins}");

        PlayerPrefs.Save();
    }

    public static void DeleteData()
    {
        coins = 0;
        PlayerPrefs.SetInt("Coins", coins);
        UpdateCoins();
    }
}
