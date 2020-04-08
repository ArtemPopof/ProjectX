using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance {set; get;}

    private const bool SNOW_COLLIDER = true;

    //Level spawning
    private const float DISTANCE_BEFORE_SPAWN = 100.0f;
    private const int INITIAL_SEGMENTS = 5;
    private const int MAX_SEGMENTS_ON_SCREEN = 15;
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

    // gameplay;
    // private bool isMoving = false;

    private void Awake ()
    {
        Instance = this;
        cameraContainer = Camera.main.transform;
        currentSpawnZ = 0;
        // currentLevel = 0;
    }

     private void Start() 
     {
         for (int i = 0; amountOfActiveSegments < INITIAL_SEGMENTS; i++)
            GenerateSegment();
     }


        private void Update()
        {
            if (currentSpawnZ - cameraContainer.position.z < DISTANCE_BEFORE_SPAWN)
                GenerateSegment();

            if (amountOfActiveSegments > MAX_SEGMENTS_ON_SCREEN)
            {
                segments[amountOfActiveSegments - 1].DeSpawn();
                amountOfActiveSegments--;
            }
        }
     private void GenerateSegment()
     {
         SpawnerSegment();
         if (Random.Range(0f, 1f) < countiousSegments * 0.25f)
         {
             // Spawn transition seg
             countiousSegments = 0;
             SpawnTransition();
         }
         else
         {
             countiousSegments++;
         }
     }

    // TODO Bad naming (not a verb)
     private void SpawnerSegment() 
     {
         List<Segment> possibleSegment = availableSegements.FindAll(
             x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3 );

        int id = Random.Range(0, possibleSegment.Count);

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
             x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3 );

        int id = Random.Range(0, possibleTransition.Count);

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
}
