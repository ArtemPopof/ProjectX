using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrizeScene : MonoBehaviour
{
    public static string CURRENT_PRIZE = "currentPrizeScene";
    public Transform prizes;
    public Transform closedPrize;
    public Text prizeDescriptionText;
    public Transform emptyPrizePanel;
    public Transform receivedPrizeLabel;
    public Text yayLabel;

    public GameObject chestSceneObjects;
    public GameObject letterSceneObjects;
    public GameObject eggSceneObjects;

    private bool open = false;
    private PrizeType type;

    // Start is called before the first frame update
    void Start()
    {
        type = (PrizeType) PlayerPrefs.GetInt(CURRENT_PRIZE);
        var description = GetPrizeDescriptionText(type);
        prizeDescriptionText.text = description;

        CustomizeSceneForType(type);
    }

    private string GetPrizeDescriptionText(PrizeType type)
    {
        switch (type)
        {
            case PrizeType.CHEST:
                return "You found a chest";
            case PrizeType.EGG_QUEST:
                return "All eggs collected!";
            case PrizeType.LETTER_QUEST:
                return "All letters collected!";
        }

        throw new InvalidOperationException();
    }

    private void CustomizeSceneForType(PrizeType type)
    {
        switch (type)
        {
            case PrizeType.CHEST:
                chestSceneObjects.SetActive(true);
                break;
            case PrizeType.EGG_QUEST:
                eggSceneObjects.SetActive(true);
                break;
            case PrizeType.LETTER_QUEST:
                letterSceneObjects.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPrizeClick()
    {
        if (open)
        {
            SceneManager.Instance.GoToNextScene();
            return;
        }

        open = true;
        SoundManager.PlaySound("Chest");
        yayLabel.text = "Wow! Click to continue";
        //closedPrize.gameObject.SetActive(false);

        // generate random prize
        //bool prizeIsEmpty = Random.Range(0, 100) <= chanceToGetNothing;
        if (false)
        {
            receivedPrizeLabel.gameObject.SetActive(false);
            GiveEmptyPrize();
            return;
        }
        GiveMaxPrize();
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
        var text = receivedPrizeLabel.GetComponent<Text>();
        text.text = prize.type.ToString() + " x " + count.ToString();

        PrizeGiver.GivePrize(prize, count);
    }

    private bool IsOpenAlready()
    {
        return !closedPrize.gameObject.activeSelf;
    }

    private void GiveMaxPrize() 
    {

        var prizeIndex = UnityEngine.Random.Range(0, prizes.childCount);
        var prize = prizes.GetChild(prizeIndex);
        prize.gameObject.SetActive(true);

        var maxPrizeCount = GetMaxPrizeCount(prize);
        var prizeCount = UnityEngine.Random.Range((int) (0.1 * maxPrizeCount), maxPrizeCount);
        prizeCount = Mathf.Max(1, prizeCount);

        GivePrize(prize.GetComponent<Prize>(), prizeCount);
    }

    public enum PrizeType {
        CHEST,
        EGG_QUEST,
        LETTER_QUEST
    }
}
