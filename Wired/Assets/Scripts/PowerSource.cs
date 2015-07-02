using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerSource : MonoBehaviour {

    GameObject staticWorld;

    PlacementRegister placementRegister;
    SignalRegister signalRegister;

    bool containsSignal;
    string powerSourceLocation;

	void Start () {
        staticWorld = GameObject.Find("Static World");
        placementRegister = staticWorld.GetComponent<PlacementRegister>();
        signalRegister = staticWorld.GetComponent<SignalRegister>();
        powerSourceLocation = placementRegister.CoordinatesVector3ToString(this.gameObject.transform.position);

        containsSignal = true;
	}

    void OnTriggerEnter(Collider other)
    {
        // On trigger collision with power source, confers charge to conductor.
        if (other.gameObject.tag.Contains("Conductive"))
        {
            Conductor conductor = other.gameObject.GetComponent<Conductor>();
            conductor.SetSignalOrigin(powerSourceLocation);
            conductor.containsSignal = true;
            conductor.signalStrength = 10;
            signalRegister.signalRegister.Add(placementRegister.CoordinatesVector3ToString(other.gameObject.transform.position));
        }
    }
}
