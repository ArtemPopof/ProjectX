using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public int chanceToSpawn = 50;
    public int maxToSpawn = -1;

    public GameObject egg;

    public GameObject d,r,a,g,o,n;

    // Start is called before the first frame update
    void Start()
    {
        DisableSpawnLetter();
        DisableAllChildren();
        
        if(egg != null)
        {
        DisableEgg();    
        }

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
        if (GameManager.Instance.Properties.GetInt("eggs") == 10)
        {
        egg = GameObject.FindWithTag("Egg");
        egg.SetActive(false);
        }
    }
    
    void DisableSpawnLetter() 
    {
        if (PlayerPrefs.GetString("D").Length >= 1)
        {
            d = GameObject.FindWithTag("D");
            d.SetActive(false);
        }
        else if (PlayerPrefs.GetString("R").Length >= 1)
        {
           r = GameObject.FindWithTag("R");
           r.SetActive(false);
        }
        else if (PlayerPrefs.GetString("A").Length >= 1)
        {
            a = GameObject.FindWithTag("A");
            a.SetActive(false);
        }
        else if (PlayerPrefs.GetString("G").Length >= 1)
        {
            g = GameObject.FindWithTag("G");
             g.SetActive(g);
        }
        else if (PlayerPrefs.GetString("O").Length >= 1)
        {
            o = GameObject.FindWithTag("O");
            o.SetActive(false);
        }
        else if (PlayerPrefs.GetString("N").Length >= 1)
        {
            n = GameObject.FindWithTag("N");
            n.SetActive(false);
        }
    }
}
