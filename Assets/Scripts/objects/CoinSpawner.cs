using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public int chanceToSpawn = 50;
    public int maxToSpawn = -1;

    public GameObject egg;

    // Start is called before the first frame update
    void Start()
    {
        DisableAllChildren();
        
        // if(egg != null)
        // {
        // DisableEgg();    
        // }

        if (!IsLuckyToSpawn())
        {
            return;
        }

        if (maxToSpawn < 0) maxToSpawn = transform.childCount;

        EnableLuckyChildren();
    }

    private void EnableLuckyChildren()
    {
        for (int i = 0; i < maxToSpawn; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void DisableAllChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private bool IsLuckyToSpawn()
    {
        int random = UnityEngine.Random.Range(0, 100);
        return random <= chanceToSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisableEgg()
    {
        if(GameManager.Instance.Properties.GetInt("eggs") == 10)
        {
        egg = GameObject.FindWithTag("Egg");
        egg.SetActive(false);
        }
    }
}
