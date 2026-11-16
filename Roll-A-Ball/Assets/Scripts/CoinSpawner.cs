using NUnit.Framework;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject[] coinSpawnPoints;
    public GameObject coinPrefab;
    public GameObject ground;

    private float spawnTimer;
    private float spawnDelay;

    private bool coinSpawnSelected;
    private int coinSpawnIndex;
    private int previousSpawnIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinSpawnSelected = false;
        spawnTimer = 0;
        spawnDelay = 3;
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
            Vector3 coinSpawnLocation = coinSpawnPoints[coinSpawnIndex].transform.position;

            // Create an instance of the coin prefab at the selected coin's position with a rotation and set the parent to the ground object
            GameObject coinInstance = Instantiate(coinPrefab, new Vector3(coinSpawnLocation.x, coinSpawnLocation.y, coinSpawnLocation.z), Quaternion.identity, ground.transform);

            // Makes the current index the previous one before spawning next coin
            previousSpawnIndex = coinSpawnIndex;

            coinSpawnSelected = false;
            spawnTimer = 0;
        }
    }

    private int RandomLocation()
    {
        // Randomly returns a value between 0 and the number of spawn points within the list
        return Random.Range(0, coinSpawnPoints.Length);
    }
}
