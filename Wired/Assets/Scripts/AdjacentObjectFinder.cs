using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdjacentObjectFinder : MonoBehaviour
{
    PlacementRegister placementRegister;

    void Awake()
    {
        placementRegister = gameObject.GetComponent<PlacementRegister>();
    }

    public Dictionary<string, int> AdjacentObjectFinderMain(string targetPositionString)
    {
        // Checks orthogonally adjacent coordinates for objects and adds appropriate Vector3 location and angle to Dictionary.
        // Only checks in same plane currently, plan to account for above and below in future.
        Dictionary<string, int> adjacentObjects = new Dictionary<string, int>();

        Vector3 targetPosition = placementRegister.CoordinatesStringToVector3(targetPositionString);

        Vector3 rightCheck = targetPosition;
        rightCheck.x += 1;
        if (placementRegister.coordinatesIDDictionary.ContainsKey(placementRegister.CoordinatesVector3ToString(rightCheck)))
        {
            adjacentObjects.Add(placementRegister.CoordinatesVector3ToString(rightCheck), 0);
        }

        Vector3 forwardCheck = targetPosition;
        forwardCheck.z += 1;
        if (placementRegister.coordinatesIDDictionary.ContainsKey(placementRegister.CoordinatesVector3ToString(forwardCheck)))
        {
            adjacentObjects.Add(placementRegister.CoordinatesVector3ToString(forwardCheck), 90);
        }

        Vector3 leftCheck = targetPosition;
        leftCheck.x -= 1;
        if (placementRegister.coordinatesIDDictionary.ContainsKey(placementRegister.CoordinatesVector3ToString(leftCheck)))
        {
            adjacentObjects.Add(placementRegister.CoordinatesVector3ToString(leftCheck), 180);
        }

        Vector3 backCheck = targetPosition;
        backCheck.z -= 1;
        if (placementRegister.coordinatesIDDictionary.ContainsKey(placementRegister.CoordinatesVector3ToString(backCheck)))
        {
            adjacentObjects.Add(placementRegister.CoordinatesVector3ToString(backCheck), 270);
        }

        return adjacentObjects;
    }
}
