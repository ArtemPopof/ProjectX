using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeGiver : MonoBehaviour
{

    public static void GivePrize(Prize prize, int count)
    {
        switch(prize.type)
        {
            case PrizeType.Coins:
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + count);
                break;
                case PrizeType.Heart:
                PlayerPrefs.SetInt("heart", PlayerPrefs.GetInt("hearts") + count);
                break;
            default:
                break;
        }
    }
}
