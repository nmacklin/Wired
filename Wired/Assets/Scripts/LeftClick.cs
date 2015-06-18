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
        string clickedCubeCoordinates = placementRegister.CubeFinder(hitInfo);
        Debug.Log(hitInfo.collider);

        if (hitInfo.collider.gameObject.tag == "Destructable")
        {
            Destroy(hitInfo.collider.gameObject);
            placementRegister.integerCoordinates.Remove(clickedCubeCoordinates);
        }
    }
}
