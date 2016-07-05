using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WirePlacement : MonoBehaviour {
    public GameObject terrain;
    public GameObject wire0;
    public GameObject wire1;
    public GameObject wire2a;
    public GameObject wire2b;
    public GameObject wire3;
    public GameObject wire4;

    PlacementRegister placementRegister;
    AdjacentObjectFinder adjacentObjectFinder;

    void Awake()
    {
        // Gets appropriate scripts.
        placementRegister = terrain.GetComponent<PlacementRegister>();
        adjacentObjectFinder = gameObject.GetComponent<AdjacentObjectFinder>();
    }

    public void ObjectPlacement(Vector3 placementPosition, Dictionary<string, int> adjacentObjects)
    {
        // Takes information about adjacent objects and instantiates appropriate model based on connections necessary.
        // Lots of esoteric math to compensate for unit circle coming around (i.e. 0 != 360).
        // Then adds object entry to dictionary in PlacementRegister.cs.
        Object objectPlaced;

        List<int> adjacentObjectAngles = new List<int>();
        foreach (int angle in adjacentObjects.Values)
        {
            adjacentObjectAngles.Add(angle);
        }

        int averageAdjacentAngle = 0; // NOT zero by default - can't be null and compiler won't shut up without declaration value.
        try
        {
            averageAdjacentAngle = adjacentObjectAngles.Sum() / adjacentObjectAngles.Count();
        }
        catch (System.DivideByZeroException) { }

        switch (adjacentObjectAngles.Count)
        {
            default:
            case 0:
                objectPlaced = Instantiate(wire0, placementPosition, Quaternion.Euler(0, 0, 0));
                break;
            case 1:
                objectPlaced = Instantiate(wire1, placementPosition, Quaternion.Euler(90, 90, adjacentObjectAngles[0]));
                break;
            case 2:
                if (adjacentObjectAngles.Contains(averageAdjacentAngle - 90))
                {
                    objectPlaced = Instantiate(wire2a, placementPosition, Quaternion.Euler(90, 0, averageAdjacentAngle));
                }
                else
                {
                    if (adjacentObjectAngles.Contains(0) && adjacentObjectAngles.Contains(270))
                    {
                        objectPlaced = Instantiate(wire2b, placementPosition, Quaternion.Euler(90, 90, 270)); // Compensating for 360 -> 0 difficulty.
                    }
                    else
                    {
                        objectPlaced = Instantiate(wire2b, placementPosition, Quaternion.Euler(90, 90, averageAdjacentAngle - 45));
                    }
                }
                break;
            case 3:
                objectPlaced = Instantiate(wire3, placementPosition, Quaternion.Euler(0, adjacentObjectAngles.Sum(), 180));
                break;
            case 4:
                objectPlaced = Instantiate(wire4, placementPosition, Quaternion.Euler(0, 0, 0));
                break;
        }

        GameObject gameObjectPlaced = objectPlaced as GameObject;
        placementRegister.AddToObjectRegister(placementRegister.CoordinatesVector3ToString(placementPosition), gameObjectPlaced);
    }

    public Dictionary<string, int> WirePlacementMain(Vector3 placementPosition)
    {
        string placementPositionString = placementRegister.CoordinatesVector3ToString(placementPosition);
        Dictionary<string, int> adjacentObjects = adjacentObjectFinder.AdjacentObjectFinderMain(placementPositionString);
        ObjectPlacement(placementPosition, adjacentObjects);
        return adjacentObjects;
    }
}