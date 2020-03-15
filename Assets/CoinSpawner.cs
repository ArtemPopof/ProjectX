using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public int maxCoinCount = 5;
    public float chanceToSpawn = 0.5f;
    public bool forceSpawnAll = false;

    private GameObject[] coins;

    // Coin line template prefab
    // it should be parent of several
    // coin prefabs 
    public Transform coinLinePrefab;

    private void Awake()
    {
        coins = new GameObject[coinLinePrefab.childCount];
        for (int i = 0; i < coinLinePrefab.childCount; i++)
        {
            coins[i] = coinLinePrefab.GetChild(i).gameObject;
        }
    }

    private void OnEnable()
    {
        if (Random.Range(0.0f, 1.0f) < chanceToSpawn)
        {
            return;
        }

        if (forceSpawnAll)
        {
            foreach(GameObject go in coins)
            {
                go.SetActive(true);
            } 
        } else
        {
            int howMuchToSpawn = Random.Range(0, maxCoinCount);
            for (int i = 0; i < howMuchToSpawn; i++)
            {
                coins[i].SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        foreach(GameObject go in coins)
        {
            go.SetActive(false);
        }
    }
}
