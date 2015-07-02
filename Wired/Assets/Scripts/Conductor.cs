using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Conductor : MonoBehaviour {
    // Script for wires which conduct charge but do not generate it.

    GameObject staticWorld;
    Renderer renderer1;
    PlacementRegister placementRegister;

    private string signalOriginCoordinates;
    private GameObject signalOriginObject;
    private Conductor signalOriginConductor;
    private PowerSource signalOriginPowerSource;

    public float emissionIntensity;
    public bool containsSignal;
    public int signalStrength;
    int maxSignalStrength;
    int lastSignalStrength;
    public bool importedValues;

    void Awake () 
    {
        staticWorld = GameObject.Find("Static World");

        renderer1 = gameObject.GetComponent<Renderer>();
        placementRegister = staticWorld.GetComponent<PlacementRegister>();

        maxSignalStrength = 10;

        if (!importedValues)
        {
            // Sets default values of various parameters if being instantiated de novo (i.e. without imported values).
            // Skips if object being destroyed and replaced with new model to form adjacent connections.
            signalOriginObject = null;
            signalOriginConductor = null;
            signalOriginPowerSource = null;

            containsSignal = false;
            emissionIntensity = 0f;
            signalStrength = 0;
        }
    }

    void Update()
    {
        // Updates emission property of default shader to reflect increasing temperature of object as it passes charge.
        // Does not update if at equilibrium (i.e. reached emission intensity of 1).

        bool updateEmission = false;

        if (importedValues)
        {
            updateEmission = true;
        }

        // Used to prevent unrealistic drop in emission upon abruptly losing charge due to emission adjustment.
        if (lastSignalStrength != signalStrength)
        {
            lastSignalStrength = signalStrength;
        }

        // Either adjusts emission or resets conductor based on signal and emission status.
        if (containsSignal && emissionIntensity < 1)
        {
            emissionIntensity += 0.05f;
            updateEmission = true;
        }
        else
        {
            if (emissionIntensity > 0 && !containsSignal)
            {
                emissionIntensity -= 0.005f;
                updateEmission = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(signalOriginCoordinates) && !containsSignal)
                {
                    // Resets conductor if completely cooled and doesn't contain signal.
                    SetSignalOrigin(null);
                    signalStrength = 0;
                }
            }
        }

        if (updateEmission)
        {
            UpdateEmission();
        }
    }

    public void UpdateEmission()
    {
        // Updates emission based on signalStrength and emissionIntensity.
        int adjustedSignalStrength;
        if (signalStrength == 0)
        {
            adjustedSignalStrength = lastSignalStrength;
        }
        else
        {
            adjustedSignalStrength = signalStrength;
        }
        float adjustedEmissionIntensity = emissionIntensity * ((float)adjustedSignalStrength / (float)maxSignalStrength);
        renderer1.material.EnableKeyword("_EMISSION");
        renderer1.material.SetColor("_EmissionColor", new Color(adjustedEmissionIntensity, adjustedEmissionIntensity, adjustedEmissionIntensity));
    }

    public void SetSignalOrigin (string signalOriginSetString)
    {
        // Sets signal origin coordinates and gets either PowerSource or Conductor script as appropriate.
        signalOriginCoordinates = signalOriginSetString;

        if (!string.IsNullOrEmpty(signalOriginCoordinates))
        {
            signalOriginObject = placementRegister.ObjectLookupByCoordinateString(signalOriginSetString);
            if (signalOriginObject.tag.Contains("Power Source"))
            {
                signalOriginPowerSource = signalOriginObject.GetComponent<PowerSource>();
            }
            else
            {
                signalOriginConductor = signalOriginObject.GetComponent<Conductor>();
            }
        }
    }

    public string GetSignalOriginCoordinates ()
    {
        return signalOriginCoordinates;
    }

    public bool GetSignalOriginCharge ()
    {
        if (signalOriginConductor != null)
        {
            return signalOriginConductor.containsSignal;
        }
        else
        {
            if (signalOriginPowerSource != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
