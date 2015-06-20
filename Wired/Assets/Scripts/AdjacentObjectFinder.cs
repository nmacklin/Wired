using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdjacentObjectFinder : MonoBehaviour
{
    public GameObject terrain;

    PlacementRegister placementRegister;

    void Awake()
    {
        placementRegister = terrain.GetComponent<PlacementRegister>();
    }

    public Dictionary<Vector3, int> AdjacentObjectFinderMain(Vector3 placementPosition)
    {
        // Checks orthogonally adjacent coordinates for objects and adds appropriate angle to integer list.
        Dictionary<Vector3, int> adjacentObjects = new Dictionary<Vector3, int>();

        Vector3 rightCheck = placementPosition;
        rightCheck.x += 1;
        if (placementRegister.coordinatesIDDictionary.ContainsKey(placementRegister.CoordinatesVector3ToString(rightCheck)))
        {
            adjacentObjects.Add(rightCheck, 0);
        }

        Vector3 forwardCheck = placementPosition;
        forwardCheck.z += 1;
        if (placementRegister.coordinatesIDDictionary.ContainsKey(placementRegister.CoordinatesVector3ToString(forwardCheck)))
        {
            adjacentObjects.Add(forwardCheck, 90);
        }

        Vector3 leftCheck = placementPosition;
        leftCheck.x -= 1;
        if (placementRegister.coordinatesIDDictionary.ContainsKey(placementRegister.CoordinatesVector3ToString(leftCheck)))
        {
            adjacentObjects.Add(leftCheck, 180);
        }

        Vector3 backCheck = placementPosition;
        backCheck.z -= 1;
        if (placementRegister.coordinatesIDDictionary.ContainsKey(placementRegister.CoordinatesVector3ToString(backCheck)))
        {
            adjacentObjects.Add(backCheck, 270);
        }

        return adjacentObjects;
    }
}
