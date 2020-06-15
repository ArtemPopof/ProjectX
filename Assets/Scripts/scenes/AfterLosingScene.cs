using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterLosingScene : MonoBehaviour
{
    public Transform prizes;
    public Transform closedPrize;
    private const int chanceToGetNothing = 5;
    public Transform emptyPrizePanel;
    public Transform footer;

    public PrizeType prize;

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
        if (IsOpenAlready())
        {
            SceneManager.Instance.GoToNextScene();
            return;
        }

        if (PlayerPrefs.GetInt("eggs") == 0) {
            return;
        }

        SoundManager.PlaySound("Chest");
        if (PlayerPrefs.GetInt("isWordCollected") == 1)
        {
            GiveMaxPrize();
        }

        closedPrize.gameObject.SetActive(false);

        // generate random prize
        bool prizeIsEmpty = Random.Range(0, 100) <= chanceToGetNothing;
        if (prizeIsEmpty)
        {
            footer.gameObject.SetActive(false);
            GiveEmptyPrize();
            return;
        }
        // GiveMaxPrize();
    }

    private void GiveEmptyPrize()
    {
        emptyPrizePanel.gameObject.SetActive(true);
    }

    private int GetMaxPrizeCount(Transform prize)
    {
        var prizeComponent = prize.GetComponent<Prize>();
        return prizeComponent.maxCount;
    }

    private void GivePrize(Prize prize, int count)
    {
        var text = footer.GetComponent<Text>();
        text.text = prize.type.ToString() + " x " + count.ToString();

        PrizeGiver.GivePrize(prize, count);
    }

    private bool IsOpenAlready()
    {
        return !closedPrize.gameObject.activeSelf;
    }

    private void GiveMaxPrize() 
    {

        var prizeIndex = Random.Range(0, prizes.childCount);
        var prize = prizes.GetChild(prizeIndex);
        prize.gameObject.SetActive(true);

        var maxPrizeCount = GetMaxPrizeCount(prize);
        var prizeCount = Random.Range((int) (0.1 * maxPrizeCount), maxPrizeCount);
        prizeCount = Mathf.Max(1, prizeCount);

        GivePrize(prize.GetComponent<Prize>(), prizeCount);
    }

    public enum PrizeType {
        CHEST,
        EGG_QUEST,
        LETTER_QUEST
    }
}
