using Unity.Collections;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    [Header("Coins")]
    public const string Coins = "Coins";
    public static int coins = 0;

    [Header("Power-Ups")]
    public const string Magnets = "MagnetsOwned";
    public static int magnetsOwned;
    public const string MagnetEquippedKey = "MagnetEquipped";
    public static bool magnetEquipped;

    public const string ExtraLives = "ExtraLivesOwned";
    public static int extraLivesOwned;
    public const string ExtraLifeEquippedKey = "ExtraLifeEquipped";
    public static bool extraLifeEquipped;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coins = PlayerPrefs.GetInt(Coins, 0);
        Debug.Log($"Total Coins: {coins}");

        magnetsOwned = PlayerPrefs.GetInt(Magnets, 0);
        Debug.Log($"Magnets owned: {magnetsOwned}");

        magnetEquipped = PlayerPrefs.GetInt(MagnetEquippedKey, 0) == 1;

        extraLivesOwned = PlayerPrefs.GetInt(ExtraLives, 0);
        extraLifeEquipped = PlayerPrefs.GetInt(ExtraLifeEquippedKey, 0) == 1;
    }

    // [COINS]
    public static void CoinsToAdd(int collectedCoins)
    {
        coins += collectedCoins;
    }
    public static void UpdateCoins()
    {
        PlayerPrefs.SetInt(Coins, coins);
        PlayerPrefs.Save();

        Debug.Log($"Total coins: {coins}");
    }

    // [POWER-UPS]
    public static int GetMagnetsOwned()
    {
        return PlayerPrefs.GetInt(Magnets);
    }
    public static void UpdateMagnetsOwned()
    {
        PlayerPrefs.SetInt(Magnets, magnetsOwned);
        PlayerPrefs.Save();

        Debug.Log($"Magnets owned: {magnetsOwned}");
    }
    public static void AddMagnetsOwned(int purchasedMagnets)
    {
        magnetsOwned += purchasedMagnets;
    }
    public static void SetMagnetEquipped(bool equipped)
    {
        magnetEquipped = equipped;

        PlayerPrefs.SetInt(MagnetEquippedKey, equipped ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static int GetExtraLivesOwned()
    {
        return PlayerPrefs.GetInt(ExtraLives, extraLivesOwned);
    }
    public static void UpdateExtraLivesOwned()
    {
        PlayerPrefs.SetInt(ExtraLives, extraLivesOwned);
        PlayerPrefs.Save();

        Debug.Log($"Extra Lives Owned: {extraLivesOwned}");
    }
    public static void AddExtraLivesOwned(int purchasedExtraLives)
    {
        extraLivesOwned += purchasedExtraLives;
    }
    public static void SetExtraLifeEquipped(bool equipped)
    {
        extraLifeEquipped = equipped;

        PlayerPrefs.SetInt(ExtraLifeEquippedKey, equipped ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static void DeleteData()
    {
        coins = 0;
        magnetsOwned = 0;
        extraLivesOwned = 0;
        magnetEquipped = false;
        extraLifeEquipped = false;

        PlayerPrefs.DeleteKey(Coins);
        PlayerPrefs.DeleteKey(Magnets);
        PlayerPrefs.DeleteKey(ExtraLives);
        PlayerPrefs.DeleteKey(MagnetEquippedKey);
        PlayerPrefs.DeleteKey(ExtraLifeEquippedKey);
        PlayerPrefs.Save();
    }
}
