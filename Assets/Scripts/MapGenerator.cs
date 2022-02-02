using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Vector2Int mapSize;

    [Range(0 , 1)]
    public float outlinePercent;

    public float height;


    public void GenerateMap()
    {
        String holderName = "Generated Map";

        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject (holderName).transform;
        mapHolder.parent = transform;


        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x/2 + 0.5f + x , 0 , -mapSize.y/2 + 0.5f + y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                newTile.localScale = new Vector3((1 - outlinePercent), height ,(1 - outlinePercent));
                newTile.parent = mapHolder;
            }
        }
    }
}
