using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class CoinSpawner : MonoBehaviour
{
    public GameObject[] coinSpawnPoints;
    public GameObject coinPrefab;
    public GameObject ground;

    private float spawnTimer;
    private float spawnDelay;

    private bool coinSpawnSelected;
    private Vector3 coinSpawnLocation;
    private int coinSpawnIndex;
    private int previousSpawnIndex;

    public GameObject extraLifePrefab;
    private bool extraLifeSpawned;
    private float randomLifeSpawnTime;

    [SerializeField] private GameManager gameManager;
    private Gamemode currentGamemode;
    private float totalTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinSpawnSelected = false;
        spawnTimer = 0;
        spawnDelay = 3;

        currentGamemode = gameManager.GetSelectedGamemode();

        totalTime = gameManager.GetTotalTime();

        randomLifeSpawnTime = ExtraLifeSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnDelay)
        {
            if (coinSpawnSelected == false)
            {
                coinSpawnIndex = RandomLocation();

                // If the coin spawn index is the same as the previous one
                if (coinSpawnIndex == previousSpawnIndex)
                {
                    // Modulo (%) prevents a number larger than the number of spawn points
                    // by wrapping it back into the valid range
                    coinSpawnIndex = (coinSpawnIndex + 1) % coinSpawnPoints.Length;
                }

                coinSpawnSelected = true;
            }

            // Choose the position of the randomly selected spawn point index
            coinSpawnLocation = coinSpawnPoints[coinSpawnIndex].transform.position;

            // Create an instance of the coin prefab at the selected coin's position with a rotation and set the parent to the ground object
            GameObject coinInstance = Instantiate(coinPrefab, new Vector3(coinSpawnLocation.x, coinSpawnLocation.y, coinSpawnLocation.z), Quaternion.identity, ground.transform);

            // Makes the current index the previous one before spawning next coin
            previousSpawnIndex = coinSpawnIndex;

            coinSpawnSelected = false;
            spawnTimer = 0;
        }

        if (extraLifeSpawned == false)
        {
            switch (currentGamemode)
            {
                case Gamemode.Survival:
                    if (gameManager.GetCurrentTime() <= randomLifeSpawnTime)
                    {
                        SpawnExtraLife();
                    }
                    break;
                case Gamemode.Endless:
                    if (gameManager.GetCurrentTime() >= randomLifeSpawnTime)
                    {
                        SpawnExtraLife();
                    }
                    break;
            }
        }
    }

    private int RandomLocation()
    {
        // Randomly returns a value between 0 and the number of spawn points within the list
        return Random.Range(0, coinSpawnPoints.Length);
    }

    private float ExtraLifeSpawnTime()
    {
        switch (currentGamemode)
        {
            case Gamemode.Survival:
                return Random.Range(0, totalTime - 10.0f);
            case Gamemode.Endless:
                return Random.Range(0, totalTime);
            default:
                return 0;
        }
    }

    private void SpawnExtraLife()
    {
        extraLifeSpawned = true;

        int randomLifeIndex = RandomLocation();
        Vector3 randomLifeLocation = coinSpawnPoints[randomLifeIndex].transform.position;

        GameObject extraLife = Instantiate(extraLifePrefab, new Vector3(randomLifeLocation.x, randomLifeLocation.y, randomLifeLocation.z), Quaternion.identity, ground.transform);
        Debug.Log("Extra life spawned!");
    }
}
