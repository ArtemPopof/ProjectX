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

        lenght = CalculateRealLength();
    }

    private float CalculateRealLength()
    {
        var groundObjects = GameManager.FindChildrenByTag(transform, "Grass");
        if (groundObjects.Count == 0)
        {
            return lenght;
        }

        var size = 0.0f;

        groundObjects.ForEach((ground) =>
        {
            var groundSize = ground.GetComponent<MeshFilter>().mesh.bounds.size.z;
            groundSize *= ground.transform.localScale.z;

            size += groundSize;
        });


        return size;
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

        var spawnObjects = GetComponentsInChildren<CoinSpawner>();
        foreach (CoinSpawner spawnObject in spawnObjects)
        {
            spawnObject.Init();
        }
    }

    public void DeSpawn() 
    {
        //gameObject.SetActiveRecursively(false);
        gameObject.transform.localPosition = new Vector3(10000, 10000, -20000);
    }
}
