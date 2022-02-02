using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Tile[] tiles;

    [SerializeField]
    private Transform[] bubblePrefabs;

    public Transform bubbleHolder;

    public static float DistBtwTileAndBubble = 0.4f;

    public MapGenerator mapGenerator;

    private float timer = 2;
    private float ShuffleTimeRate = 2;

    public Bubble[] bubbles;
    public Bubble[] shuffledBubbles;

    public int seed;

    void Awake()
    {
        

        // Game Initialization

        // Getting all active tiles in the scene
        Transform tileHolder = Transform.FindObjectOfType<MapGenerator>().transform.GetChild(0);
        tiles = new Tile[tileHolder.childCount];

        for (int i = 0; i < tileHolder.childCount; i++)
        {
            Transform child = tileHolder.GetChild(i);
            tiles[i] = child.GetComponent<Tile>();
        }

        // Giving all tiles a random bubble
        for (int i = 0; i < tiles.Length; i++)
        {
            
            tiles[i].IsThereBubble = true;
            Transform randomBubble = Instantiate(
                bubblePrefabs[UnityEngine.Random.Range(0, bubblePrefabs.Length)]
                ,tiles[i].transform.position + new Vector3(0,DistBtwTileAndBubble,0)
                ,Quaternion.Euler(0,180,0)
            );

            randomBubble.SetParent(bubbleHolder);
        }
    }

    private void Update() {
        destroyExtraBubbles();
        if (Time.time > timer)
        {
            ShuffleBubblesOnDeadlock();
            timer = Time.time + ShuffleTimeRate;
        }

        setAllTilesUpLine();
    }

    public static void destroyConnectedBubbles(Bubble bubble)
    {
        bubble.isTraversed = true;

        if(bubble.RightBubble && !bubble.RightBubble.isTraversed && bubble.bubbleColor == bubble.RightBubble.bubbleColor)
        {
            destroyConnectedBubbles(bubble.RightBubble);
        }
        if(bubble.LeftBubble && !bubble.LeftBubble.isTraversed &&  bubble.bubbleColor == bubble.LeftBubble.bubbleColor)
        {
            destroyConnectedBubbles(bubble.LeftBubble);
        }
        if(bubble.UpBubble && !bubble.UpBubble.isTraversed &&  bubble.bubbleColor == bubble.UpBubble.bubbleColor)
        {
            destroyConnectedBubbles(bubble.UpBubble);
        }
        if(bubble.DownBubble && !bubble.DownBubble.isTraversed &&  bubble.bubbleColor == bubble.DownBubble.bubbleColor)
        {
            destroyConnectedBubbles(bubble.DownBubble);
        }

        Destroy(bubble.gameObject);        
    }


    public static void makeAllIsTraversedFalse()
    {
        Bubble[] bubblesInScene = FindObjectsOfType<Bubble>();
        foreach (var item in bubblesInScene)
        {
            item.isTraversed = false;
        }
    }

    public void setAllTilesUpLine()
    {
        List<Tile> tilesUpLine = new List<Tile>();
        foreach (Tile tile in tiles)
        {
            if (mapGenerator.mapSize.y % 2 == 0)
            {
                if (tile.transform.position.z == mapGenerator.mapSize.y / 2 - 0.5f)
                {
                    tilesUpLine.Add(tile);
                    tile.amIinUpLine = true;
                }
                else
                {
                    tile.amIinUpLine = false;
                }
            }
            else
            {
                if (tile.transform.position.z == mapGenerator.mapSize.y / 2 + 0.5f)
                {
                    tilesUpLine.Add(tile);
                    tile.amIinUpLine = true;
                }
                else
                {
                    tile.amIinUpLine = false;
                }
            }
            
        }
    }

    public void destroyExtraBubbles()
    {
        GetActiveBubbles();
        for (int i = 0; i < bubbles.Length; i++)
        {
            if (i > tiles.Length - 1)
            {
                Destroy(bubbles[i].gameObject);
            }
        }
        
    }

    public Bubble createRandomBubble(Vector3 tilePos)
    {
        Transform randomBubble = Instantiate(
                bubblePrefabs[UnityEngine.Random.Range(0, bubblePrefabs.Length)]
                , tilePos + new Vector3(0, DistBtwTileAndBubble, 0)
                , Quaternion.Euler(0, 180, 0)
            );
        randomBubble.SetParent(bubbleHolder);
        return randomBubble.GetComponent<Bubble>();
    }

    public void ShuffleBubblesOnDeadlock()
    {
        GetActiveBubbles();

        if (IsThereDeadlockOnBubbles(bubbles))
        {
            shuffledBubbles = new Bubble[bubbles.Length];
            Array.Copy(bubbles, shuffledBubbles, shuffledBubbles.Length);

            shuffledBubbles = Utility.ShuffleArray(shuffledBubbles, seed);

            seed++;

            for (int i = 0; i < shuffledBubbles.Length; i++)
            {
                bubbles[i].transform.position = shuffledBubbles[i].transform.position;
            }
        }

    }

    private void GetActiveBubbles()
    {
        bubbles = new Bubble[tiles.Length];
        bubbles = FindObjectsOfType<Bubble>();
    }

    public bool IsThereDeadlockOnBubbles(Bubble[] bubbles)
    {
        foreach (Bubble bubble in bubbles)
        {
            if (bubble.connectedBubbleCount >= 2)
            {
                return false;
            }
        }
        return true;
    }

    // These Functions are obsolete , getting bubbles or tiles with positions can be able to create easy to miss errors

    // private Tile getTileBelowBubble(Bubble bubble)
    // {
    //     Vector3 pos = bubble.transform.position - new Vector3(0,DistBtwTileAndBubble,0);

    //     return getTileFromPosition(pos);
    // }

    // public static Bubble getBubbleFromPosition(Vector3 pos)
    // {
    //     BubblesAndPositions.TryGetValue(pos, out Bubble bubble);

    //     return bubble;
    // }

    // public static Tile getTileFromPosition(Vector3 pos)
    // {
    //     TilesAndPositions.TryGetValue(pos, out Tile tile);

    //     return tile;
    // }
}
