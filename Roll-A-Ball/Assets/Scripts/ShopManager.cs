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
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
    }

    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        // If the amount of coins the user has is greater than how many the item costs.
        if (coins >= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().itemID])
        {
            // Take away the cost from the total of coins
            coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().itemID];
            // Update the players coins saved in player prefs
            PlayerPrefs.SetInt("Coins", coins);

            // Increment the quantity of the item
            shopItems[3, ButtonRef.GetComponent<ButtonInfo>().itemID]++;

            // Update the coins text with the amount left
            coinsText.text = $"Coins: {coins}";

            // Update the quantity text with the amount
            ButtonRef.GetComponent<ButtonInfo>().quantityText.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().itemID].ToString();
        }
    }
}
