using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RightClick : MonoBehaviour {

    public GameObject terrain;
    public GameObject wire0;
    public GameObject wire1;
    public GameObject wire2a;
    public GameObject wire2b;
    public GameObject wire3;
    public GameObject wire4;

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

    public List<int> AdjacentObjectFinder(Vector3 placementPosition)
    {
        // Checks orthogonally adjacent coordinates for objects and adds appropriate angle to integer list.
        List<int> adjacentObjectAngles = new List<int>();

        Vector3 rightCheck = placementPosition;
        rightCheck.x += 1;
        if (placementRegister.integerCoordinates.ContainsKey(placementRegister.CoordinatesVector3ToString(rightCheck)))
        {
            adjacentObjectAngles.Add(0);
        }

        Vector3 forwardCheck = placementPosition;
        forwardCheck.z += 1;
        if (placementRegister.integerCoordinates.ContainsKey(placementRegister.CoordinatesVector3ToString(forwardCheck)))
        {
            adjacentObjectAngles.Add(90);
        }

        Vector3 leftCheck = placementPosition;
        leftCheck.x -= 1;
        if (placementRegister.integerCoordinates.ContainsKey(placementRegister.CoordinatesVector3ToString(leftCheck)))
        {
            adjacentObjectAngles.Add(180);
        }

        Vector3 backCheck = placementPosition;
        backCheck.z -= 1;
        if (placementRegister.integerCoordinates.ContainsKey(placementRegister.CoordinatesVector3ToString(backCheck)))
        {
            adjacentObjectAngles.Add(270);
        }

        return adjacentObjectAngles;
    }

    public void ObjectPlacement(Vector3 placementPosition, List<int> adjacentObjectAngles)
    {
        // Takes information about adjacent objects and instantiates appropriate model based on connections necessary.
        // Lots of esoteric math to compensate for unit circle coming around (i.e. 0 != 360).
        // Then adds object entry to dictionary in PlacementRegister.cs.
        Object objectPlaced;

        int averageAdjacentAngle = 0; // NOT zero by default - can't be null and compiler won't shut up without declaration value.
        try
        {
            averageAdjacentAngle = adjacentObjectAngles.Sum() / adjacentObjectAngles.Count();
        }
        catch (System.DivideByZeroException) {}

        switch (adjacentObjectAngles.Count)
        {
            default:
            case 0:
                objectPlaced = Instantiate(wire0, placementPosition, Quaternion.Euler(0, 0, 0));
                break;
            case 1:
                objectPlaced = Instantiate(wire1, placementPosition, Quaternion.Euler(90, 90, adjacentObjectAngles[0]));
                break;
            case 2:
                if (adjacentObjectAngles.Contains(averageAdjacentAngle - 90))
                {
                    objectPlaced = Instantiate(wire2a, placementPosition, Quaternion.Euler(90, 0, averageAdjacentAngle));
                }
                else
                {
                    if (adjacentObjectAngles.Contains(0) && adjacentObjectAngles.Contains(270))
                    {
                        objectPlaced = Instantiate(wire2b, placementPosition, Quaternion.Euler(90, 90, 270)); // Compensating for 360 -> 0 difficulty.
                    }
                    else
                    {
                        objectPlaced = Instantiate(wire2b, placementPosition, Quaternion.Euler(90, 90, averageAdjacentAngle - 45));
                    }
                }
                break;
            case 3:
                objectPlaced = Instantiate(wire3, placementPosition, Quaternion.Euler(0, adjacentObjectAngles.Sum(), 180));
                break;
            case 4:
                objectPlaced = Instantiate(wire4, placementPosition, Quaternion.Euler(0, 0, 0));
                break;
                
        }

        placementRegister.integerCoordinates.Add(clickedCubeCoordinates, objectPlaced.GetInstanceID());

        print("Wire placed!");
        Debug.Log(clickedCubeCoordinates);
        Debug.Log(objectPlaced.GetInstanceID().ToString());
    }

    public void RightClickHandler(RaycastHit hitInfo)
    {
        // Creates string of clicked integer coordinates (see PlacementRegister.cs for method).
        clickedCubeCoordinates = placementRegister.CoordinatesVector3ToString(hitInfo.point);

        // Instantiates GameObject at clicked coordinates and registers placement in PlacementRegister. 
        if (ValidPlacement())
        {
            Vector3 placementPosition = placementRegister.CoordinatesStringToVector3(clickedCubeCoordinates);
            List<int> adjacentObjects = AdjacentObjectFinder(placementPosition);
            ObjectPlacement(placementPosition, adjacentObjects);
        }
        else
        {
            print("Invalid placement at " + clickedCubeCoordinates);
        }
    }
}
