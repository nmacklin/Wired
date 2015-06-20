using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeftClick : MonoBehaviour {

    public GameObject terrain;
    PlacementRegister placementRegister;
    AdjacentObjectFinder adjacentObjectFinder;
    WirePlacement wirePlacement;
    
    void Awake()
    {
        placementRegister = terrain.GetComponent<PlacementRegister>();
        adjacentObjectFinder = gameObject.GetComponent<AdjacentObjectFinder>();
        wirePlacement = gameObject.GetComponent<WirePlacement>();
    }

    public void LeftClickHandler(RaycastHit hitInfo)
    {
        // Checks if clicked collider is destructable, then destroys object and removes from Placement Register.
        string clickedCubeCoordinates = placementRegister.CoordinatesVector3ToString(hitInfo.point);
        Debug.Log(hitInfo.collider);

        if (hitInfo.collider.gameObject.tag == "Destructable")
        {
            Destroy(hitInfo.collider.gameObject);
            placementRegister.RemoveFromObjectRegister(clickedCubeCoordinates);
            Dictionary<Vector3, int> adjacentObjects = adjacentObjectFinder.AdjacentObjectFinderMain(placementRegister.CoordinatesStringToVector3(clickedCubeCoordinates));
            foreach (Vector3 adjacentObject in adjacentObjects.Keys)
            {
                string objectCoordinateString = placementRegister.CoordinatesVector3ToString(adjacentObject);

                Destroy(placementRegister.placedObjectsDictionary[placementRegister.coordinatesIDDictionary[objectCoordinateString]]);
                placementRegister.RemoveFromObjectRegister(objectCoordinateString);
                wirePlacement.WirePlacementMain(adjacentObject);
            }
        }
    }
}
