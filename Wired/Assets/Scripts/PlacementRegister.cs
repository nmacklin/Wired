﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlacementRegister : MonoBehaviour {

    public Dictionary<string, int> coordinatesIDDictionary;
    public Dictionary<int, GameObject> idReferenceDictionary;

    void Awake()
    {
        // Creates two dictionaries to register GameObject placements in playspace. 
        // The first has Keys of string of integer coordinates generated by CubeFinder and Values is integer representing object's unique GameID (see Unity Object.GetInstanceID).
        // The second has Keys of those GameIDs (int) and Values of GameObject references.
        // These two dictionaries comprise the "Object Register".
        coordinatesIDDictionary = new Dictionary<string, int>();
        idReferenceDictionary = new Dictionary<int, GameObject>();
    }

    public void AddToObjectRegister(string coordinateString, GameObject objectReference)
    {
        // Adds coordinate string, ObjectID to coordinatesIDDictionary and ObjectID, Object reference to idReferenceDictionary.
        int objectID = objectReference.GetInstanceID();

        coordinatesIDDictionary.Add(coordinateString, objectID);
        idReferenceDictionary.Add(objectID, objectReference);
    }

    public void RemoveFromObjectRegister(string coordinateString)
    {
        // Counterpart to above function.
        int objectID = coordinatesIDDictionary[coordinateString];

        coordinatesIDDictionary.Remove(coordinateString);
        idReferenceDictionary.Remove(objectID);
    }

    public GameObject ObjectLookupByCoordinateString(string coordinatesString)
    {
        // Gets object reference by coordinate string.
        int objectID = coordinatesIDDictionary[coordinatesString];
        GameObject objectRetrieved = idReferenceDictionary[objectID];
        return objectRetrieved;
    }

    public string CoordinatesVector3ToString(Vector3 vector3Coordinates)
    {
        // Converts Vector3 coordinates to string of format "+xxx +yyy +zzz".
        // This string is used as key in coordinatesIDDictionary dictionary. 
        string stringCoordinates = "";
        for (int i = 0; i < 3; i++)
        {
            float floatCoordinate = vector3Coordinates[i];
            int coordinate = Mathf.RoundToInt(floatCoordinate);
            
            if (coordinate >= 0)
            {
                stringCoordinates += "+" + coordinate.ToString("D3");
            }
            else
            {
                stringCoordinates += coordinate.ToString("D3");
            }
            if (i < 2)
            {
                stringCoordinates += " ";
            }
        }
        return stringCoordinates;
    }

    public Vector3 CoordinatesStringToVector3(string stringCoordinates)
    {
        // Takes coordinate string and converts it to Vector3.
        int xCoordinate = Int32.Parse(stringCoordinates.Substring(0, 4));
        int yCoordinate = Int32.Parse(stringCoordinates.Substring(5, 4));
        int zCoordinate = Int32.Parse(stringCoordinates.Substring(10, 4));

        Vector3 vector3Coordinates = new Vector3(xCoordinate, yCoordinate, zCoordinate);

        return vector3Coordinates;
    }
}
