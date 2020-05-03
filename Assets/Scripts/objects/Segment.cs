using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public int SegmentId {set; get;}
    public bool transition;

    public float lenght;
    public int beginY1, beginY2, beginY3;
    public int endY1, endY2, endY3;

    private Piece[] pieces;
    public GameObject SegmentObjects { get; set; }

    private void Awake() 
    {
        pieces = gameObject.GetComponentsInChildren<Piece>();
        SegmentObjects = FindSegmentObjects();
        SegmentObjects.SetActive(true);
    }

    private GameObject FindSegmentObjects()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("GameObjects"))
            {
                return child.gameObject;
            }
        }

        throw new InvalidProgramException("Segment ["+ name +"] must contain GameObjects object inside");
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void DeSpawn() 
    {
        gameObject.SetActive(false);
    }
}
