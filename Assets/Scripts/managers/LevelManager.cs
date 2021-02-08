using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { set; get; }

    private const bool SNOW_COLLIDER = true;

    //Level spawning
    private const float DISTANCE_BEFORE_SPAWN = 45.0f;
    private const int INITIAL_SEGMENTS = 7;
    private const int MAX_SEGMENTS_ON_SCREEN = 7;
    public const int DISTANCE_BEFORE_FIRST_SEGMENT = 40;
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
    public List<Segment> firstLevelSegments = new List<Segment>();
    public List<Segment> secondLevelSegments = new List<Segment>();
    public List<Segment> desertSegments = new List<Segment>();
    public GameObject initSegmentFirstLevel;
    public GameObject initSegmentSecondLevel;
    public GameObject initSegmentDesert;

    private List<Segment> availableTransitions = new List<Segment>();

    [HideInInspector]
    public List<Segment> segments = new List<Segment>();

    // Some segements contained in this pool
    // Level manager uses this segments to place them on the level
    // not initialising them every time, just placing them on the right spot
    private List<Segment> segmentPool;

    private List<Segment> currentLevelSegments;

    public Material firstLevelSkybox;
    public Material secondLevelSkybox;
    //public Material desertSkybox;

    // gameplay;
    // private bool isMoving = false;

    private void Awake()
    {
        Instance = this;
        cameraContainer = Camera.main.transform;
        currentSpawnZ = 0;
    }

    void Start()
    {
        // level indexes start with 100
        var currentLevel = PlayerPrefs.GetInt("currentLevel") - 100;
        if (currentLevel == 2)
        {
            currentLevelSegments = firstLevelSegments;
            initSegmentFirstLevel.SetActive(true);
            initSegmentSecondLevel.SetActive(false);
            initSegmentDesert.SetActive(false);
        }
        else if (currentLevel == 1)
        {
            currentLevelSegments = secondLevelSegments;
            initSegmentFirstLevel.SetActive(false);
            initSegmentSecondLevel.SetActive(true);
            initSegmentDesert.SetActive(false);
        }
        else if (currentLevel == 0)
        {
            currentLevelSegments = desertSegments;
            initSegmentFirstLevel.SetActive(false);
            initSegmentSecondLevel.SetActive(false);
            initSegmentDesert.SetActive(true);
        }

        InitLightingAndSkyboxSettings(currentLevel);

        segmentPool = new List<Segment>((currentLevelSegments.Count + availableTransitions.Count) * 2);

        initializeSegmentPool();

        for (int i = 0; amountOfActiveSegments < INITIAL_SEGMENTS; i++)
            SpawnSegment();
    }

    private void InitLightingAndSkyboxSettings(int levelCode)
    {
        // default level
        if (levelCode == 0 || levelCode == 2)
        {
            RenderSettings.skybox = firstLevelSkybox;
            RenderSettings.ambientSkyColor = Color.white;
        }
        else
        {
            // fire level
            RenderSettings.skybox = secondLevelSkybox;
            RenderSettings.ambientSkyColor = new Color(0.4f, 0.4f, 0.4f);
        }
    }

    private void initializeSegmentPool()
    {
        for (int i = 0; i < currentLevelSegments.Count * 2; i++)
        {
            var gameObject = Instantiate(currentLevelSegments[i / 2].gameObject) as GameObject;
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

    private void Update()
    {
        if (currentSpawnZ - cameraContainer.position.z < DISTANCE_BEFORE_SPAWN)
        {
            SpawnSegment();
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

    private void SpawnSegment()
    {
        var id = UnityEngine.Random.Range(0, segmentPool.Count);
        var segment = segmentPool[id];
        segmentPool.RemoveAt(id);

        //Debug.Log("Spawn " + segment.name);

        segment.transform.localPosition = Vector3.forward * (currentSpawnZ + DISTANCE_BEFORE_FIRST_SEGMENT);

        segments.Insert(0, segment);
        if (segments.Count > MAX_SEGMENTS_ON_SCREEN)
        {
            Despawn();
        }

        currentSpawnZ += segment.lenght;
        amountOfActiveSegments++;

        //Debug.Log("CurrentZ: " + currentSpawnZ);
        segment.Spawn();
    }

    public Segment GetSegment(int id, bool transition)
    {
        Segment segment = null;
        segment = segments.Find(x => x.SegmentId == id && x.transition == transition && !x.gameObject.activeSelf);

        if (segment == null)
        {
            GameObject go = Instantiate((transition) ? availableTransitions[id].gameObject : currentLevelSegments[id].gameObject) as GameObject;
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
