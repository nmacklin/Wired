using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RightClick : MonoBehaviour {

    public GameObject terrain;
    PlacementRegister placementRegister;
    public GameObject wire;
    string clickedCubeCoordinates;

    void Awake()
    {
        placementRegister = terrain.GetComponent<PlacementRegister>();
    }

    public bool ValidPlacement()
    {
        return !(placementRegister.integerCoordinates.ContainsKey(clickedCubeCoordinates));
    }

    public void RightClickHandler(RaycastHit hitInfo)
    {
        clickedCubeCoordinates = placementRegister.CubeFinder(hitInfo);

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
