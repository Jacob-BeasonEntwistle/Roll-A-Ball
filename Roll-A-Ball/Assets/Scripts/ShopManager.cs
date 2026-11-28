using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using JetBrains.Annotations;

public class ShopManager : MonoBehaviour
{
    public int[,] shopItems = new int[5, 5];
    private int coins;
    public TextMeshProUGUI coinsText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coins = PlayerPrefs.GetInt("Coins");
        coinsText.text = $"Coins: {coins}";

        // Setting up item ID's
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;

        // Price
        shopItems[2, 1] = 10;
        shopItems[2, 2] = 20;
        shopItems[2, 3] = 30;
        shopItems[2, 4] = 40;

        // Quantity
        shopItems[3, 1] = PlayerPrefs.GetInt("MagnetsOwned", 0);        // Magnet
        shopItems[3, 2] = PlayerPrefs.GetInt("ExtraLivesOwned", 0);     // Extra lives
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
    }

    public void Buy()
    {
        GameObject ItemRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        // If the amount of coins the user has is greater than how many the item costs.
        if (coins >= shopItems[2, ItemRef.GetComponent<ShopItem>().itemID])
        {
            // Take away the cost from the total of coins
            coins -= shopItems[2, ItemRef.GetComponent<ShopItem>().itemID];

            // Update the players coins saved in player prefs
            PlayerPrefs.SetInt("Coins", coins);
            // Update the coins text with the amount left
            coinsText.text = $"Coins: {coins}";

            // Increment the quantity of the item
            int itemID = ItemRef.GetComponent<ShopItem>().itemID;
            // ++ before value increments it first and then gets the current value
            int itemCount = ++shopItems[3, itemID];

            switch (ItemRef.GetComponent<ShopItem>().currentItemType)
            {
                case ItemType.Magnet:
                    PlayerPrefs.SetInt("MagnetsOwned", itemCount);
                    break;
                case ItemType.ExtraLife:
                    PlayerPrefs.SetInt("ExtraLivesOwned", itemCount);
                    break;
            }
            // Protects purchases from being lost if game crashes or is closed before player leaves the shop
            PlayerPrefs.Save();

            // Update the quantity text with the amount
            ItemRef.GetComponent<ShopItem>().quantityText.text = shopItems[3, itemID].ToString();
        }
    }
}
