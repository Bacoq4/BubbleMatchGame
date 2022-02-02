using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsThereBubble;

    public Bubble bubble;

    public bool amIinUpLine;

    private void Update() {

        if (TransformFunctions.GetRaycastHit(transform.position, Vector3.up, "Bubble", 10, out RaycastHit hit))
        {
            IsThereBubble = true;
        }
        else
        {
            IsThereBubble = false;
        }

        if (amIinUpLine && !IsThereBubble)
        {
            Bubble bubble = FindObjectOfType<GameManager>().createRandomBubble(transform.position);
        }
        
    }

    
}
