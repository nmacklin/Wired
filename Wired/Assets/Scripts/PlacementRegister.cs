using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlacementRegister : MonoBehaviour {

    public Dictionary<string, int> integerCoordinates;

    void Awake()
    {
        integerCoordinates = new Dictionary<string, int>();
    }

    public string CubeFinder(RaycastHit hitInfo)
    {
        // Takes RaycastHit and converts Vector3 coordinates to string of format "sxxx syyy szzz" where s is + or -
        string clickedCubeCoordinates = "";
        for (int i = 0; i < 3; i++)
        {
            float floatCoordinate = hitInfo.point[i];
            int coordinate = Mathf.RoundToInt(floatCoordinate);
            if (coordinate >= 0)
            {
                clickedCubeCoordinates += "+" + coordinate.ToString("D3");
            }
            else
            {
                clickedCubeCoordinates += coordinate.ToString("D3");
            }
            if (i < 2)
            {
                clickedCubeCoordinates += " ";
            }
        }
        return clickedCubeCoordinates;
    }

    public Vector3 CoordinatesStringToVector(string clickedCubeCoordinates)
    {
        // Takes coordinate string and converts it to Vector3
        int xCoordinate = Int32.Parse(clickedCubeCoordinates.Substring(0, 4));
        int yCoordinate = Int32.Parse(clickedCubeCoordinates.Substring(5, 4));
        int zCoordinate = Int32.Parse(clickedCubeCoordinates.Substring(10, 4));

        Vector3 vector3Coordinates = new Vector3(xCoordinate, yCoordinate, zCoordinate);

        return vector3Coordinates;
    }
}
