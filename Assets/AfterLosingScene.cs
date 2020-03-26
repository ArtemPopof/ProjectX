using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterLosingScene : MonoBehaviour
{
    public Transform prizes;
    public Transform closedPrize;
    private const int chanceToGetNothing = 5;
    public Transform emptyPrizePanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPrizeClick()
    {
        closedPrize.gameObject.SetActive(false);

        // generate random prize
        bool prizeIsEmpty = UnityEngine.Random.Range(0, 100) <= chanceToGetNothing;
        if (prizeIsEmpty)
        {
            GiveEmptyPrize();
            return;
        }

        var prizeIndex = UnityEngine.Random.Range(0, prizes.childCount);
        var prize = prizes.GetChild(prizeIndex);
        prize.gameObject.SetActive(true);
    }

    private void GiveEmptyPrize()
    {
        emptyPrizePanel.gameObject.SetActive(true);
    }
}
