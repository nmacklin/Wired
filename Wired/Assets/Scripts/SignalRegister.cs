using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SignalRegister : MonoBehaviour {

    AdjacentObjectFinder adjacentObjectFinder;
    PlacementRegister placementRegister;
    
    // List of string coordinates to store current locations of signals within circuits.
    public List<string> signalRegister;

    void Awake()
    {
        adjacentObjectFinder = gameObject.GetComponent<AdjacentObjectFinder>();
        placementRegister = gameObject.GetComponent<PlacementRegister>();

        signalRegister = new List<string>();
    }

    void Update()
    {
        List<string> tempSignalRegister = new List<string>();

        foreach (string signal in signalRegister)
        {
            GameObject currentObjectReference = null;
            Conductor currentConductor = null;

            // Tries to get object reference and associated conductor for each charge in signal register.
            // Throws exception if object was destroyed that frame.
            currentObjectReference = placementRegister.ObjectLookupByCoordinateString(signal);
            currentConductor = currentObjectReference.GetComponent<Conductor>();

            if (currentObjectReference != null)
            {
                if (!(currentConductor.GetSignalOriginCharge()))
                {
                    currentConductor.containsSignal = false;
                }
                else
                {
                    if (!tempSignalRegister.Contains(signal))
                    {
                        tempSignalRegister.Add(signal);
                    }
                }

                if (currentConductor.signalStrength > 1)
                {
                    Dictionary<string, int> adjacentObjects = adjacentObjectFinder.AdjacentObjectFinderMain(signal);

                    foreach (string adjacentObject in adjacentObjects.Keys)
                    {
                        GameObject adjacentObjectReference = placementRegister.ObjectLookupByCoordinateString(adjacentObject);

                        if (
                            adjacentObjectReference.tag.Contains("Conductive")
                            && currentConductor.GetSignalOriginCoordinates() != adjacentObject
                            && !(tempSignalRegister.Contains(adjacentObject))
                            )
                        {
                            Conductor adjacentConductor = adjacentObjectReference.GetComponent<Conductor>();
                            adjacentConductor.containsSignal = true;
                            adjacentConductor.signalStrength = currentConductor.signalStrength - 1;
                            adjacentConductor.SetSignalOrigin(signal);
                            tempSignalRegister.Add(adjacentObject);
                        }
                    }
                } 
            }
        }

        signalRegister = new List<string>(tempSignalRegister);
    }

    public void PulseSignal (string pulsedObjectCoordinates, GameObject pulsedObjectReference)
    {
        // Pulses current conductor with charge by pressing Left Control.
        Conductor pulsedObjectConductor = pulsedObjectReference.GetComponent<Conductor>();

        if (!(pulsedObjectConductor.containsSignal))
        {
            pulsedObjectConductor.containsSignal = true;

            pulsedObjectConductor.signalStrength = 10;

            signalRegister.Add(pulsedObjectCoordinates);

            print(signalRegister.Count);
        }
    }
}
