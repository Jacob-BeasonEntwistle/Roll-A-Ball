using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonInfo : MonoBehaviour
{
    public int itemID;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI quantityText;
    public GameObject shopManager;

    // Update is called once per frame
    void Update()
    {
        priceText.text = $"Price: {shopManager.GetComponent<ShopManager>().shopItems[2, itemID]}";
        quantityText.text = $"{shopManager.GetComponent<ShopManager>().shopItems[2, itemID]}";
    }
}
