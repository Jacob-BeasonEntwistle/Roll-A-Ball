using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ItemType
{ 
    Magnet, ExtraLife
}

public class ShopItem : MonoBehaviour
{
    public int itemID;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI quantityText;
    public GameObject shopManager;

    public ItemType currentItemType;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        priceText.text = $"Price: {shopManager.GetComponent<ShopManager>().shopItems[2, itemID]}";
        quantityText.text = $"{shopManager.GetComponent<ShopManager>().shopItems[3, itemID]}";
    }
}
