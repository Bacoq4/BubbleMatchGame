using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformFunctions : MonoBehaviour
{

    public enum directions
    {
        Up,
        right,
        bottom,
        left,
        UpLeft,
        UpRight,
        Bottomleft,
        BottomRight,
        UpRightSpline
    }

    

    public static T getLastChildFromList<T>(List<T> list)
    {
        return list[list.Count - 1];
    }

    public static Text create3DText(Transform parent,String str , Vector3 pos , Transform CanvasPrefab)
    {
        // text need to be first child of the canvas prefab
        Transform go = TransformFunctions.createObject(CanvasPrefab.gameObject);
        go.transform.position = pos;
        TransformFunctions.addObjectToParent(parent, go.transform);
        Text objText = go.GetChild(0).GetComponent<Text>();
        objText.text = str;
        return objText;
    }

    public static void MoveOneObject(Transform gObj, Vector3 pos, float speed)
    {
        gObj.position = Vector3.MoveTowards(gObj.position, pos, speed*Time.deltaTime);
    }

    public static void LookAtObject(Transform gObj, Transform target)
    {
        gObj.LookAt(target);
    }

    public static Vector3 GetHitPositionOnPlane(Vector3 startPos)
    {
        RaycastHit Hit;
        Vector3 v = new Vector3(0, 1, 0);
        Physics.Raycast(startPos + v, Vector3.down, out Hit, 100);
        return Hit.point;
    }

    public static Transform GetHitTransformFromMouse(String layerMask)
    {
        RaycastHit hit;
        // Camera need to be "MainCamera"
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Transform objectHit;

        if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask(layerMask)))
        {
            objectHit = hit.transform;
            return objectHit;
        }
        else
        {
            return null;
        }
    }
    public static bool GetRaycastHit(Vector3 startPos, Vector3 direction,String layerMask, float distance, out RaycastHit hit)
    {
        // Camera need to be "MainCamera"
        Ray ray = new Ray();

        ray.origin = startPos;
        ray.direction = direction;

        if (Physics.Raycast(ray, out hit, distance, LayerMask.GetMask(layerMask)))
        {
            return true;
        }
        else
        {
            return false;    
        }
        
    }

    public static float GetHitDistanceOnYFromPlane(Vector3 startPos)
    {
        RaycastHit Hit;
        Physics.Raycast(startPos, Vector3.down, out Hit, 100);
        return Hit.distance;
    }

    public static Transform[] createObjects(int posCount, GameObject goPrefab)
    {
        Transform[] objects = new Transform[posCount];
        for (int i = 0; i < posCount; i++)
        {
            GameObject go = Instantiate(goPrefab);
            objects[i] = go.transform;
        }
        return objects;
    }
    public static Transform createObject(GameObject goPrefab)
    {
        GameObject go = Instantiate(goPrefab);
        return go.transform;
    }

    public static void getTransformsToPositions(Transform[] gOArray, Vector3[] positions)
    {
        for (int i = 0; i < gOArray.Length; i++)
        {
            gOArray[i].transform.position = positions[i];
        }
    }

    public static void addObjectsToParent(Transform parent, Transform[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetParent(parent);
        }
    }
    public static void addObjectToParent(Transform parent, Transform obj)
    {
        obj.SetParent(parent);
    }

    public static void DeleteObjectsOnParent(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    public static Transform[] getObjectsOfParent(Transform t)
    {
        Transform[] f = new Transform[t.childCount];
        for (int i = 0; i < t.childCount; i++)
        {
            f[i] = t.GetChild(i);
        }
        return f;
    }

    public static Vector3[] getPositionsOfObjects(Transform[] arr)
    {
        Vector3[] places = new Vector3[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            places[i] = arr[i].position;
        }
        return places;
    }

    public static void MoveObjectsToPositionsHold(List<Transform> objects, Vector3[] positions, float speed)
    {
        if (objects.Count != positions.Length || objects[0] == null || positions[0] == null)
        {
            return;
        }
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].position = Vector3.MoveTowards(objects[i].position, positions[i], speed * Time.deltaTime);
        }
    }

    public static void reverseArray<T>(ref T[] arr)
    {
        Array.Reverse(arr);
    }

    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return a + (b - a) * t;
    }

    public static Vector2 QuadraticCurve(Vector3 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 p0 = Lerp(a ,b ,t);
        Vector2 p1 = Lerp(b, c, t);
        return Lerp(p0, p1, t);
    }

    public static Vector3 Lerp3D(Vector3 a, Vector3 b, float t)
    {
        return a + (b - a) * t;
    }

    public static Vector3 QuadraticCurve3D(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 p0 = Lerp3D(a, b, t);
        Vector3 p1 = Lerp3D(b, c, t);
        return Lerp3D(p0, p1, t);
    }

    public static Vector3 BezierQuadraticCurve3D(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return p;
    }

    public static Vector3[] getPositionsOnYZ(directions dir, Vector3 startPos, float gapY, float gapZ, int posCount)
    {
        Vector3[] positions = new Vector3[posCount];
        if (dir == directions.UpLeft)
        {
            for (int i = 0; i < posCount; i++)
            {
                positions[i] = new Vector3(startPos.x, startPos.y + i * gapY, startPos.z - i * gapZ);
            }
        }
        else if (dir == directions.UpRight)
        {
            for (int i = 0; i < posCount; i++)
            {
                positions[i] = new Vector3(startPos.x, startPos.y + i * gapY, startPos.z + i * gapZ);
            }
        }
        else if (dir == directions.Bottomleft)
        {
            for (int i = 0; i < posCount; i++)
            {
                positions[i] = new Vector3(startPos.x, startPos.y - i * gapY, startPos.z - i * gapZ);
            }
        }
        else if (dir == directions.BottomRight)
        {
            for (int i = 0; i < posCount; i++)
            {
                positions[i] = new Vector3(startPos.x, startPos.y - i * gapY, startPos.z + i * gapZ);
            }
        }
        else if (dir == directions.Up)
        {
            for (int i = 0; i < posCount; i++)
            {
                positions[i] = new Vector3(startPos.x, startPos.y + i * gapY, startPos.z);
            }
        }
        else if (dir == directions.right)
        {
            for (int i = 0; i < posCount; i++)
            {
                positions[i] = new Vector3(startPos.x, startPos.y, startPos.z + i * gapZ);
            }
        }
        else if (dir == directions.bottom)
        {
            for (int i = 0; i < posCount; i++)
            {
                positions[i] = new Vector3(startPos.x, startPos.y - i * gapY, startPos.z);
            }
        }
        else if (dir == directions.left)
        {
            for (int i = 0; i < posCount; i++)
            {
                positions[i] = new Vector3(startPos.x, startPos.y, startPos.z - i * gapZ);
            }
        }
        else if(dir == directions.UpRightSpline)
        {
            for (int i = 0; i < posCount; i++)
            {
                positions[i] = new Vector3(startPos.x, startPos.y + i * gapY, startPos.z + i * gapZ);
            }
        }
        return positions;
    }
}
