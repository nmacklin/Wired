using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RightClick : MonoBehaviour {
    public GameObject terrain;

    string clickedCubeCoordinates;
    PlacementRegister placementRegister;
    WirePlacement wirePlacement;

    void Awake()
    {
        // Gets PlacementRegister.cs script from terrain GameObject and wirePlacement.cs script.
        placementRegister = terrain.GetComponent<PlacementRegister>();
        wirePlacement = gameObject.GetComponent<WirePlacement>();
    }

    public bool ValidPlacement()
    {
        // Checks to see if object already registered in PlacementRegister at clicked integer coordinates.
        return !(placementRegister.coordinatesIDDictionary.ContainsKey(clickedCubeCoordinates));
    }

    public void RightClickHandler(RaycastHit hitInfo)
    {
        // Creates string of clicked integer coordinates (see PlacementRegister.cs for method).
        clickedCubeCoordinates = placementRegister.CoordinatesVector3ToString(hitInfo.point);

        // Instantiates GameObject at clicked coordinates and registers placement in PlacementRegister. 
        // Then destroys and re-instantiates adjacent wires to accomodate new connections.
        if (ValidPlacement())
        {
            Vector3 placementPosition = placementRegister.CoordinatesStringToVector3(clickedCubeCoordinates);
            Dictionary<Vector3, int> adjacentObjects = wirePlacement.WirePlacementMain(placementPosition);
            foreach (Vector3 adjacentObject in adjacentObjects.Keys)
            {
                string objectCoordinateString = placementRegister.CoordinatesVector3ToString(adjacentObject);

                Destroy(placementRegister.placedObjectsDictionary[placementRegister.coordinatesIDDictionary[objectCoordinateString]]);
                placementRegister.RemoveFromObjectRegister(objectCoordinateString);
                wirePlacement.WirePlacementMain(adjacentObject);
            }
        }
        else
        {
            print("Invalid placement at " + clickedCubeCoordinates);
        }
    }
}
