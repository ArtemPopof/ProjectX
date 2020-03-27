using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour
{
    public int maxCount;
    public int chance;
    public PrizeType type;
}

public enum PrizeType
{
    Coins,
    Nothing
}
