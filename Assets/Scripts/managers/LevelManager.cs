using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { set; get; }

    private const bool SNOW_COLLIDER = true;

    //Level spawning
    private const float DISTANCE_BEFORE_SPAWN = 40.0f;
    private const int INITIAL_SEGMENTS = 5;
    private const int MAX_SEGMENTS_ON_SCREEN = 9;
    public const int DISTANCE_BEFORE_FIRST_SEGMENT = 50;
    private Transform cameraContainer;
    private int amountOfActiveSegments;
    private int countiousSegments;
    //  private int currentLevel;
    private float currentSpawnZ;

    private int y1, y2, y3;

    //List of pieces

    public List<Piece> ramps = new List<Piece>();
    public List<Piece> longBlocks = new List<Piece>();
    public List<Piece> jumps = new List<Piece>();
    public List<Piece> slides = new List<Piece>();
    public List<Piece> houses = new List<Piece>();

    [HideInInspector]
    public List<Piece> pieces = new List<Piece>();

    // list of segments;
    public List<Segment> availableSegements = new List<Segment>();
    public List<Segment> availableTransitions = new List<Segment>();
    [HideInInspector]
    public List<Segment> segments = new List<Segment>();

    // Some segements contained in this pool
    // Level manager uses this segments to place them on the level
    // not initialising them every time, just placing them on the right spot
    public List<Segment> segmentPool;

    // gameplay;
    // private bool isMoving = false;

    private void Awake()
    {
        Instance = this;
        cameraContainer = Camera.main.transform;
        currentSpawnZ = 0;
        // currentLevel = 0;

        segmentPool = new List<Segment>((availableSegements.Count + availableTransitions.Count) * 2);

        var current = DateTime.Now;
        initializeSegmentPool();
        var delta = (DateTime.Now - current);
        GameManager.Instance.Properties.setProperty("debug", delta.TotalMilliseconds);
    }

    private void initializeSegmentPool()
    {
        for (int i = 0; i < availableSegements.Count * 2; i++)
        {
            var gameObject = Instantiate(availableSegements[i / 2].gameObject) as GameObject;
            var segment = gameObject.GetComponent<Segment>();
            segmentPool.Add(segment);
        }
        for (int i = 0; i < availableTransitions.Count * 2; i++)
        {
            var gameObject = Instantiate(availableTransitions[i / 2].gameObject) as GameObject;
            var segment = gameObject.GetComponent<Segment>();
            segmentPool.Add(segment);
        }

        foreach (var segment in segmentPool) {
            segment.transform.SetParent(transform);
            segment.gameObject.SetActive(false);
            segment.transform.localPosition = new Vector3(-1000, -1000, -1000); // some distant from level point
        }
    }

    private void Start()
    {
        for (int i = 0; amountOfActiveSegments < INITIAL_SEGMENTS; i++)
            GenerateSegment();
    }


    private void Update()
    {
        if (currentSpawnZ - cameraContainer.position.z < DISTANCE_BEFORE_SPAWN)
        {
            GenerateSegment();
        }
    }

    private void Despawn()
    {       
        var last = segments.Count - 1;
        segments[last].DeSpawn();

        // put back in the pool
        segmentPool.Add(segments[last]);

        segments.RemoveAt(last);
        amountOfActiveSegments--;
    }

    private void GenerateSegment()
    {
        SpawnSegment();

        //if (UnityEngine.Random.Range(0f, 1f) < countiousSegments * 0.25f)
        // {
        //    // Spawn transition seg
        //     countiousSegments = 0;
        //    SpawnTransition();
        //}
        //else
        //{
        //    countiousSegments++;
        //}
    }
    private void SpawnSegment()
    {
        var id = UnityEngine.Random.Range(0, segmentPool.Count);
        var segment = segmentPool[id];
        segmentPool.RemoveAt(id);

        segment.transform.localPosition = Vector3.forward * (currentSpawnZ + DISTANCE_BEFORE_SPAWN);

        segments.Insert(0, segment);
        if (segments.Count > MAX_SEGMENTS_ON_SCREEN)
        {
            Despawn();
        }

        currentSpawnZ += segment.lenght;
        amountOfActiveSegments++;
        segment.Spawn();
    }

    private void SpawnSegmentOld()
    {
        List<Segment> possibleSegment = availableSegements.FindAll(
            x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);

        int id = UnityEngine.Random.Range(0, possibleSegment.Count);

        // TODO what does bool do?
        Segment segment = GetSegment(id, false);


        y1 = segment.endY1;
        y2 = segment.endY2;
        y3 = segment.endY3;


        segment.transform.SetParent(transform);
        segment.transform.localPosition = Vector3.forward * (currentSpawnZ + DISTANCE_BEFORE_FIRST_SEGMENT);

        currentSpawnZ += segment.lenght;
        amountOfActiveSegments++;
        segment.Spawn();
    }

    public Segment GetSegment(int id, bool transition)
    {
        Segment segment = null;
        segment = segments.Find(x => x.SegmentId == id && x.transition == transition && !x.gameObject.activeSelf);

        if (segment == null)
        {
            GameObject go = Instantiate((transition) ? availableTransitions[id].gameObject : availableSegements[id].gameObject) as GameObject;
            segment = go.GetComponent<Segment>();

            segment.SegmentId = id;
            segment.transition = transition;
            segments.Insert(0, segment);
        }
        else
        {
            segments.Remove(segment);
            segments.Insert(0, segment);
        }

        return segment;
    }

    private void SpawnTransition()
    {
        List<Segment> possibleTransition = availableTransitions.FindAll(
            x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);

        int id = UnityEngine.Random.Range(0, possibleTransition.Count);

        Segment segment = GetSegment(id, true);

        y2 = segment.endY2;
        y3 = segment.endY3;

        segment.transform.SetParent(transform);
        segment.transform.localPosition = Vector3.forward * (currentSpawnZ + DISTANCE_BEFORE_FIRST_SEGMENT);

        currentSpawnZ += segment.lenght;
        amountOfActiveSegments++;
        segment.Spawn();
    }

    public Piece GetPiece(PieceType pieceType, int visualIndex)
    {
        Piece piece = pieces.Find(x => x.type == pieceType && x.visialIndex == visualIndex && !x.gameObject.activeSelf);

        if (piece == null)
        {
            GameObject go = null;
            if (pieceType == PieceType.ramp)
                go = ramps[visualIndex].gameObject;
            else if (pieceType == PieceType.longBlock)
                go = longBlocks[visualIndex].gameObject;
            else if (pieceType == PieceType.jump)
                go = jumps[visualIndex].gameObject;
            else if (pieceType == PieceType.slide)
                go = slides[visualIndex].gameObject;
            else if (pieceType == PieceType.house)
                go = houses[visualIndex].gameObject;

            go = Instantiate(go);
            piece = go.GetComponent<Piece>();
            pieces.Add(piece);
        }
        return piece;
    }

    public Segment GetSegementByGameObject(GameObject someObject)
    {
        var segment = someObject.transform.GetComponentInParent<Segment>();
        if (segment == null)
        {
            throw new ArgumentOutOfRangeException("Cannot find segment for given playerPosition");
        }

        return segment;
    }

    public Segment GetNextSegment(Segment currentSegment)
    {
        for (int i = 0; i < segments.Count; i++)
        {
            if (segments[i].transform.position.z == currentSegment.transform.position.z)
            {
                return segments[i - 1];
            }
        }

        throw new ArgumentOutOfRangeException("Cannot find next segment for given one");
    }
}
