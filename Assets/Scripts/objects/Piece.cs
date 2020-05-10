﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType{
    letter = -1,
    ramp = 0,
    longBlock =1,
    jump = 2,
    slide = 3,
    house = 4,

}

public class Piece : MonoBehaviour
{
    public PieceType type;
    public int visialIndex;
}
