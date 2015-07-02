using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RightClick : MonoBehaviour {
    public GameObject staticWorld;

    string clickedCubeCoordinates;
    float timeAtLastPlacement;

    PlacementRegister placementRegister;
    ObjectPlacement objectPlacement;
    PlayerInventory playerInventory;

    void Awake()
    {
        // Gets appropriate scripts.
        placementRegister = staticWorld.GetComponent<PlacementRegister>();
        objectPlacement = gameObject.GetComponent<ObjectPlacement>();
        playerInventory = gameObject.GetComponent<PlayerInventory>();
    }

    public void RightClickHandler(RaycastHit hitInfo)
    {
        // Creates string of clicked integer coordinates (see PlacementRegister.cs for method).
        clickedCubeCoordinates = placementRegister.CoordinatesVector3ToString(hitInfo.collider.gameObject.transform.position);

        // Instantiates GameObject at clicked coordinates and registers placement in object register. 
        // Then destroys and re-instantiates adjacent wires to accomodate new connections.
        if (Time.time - timeAtLastPlacement > 0.2f)
        {
            // Temporary backpack selection code
            int backpackSelection = playerInventory.GetBackpackSelection();

            Dictionary<string, int> adjacentObjects = objectPlacement.ObjectPlacementMain(hitInfo, clickedCubeCoordinates, backpackSelection, null);

            if (adjacentObjects != null)
            {
                foreach (string adjacentObject in adjacentObjects.Keys)
                {
                    GameObject adjacentObjectReference = placementRegister.ObjectLookupByCoordinateString(adjacentObject);
                    if (adjacentObjectReference.tag.Contains("Mutable"))
                    {
                        Conductor adjacentConductor = adjacentObjectReference.GetComponent<Conductor>();
                        List<object> persistentValues = new List<object>();
                        if (adjacentConductor != null)
                        {
                            print("VALUES PERSISTING!!!!");
                            persistentValues.Add(adjacentConductor.containsSignal);
                            persistentValues.Add(adjacentConductor.signalStrength);
                            persistentValues.Add(adjacentConductor.emissionIntensity);
                            persistentValues.Add(adjacentConductor.GetSignalOriginCoordinates());
                            print("SIGNAL ORIGIN COORDINATES ADDED: " + adjacentConductor.GetSignalOriginCoordinates());
                        }
                        Destroy(placementRegister.ObjectLookupByCoordinateString(adjacentObject));
                        placementRegister.RemoveFromObjectRegister(adjacentObject);
                        objectPlacement.ObjectPlacementMain(hitInfo, adjacentObject, 99, persistentValues);
                    }

                }
                timeAtLastPlacement = Time.time;
            }
        }
        else
        {
            print("Invalid placement at " + clickedCubeCoordinates);
        }
    }
}
