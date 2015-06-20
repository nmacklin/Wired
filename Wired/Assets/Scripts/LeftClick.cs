using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeftClick : MonoBehaviour {

    public GameObject terrain;
    PlacementRegister placementRegister;
    
    void Awake()
    {
        placementRegister = terrain.GetComponent<PlacementRegister>();
    }

    public void LeftClickHandler(RaycastHit hitInfo)
    {
        // Checks if clicked collider is destructable, then destroys object and removes from Placement Register.
        string clickedCubeCoordinates = placementRegister.CoordinatesVector3ToString(hitInfo.point);
        Debug.Log(hitInfo.collider);

        if (hitInfo.collider.gameObject.tag == "Destructable")
        {
            Destroy(hitInfo.collider.gameObject);
            placementRegister.integerCoordinates.Remove(clickedCubeCoordinates);
        }
    }
}
