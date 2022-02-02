using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public BubbleColors bubbleColor;
    // bubble mechanic
    public bool isTraversed = false;

    public Bubble RightBubble;
    public Bubble LeftBubble;
    public Bubble UpBubble;
    public Bubble DownBubble;

    public bool isThereTileDownBelow;

    Bubble belowBubble;

    public int connectedBubbleCount;
    private float timer = 0;
    private float timerRate = 0.2f;

    public MeshRenderer myRenderer;

    private MaterialsHolder materialsHolder;


    private void Awake() {
        materialsHolder = GetComponent<MaterialsHolder>();
    }

    void Update()
    {
        if (TransformFunctions.GetRaycastHit(transform.position, Vector3.right,"Bubble", 0.8f, out RaycastHit hitRight))
            RightBubble = hitRight.transform.GetComponent<Bubble>();
        else
            RightBubble = null;

        if (TransformFunctions.GetRaycastHit(transform.position, Vector3.left, "Bubble", 0.8f, out RaycastHit hitLeft))
            LeftBubble = hitLeft.transform.GetComponent<Bubble>();
        else
            LeftBubble = null;

        if (TransformFunctions.GetRaycastHit(transform.position, Vector3.forward, "Bubble", 0.8f, out RaycastHit hitUp))
            UpBubble = hitUp.transform.GetComponent<Bubble>();
        else
            UpBubble = null;

        if (TransformFunctions.GetRaycastHit(transform.position, Vector3.back, "Bubble", 0.8f, out RaycastHit hitDown))
            DownBubble = hitDown.transform.GetComponent<Bubble>();
        else
            DownBubble = null;

        if (TransformFunctions.GetRaycastHit(transform.position + Vector3.back , Vector3.down, "Tile", 1, out RaycastHit hitTile))
            isThereTileDownBelow = true;
        else
            isThereTileDownBelow = false;        

        if (!DownBubble && isThereTileDownBelow)
        {
            transform.position = transform.position + Vector3.back;
        }

        if (Time.time > timer)
        {
            GameManager.makeAllIsTraversedFalse();
            timer = Time.time + timerRate;
            connectedBubbleCount = getConnectedBubbleCount(this);
        }

        if (bubbleColor == BubbleColors.blue)
        {
            if (this.connectedBubbleCount < 4)
            {
                myRenderer.sharedMaterial = materialsHolder.materialsBlue[0];
            }
            if (this.connectedBubbleCount >= 4)
            {
                myRenderer.sharedMaterial = materialsHolder.materialsBlue[1];
            }
        }

        switch (bubbleColor)
        {
            case BubbleColors.blue:
                changeTexture(materialsHolder.materialsBlue);
                break;
            case BubbleColors.red:
                changeTexture(materialsHolder.materialsRed);
                break;
            case BubbleColors.yellow:
                changeTexture(materialsHolder.materialsYellow);
                break;
            case BubbleColors.pink:
                changeTexture(materialsHolder.materialsPink);
                break;
            case BubbleColors.purple:
                changeTexture(materialsHolder.materialsPurple);
                break;
            case BubbleColors.green:
                changeTexture(materialsHolder.materialsGreen);
                break;
        }
    }

    public void changeTexture(Material[] mats)
    {
        if (this.connectedBubbleCount < 4)
        {           
            myRenderer.sharedMaterial = mats[0];  
        }
        else if (this.connectedBubbleCount >= 4 && connectedBubbleCount < 7)
        {
            myRenderer.sharedMaterial = mats[1];
        }
        else if (connectedBubbleCount >= 7 && connectedBubbleCount < 10)
        {
            myRenderer.sharedMaterial = mats[2];
        }
        else if (connectedBubbleCount >= 10)
        {         
            myRenderer.sharedMaterial = mats[3];
        }
    }



    public int getConnectedBubbleCount(Bubble bubble)
    {
        bubble.isTraversed = true;
        connectedBubbleCount = 0;

        connectedBubbleCount++;

        if (bubble.RightBubble && !bubble.RightBubble.isTraversed && bubble.bubbleColor == bubble.RightBubble.bubbleColor)
        {
            connectedBubbleCount += getConnectedBubbleCount(bubble.RightBubble);
        }
        if (bubble.LeftBubble && !bubble.LeftBubble.isTraversed && bubble.bubbleColor == bubble.LeftBubble.bubbleColor)
        {
            connectedBubbleCount += getConnectedBubbleCount(bubble.LeftBubble);
        }
        if (bubble.UpBubble && !bubble.UpBubble.isTraversed && bubble.bubbleColor == bubble.UpBubble.bubbleColor)
        {
            connectedBubbleCount += getConnectedBubbleCount(bubble.UpBubble);
        }
        if (bubble.DownBubble && !bubble.DownBubble.isTraversed && bubble.bubbleColor == bubble.DownBubble.bubbleColor)
        {
            connectedBubbleCount += getConnectedBubbleCount(bubble.DownBubble);
        }

        return connectedBubbleCount;
    }

}
