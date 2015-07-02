using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectPlacement : MonoBehaviour {

    public GameObject staticWorld;
    public GameObject powerSource;
    public GameObject wire0;
    public GameObject wire1;
    public GameObject wire2a;
    public GameObject wire2b;
    public GameObject wire3;
    public GameObject wire4;

    PlacementRegister placementRegister;
    AdjacentObjectFinder adjacentObjectFinder;

	void Start () 
    {
        // Gets appropriate scripts.
        placementRegister = staticWorld.GetComponent<PlacementRegister>();
        adjacentObjectFinder = staticWorld.GetComponent<AdjacentObjectFinder>();
	}

    public Vector3 DeterminePlacementPosition (RaycastHit hitInfo, string clickedCubeCoordinates)
    {
        // Determines placement position by determining which side of object was clicked.
        // Placement position is cube adjacent to clicked face.
        Vector3 hitPoint = hitInfo.point;
        Vector3 hitObjectCenter = hitInfo.transform.position;

        if (hitInfo.collider.gameObject.tag.Contains("Mutable"))
        {
            hitPoint.y += 0.5f;
        }

        float greatestCoordinateDistance = 0;
        int axisOfGreatestDistance = 99;

        for (int i = 0; i < 3; i++)
        {
            float coordinateDistanceFromCenter = hitPoint[i] - hitObjectCenter[i];
            if (Mathf.Abs(coordinateDistanceFromCenter) > Mathf.Abs(greatestCoordinateDistance))
            {
                greatestCoordinateDistance = coordinateDistanceFromCenter;
                axisOfGreatestDistance = i;
            }
        }

        Vector3 placementPosition = placementRegister.CoordinatesStringToVector3(clickedCubeCoordinates);

        switch (axisOfGreatestDistance)
        {
            case 0:
                placementPosition.x += 1 * Mathf.Sign(greatestCoordinateDistance);
                break;

            case 1:
                placementPosition.y += 1 * Mathf.Sign(greatestCoordinateDistance);
                break;

            case 2:
                placementPosition.z += 1 * Mathf.Sign(greatestCoordinateDistance);
                break;

            case 99:
                print("Unable to determine placement position.");
                break;
        }

        return placementPosition;
    }

    public bool ValidPlacement(string placementPositionCoordinates)
    {
        // Checks that placement position is not already registered in object register.
        return !(placementRegister.coordinatesIDDictionary.ContainsKey(placementPositionCoordinates));
    }

    public Dictionary<GameObject, Quaternion> DeterminePlacementAngle (Dictionary<string, int> adjacentObjects)
    {
        // Takes information about adjacent objects and returns appropriate angles for wires as Quaternion.
        // Lots of esoteric math to compensate for unit circle coming around (i.e. 0 != 360).

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

        Dictionary<GameObject, Quaternion> placementAngleDictionary = new Dictionary<GameObject,Quaternion>();

        switch (adjacentObjectAngles.Count)
        {
            default:
            case 0:
                placementAngleDictionary.Add(wire0, Quaternion.Euler(0, 0, 0));
                break;
            case 1:
                placementAngleDictionary.Add(wire1, Quaternion.Euler(0, adjacentObjectAngles[0] * -1, 0));
                break;
            case 2:
                if (adjacentObjectAngles.Contains(averageAdjacentAngle - 90))
                {
                    placementAngleDictionary.Add(wire2a, Quaternion.Euler(0, averageAdjacentAngle - 90, 0));
                }
                else
                {
                    if (adjacentObjectAngles.Contains(0) && adjacentObjectAngles.Contains(270))
                    {
                        placementAngleDictionary.Add(wire2b, Quaternion.Euler(0, 90, 0)); // Compensating for 360 -> 0 difficulty.
                    }
                    else
                    {
                        placementAngleDictionary.Add(wire2b, Quaternion.Euler(0, -1 * (averageAdjacentAngle - 45), 0));
                    }
                }
                break;
            case 3:
                placementAngleDictionary.Add(wire3, Quaternion.Euler(0, adjacentObjectAngles.Sum(), 0));
                break;
            case 4:
                placementAngleDictionary.Add(wire4, Quaternion.Euler(0, 0, 0));
                break;
        }

        return placementAngleDictionary;
    }

    public Dictionary<string, int> ObjectPlacementMain (RaycastHit hitInfo, string clickedCubeCoordinates, int backpackSelection, List<object> persistentValues)
    {
        // Function called by external scripts to place object.
        // First determines placement position, then determines placement angle if object is "Mutable" (e.g. wire),
        // Then instantiates object and adds to object register.
        // Also passes on previous qualities if placed object is replacing another.

        Dictionary<string, int> adjacentObjects;
        Vector3 placementPosition;
        GameObject objectToBePlaced;
        Quaternion placementAngle;
        Dictionary<GameObject, Quaternion> placementAngleDictionary = new Dictionary<GameObject, Quaternion>();

        if (backpackSelection != 99) // Case 99 is used when replacing an object that was just destroyed to form new connections.
        {
            placementPosition = DeterminePlacementPosition(hitInfo, clickedCubeCoordinates);
        }
        else
        {
            placementPosition = placementRegister.CoordinatesStringToVector3(clickedCubeCoordinates);
        }

        string placementPositionCoordinates = placementRegister.CoordinatesVector3ToString(placementPosition); 

        if (ValidPlacement(placementPositionCoordinates))
        {
            adjacentObjects = adjacentObjectFinder.AdjacentObjectFinderMain(placementPositionCoordinates);

            switch (backpackSelection)
            {
                default:
                case 0:
                    placementAngleDictionary = DeterminePlacementAngle(adjacentObjects);
                    objectToBePlaced = placementAngleDictionary.Keys.ElementAt(0);
                    placementAngle = placementAngleDictionary.Values.ElementAt(0);
                    break;

                case 1:
                    objectToBePlaced = powerSource;
                    placementAngle = Quaternion.identity;
                    break;

                case 99:
                    placementPosition = placementRegister.CoordinatesStringToVector3(clickedCubeCoordinates);
                    placementAngleDictionary = DeterminePlacementAngle(adjacentObjects);
                    objectToBePlaced = placementAngleDictionary.Keys.ElementAt(0);
                    placementAngle = placementAngleDictionary.Values.ElementAt(0);
                    break;
            }

            GameObject objectPlaced = Instantiate(objectToBePlaced, placementPosition, placementAngle) as GameObject;
            print("Adding following to register: " + placementPosition);
            placementRegister.AddToObjectRegister(placementPositionCoordinates, objectPlaced);
            print(placementPositionCoordinates + " added to object register!");

            Conductor placedConductor = objectPlaced.GetComponent<Conductor>();
            if (persistentValues != null && placedConductor != null)
            {
                placedConductor.importedValues = true;
                placedConductor.containsSignal = (bool)persistentValues[0];
                placedConductor.signalStrength = (int)persistentValues[1];
                placedConductor.emissionIntensity = (float)persistentValues[2];
                placedConductor.UpdateEmission();
                try
                {
                    placedConductor.SetSignalOrigin((string)persistentValues[3]);
                }
                catch (KeyNotFoundException)
                {
                    // Catches exception thrown when attempting to set origin to object that was just destroyed.
                    placedConductor.SetSignalOrigin(null);
                }
            }
        }
        else 
        {
            adjacentObjects = null;
        }

        return adjacentObjects;
    }
}
