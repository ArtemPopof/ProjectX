﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeGiver : MonoBehaviour
{

    public static void GivePrize(Prize prize, int count)
    {
        switch(prize.type)
        {
            case PrizeType.Coins:
                GameManager.Instance.AddCoins(count);
                break;
            default:
                break;
        }
    }
}