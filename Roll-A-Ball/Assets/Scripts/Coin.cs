using System.Runtime.InteropServices;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;

public class Coin : MonoBehaviour
{
    [SerializeField] private Material common;
    [SerializeField] private Material uncommon;
    [SerializeField] private Material rare;
    [SerializeField] private Material epic;
    [SerializeField] private Material legendary;

    private Material currentRarity;

    private float lifetime;
    private int value;

    private void Start()
    {
        int coinRarity = RandomRarity();

        switch (coinRarity)
        {
            case 0:
                currentRarity = common;
                value = 1;
                break;
            case 1:
                currentRarity = uncommon;
                value = 10;
                break;
            case 2:
                currentRarity = rare;
                value = 20;
                break;
            case 3:
                currentRarity = epic;
                value = 50;
                break;
            case 4:
                currentRarity = legendary;
                value = 100;
                break;
        }

        GetComponent<Renderer>().material = currentRarity;
    }

    private void Update()
    {
        lifetime += Time.deltaTime;

        if (lifetime > 10)
        {
            Destroy(this.gameObject);
        }
    }

    private int RandomRarity()
    {
        int chanceRoll = Random.Range(0, 100);
        int rarity;

        // 0-59: 60% chance of common
        if (chanceRoll < 60)
        {
            rarity = 0;
        }
        // 60-84: 25% chance of uncommon
        else if (chanceRoll < 85)
        {
            rarity = 1;
        }
        // 85-94: 10% chance of rare
        else if (chanceRoll < 95)
        {
            rarity = 2;
        }
        // 95-98: 4% chance of epic
        else if (chanceRoll < 99)
        {
            rarity = 3;
        }
        // 99: 1% chance of legendary
        else
        {
            rarity = 4;
        }

        return rarity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerState>().AddCoins(value);
            Destroy(this.gameObject);
        }
    }
}
