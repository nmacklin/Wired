using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeftClick : MonoBehaviour {

    public GameObject staticWorld;
    PlacementRegister placementRegister;
    AdjacentObjectFinder adjacentObjectFinder;
    ObjectPlacement objectPlacement;
    SignalRegister signalRegister;

    float timeAtLastRemoval;
    
    void Awake()
    {
        placementRegister = staticWorld.GetComponent<PlacementRegister>();
        adjacentObjectFinder = staticWorld.GetComponent<AdjacentObjectFinder>();
        objectPlacement = this.gameObject.GetComponent<ObjectPlacement>();
        signalRegister = staticWorld.GetComponent<SignalRegister>();
    }

    public void LeftClickHandler(RaycastHit hitInfo)
    {
        // Checks if clicked collider is destructable, then destroys object and removes from Placement Register.
        // Limited to 5 instances of destructions per second.
        string clickedCubeCoordinates = placementRegister.CoordinatesVector3ToString(hitInfo.point);

        if (hitInfo.collider.gameObject.tag.Contains("Destructable") && (Time.time - timeAtLastRemoval > 0.2f))
        {
            print("Destroying " + clickedCubeCoordinates);
            Destroy(hitInfo.collider.gameObject);
            signalRegister.signalRegister.Remove(clickedCubeCoordinates);
            placementRegister.RemoveFromObjectRegister(clickedCubeCoordinates);

            // Destroys and replaces adjacent objects if Mutable for connections formation.
            // Note also passes persistent values onto new object.
            Dictionary<string, int> adjacentObjects = adjacentObjectFinder.AdjacentObjectFinderMain(clickedCubeCoordinates);
            foreach (string adjacentObject in adjacentObjects.Keys)
            {
                GameObject adjacentObjectReference = placementRegister.ObjectLookupByCoordinateString(adjacentObject);
                if (adjacentObjectReference.tag.Contains("Mutable"))
                {
                    Conductor adjacentConductor = adjacentObjectReference.GetComponent<Conductor>();
                    List<object> persistentValues = new List<object>();
                    if (adjacentConductor != null)
                    {
                        persistentValues.Add(adjacentConductor.containsSignal);
                        persistentValues.Add(adjacentConductor.signalStrength);
                        persistentValues.Add(adjacentConductor.emissionIntensity);
                        persistentValues.Add(adjacentConductor.GetSignalOriginCoordinates());
                    }
                    Destroy(placementRegister.idReferenceDictionary[placementRegister.coordinatesIDDictionary[adjacentObject]]);
                    placementRegister.RemoveFromObjectRegister(adjacentObject);
                    objectPlacement.ObjectPlacementMain(hitInfo, adjacentObject, 99, persistentValues);
                }
            }

            timeAtLastRemoval = Time.time;
        }
    }
}
