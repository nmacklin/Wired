using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;
    public Transform camera1;
    public GameObject cubeOutline;
    public GameObject staticWorld;

    Rigidbody rb;
    RightClick rightClick;
    LeftClick leftClick;
    GameObject cubeOutlineReference;
    PlacementRegister placementRegister;
    PlayerInventory playerInventory;
    SignalRegister signalRegister;

    string lastCubeOutlinePosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rightClick = gameObject.GetComponent<RightClick>();
        leftClick = gameObject.GetComponent<LeftClick>();
        playerInventory = gameObject.GetComponent<PlayerInventory>();
        placementRegister = staticWorld.GetComponent<PlacementRegister>();
        signalRegister = staticWorld.GetComponent<SignalRegister>();
    }

    void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(0, camera1.eulerAngles.y, 0); // Rotates player based on camera rotation in y plane only to keep movement parallel to ground.

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddRelativeForce(movement * speed);
    }

    void Update()
    {
        // Sends out ray to where cursor is pointed and returns hitInfo.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 7))
        {
            // Draws cube outlining current where cursor is pointing if in range.
            GameObject hitObject = hitInfo.collider.gameObject;

            string cursorCube = placementRegister.CoordinatesVector3ToString(hitObject.transform.position);

            if (cursorCube != lastCubeOutlinePosition)
            {
                Destroy(cubeOutlineReference);
                cubeOutlineReference = Instantiate(cubeOutline, hitObject.transform.position, Quaternion.identity) as GameObject;
                lastCubeOutlinePosition = cursorCube;
            }

            // Checks for right click and hands off clicked object (hitInfo) to RightClick.cs.
            if (Input.GetMouseButton(1))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.red, 10f);

                if (Physics.Raycast(ray, out hitInfo, 100)) //100 == max distance 
                {

                    rightClick.RightClickHandler(hitInfo);
                }
            }

            // Checks for left click and hands off clicked object (hitInfo) to LeftClick.cs.
            if (Input.GetMouseButton(0))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.red, 10f);

                if (Physics.Raycast(ray, out hitInfo, 100))
                {
                    leftClick.LeftClickHandler(hitInfo);
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Vector3 clickedObjectPosition = hitInfo.collider.transform.position;
                string clickedCubeCoordinates = placementRegister.CoordinatesVector3ToString(clickedObjectPosition);
                if (placementRegister.coordinatesIDDictionary.ContainsKey(clickedCubeCoordinates))
                {
                    GameObject clickedObjectReference = placementRegister.ObjectLookupByCoordinateString(clickedCubeCoordinates);
                    if (clickedObjectReference.tag.Contains("Conductive"))
                    {
                        print("Pulsing conductive object");
                        signalRegister.PulseSignal(clickedCubeCoordinates, clickedObjectReference);
                    }
                }
            }
        }
        else
        {
            // Destroys cube outline if cursor is pointing at object out of range.
            Destroy(cubeOutlineReference);
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            playerInventory.CycleBackpackSelection("backward");
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            playerInventory.CycleBackpackSelection("forward");
        }
    }
}
