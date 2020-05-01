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
        DisableSpawnLetter();
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
    
    void DisableSpawnLetter() 
    {
        if(PlayerPrefs.GetString("d").Length == 1)
        {
            GameObject d;
            d = GameObject.Find("D");
            d.SetActive(false);
        }
        else if(PlayerPrefs.GetString("r").Length == 1)
        {
            GameObject r;
            r = GameObject.Find("R");
           r.SetActive(false);
        }
        else if(PlayerPrefs.GetString("a").Length == 1)
        {
            GameObject a;
            a = GameObject.Find("A");
            a.SetActive(false);
        }
        else if(PlayerPrefs.GetString("g").Length == 1)
        {
            GameObject g;
            g = GameObject.Find("G");
             g.SetActive(g);
        }
        else if(PlayerPrefs.GetString("o").Length == 1)
        {
            GameObject o;
            o = GameObject.Find("O");
            o.SetActive(false);
        }
        else if(PlayerPrefs.GetString("n").Length == 1)
        {
            GameObject n;
            n = GameObject.Find("N");
            n.SetActive(false);
        }
    }
}
