using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameManager gameManager;

    public Dictionary<string, int> countHolder = new Dictionary<string, int>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.makeAllIsTraversedFalse();

            Transform objectHit = TransformFunctions.GetHitTransformFromMouse("Bubble");
            Bubble hitBubble = objectHit.GetComponent<Bubble>();

            if (objectHit == null) return;
            if (hitBubble.connectedBubbleCount >= 2)
            {
                GameManager.destroyConnectedBubbles(hitBubble);    
            }
            
            
        }
    }
}
