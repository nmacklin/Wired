using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RightClick : MonoBehaviour {

    public GameObject terrain;
    public GameObject wire;

    PlacementRegister placementRegister;
    string clickedCubeCoordinates;

    void Awake()
    {
        // Gets PlacementRegister.cs script from terrain GameObject.
        placementRegister = terrain.GetComponent<PlacementRegister>();
    }

    public bool ValidPlacement()
    {
        // Checks to see if object already registered in PlacementRegister at clicked integer coordinates.
        return !(placementRegister.integerCoordinates.ContainsKey(clickedCubeCoordinates));
    }

    public void RightClickHandler(RaycastHit hitInfo)
    {
        // Creates string of clicked integer coordinates (see PlacementRegister.cs for method).
        clickedCubeCoordinates = placementRegister.CubeFinder(hitInfo);

        // Instantiates GameObject at clicked coordinates and registers placement in PlacementRegister. 
        if (ValidPlacement())
        {
            Vector3 placementPosition = placementRegister.CoordinatesStringToVector(clickedCubeCoordinates);

            Object objectPlaced = Instantiate(wire, placementPosition, Quaternion.Euler(90, 0, 0));

            placementRegister.integerCoordinates.Add(clickedCubeCoordinates, objectPlaced.GetInstanceID());

            print("Wire placed!");
            Debug.Log(clickedCubeCoordinates);
            Debug.Log(objectPlaced.GetInstanceID().ToString());
        }
        else
        {
            print("Invalid placement at " + clickedCubeCoordinates);
        }
    }
}
