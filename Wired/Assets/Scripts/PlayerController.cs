using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;
    public Transform camera1;

    private Rigidbody rb;
    private RightClick rightClick;
    private LeftClick leftClick;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rightClick = gameObject.GetComponent<RightClick>();
        leftClick = gameObject.GetComponent<LeftClick>();
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
        // Checks for right click and hands off clicked object (hitInfo) to RightClick.cs.
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            Debug.DrawRay(ray.origin, ray.direction, Color.red, 10f);

            if (Physics.Raycast(ray, out hitInfo, 100)) //100 == max distance 
            {
                
                rightClick.RightClickHandler(hitInfo);
            }
        }

        // Checks for left click and hands off clicked object (hitInfo) to LeftClick.cs.
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            Debug.DrawRay(ray.origin, ray.direction, Color.red, 10f);

            if (Physics.Raycast(ray, out hitInfo, 100))
            {
                leftClick.LeftClickHandler(hitInfo);
            }
        }
    }
}
