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
                coinSpawnIndex = RandomCoin();
                coinSpawnSelected = true;
            }
           
            Vector3 coinSpawnLocation = coinSpawnPoints[coinSpawnIndex].transform.position;

            // Create an instance of the coin prefab at the selected coin's position with a rotation and set the parent to the ground object
            GameObject coinInstance = Instantiate(coinPrefab, new Vector3(coinSpawnLocation.x, coinSpawnLocation.y, coinSpawnLocation.z), Quaternion.identity, ground.transform);
            coinSpawnSelected = false;
            spawnTimer = 0;
        }
    }

    private int RandomCoin()
    {
        return Random.Range(0, coinSpawnPoints.Length);
    }
}
