using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;
    private Rigidbody rb;
    public Transform camera1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(0, camera1.eulerAngles.y, 0);

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddRelativeForce(movement * speed);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            Debug.DrawRay(ray.origin, ray.direction, Color.red, 10f);

            if (Physics.Raycast(ray, out hitInfo, 100)) //100 == max distance 
            {
                RightClick rightClick = gameObject.GetComponent<RightClick>();
                rightClick.RightClickHandler(hitInfo);
            }
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            Debug.DrawRay(ray.origin, ray.direction, Color.red, 10f);

            if (Physics.Raycast(ray, out hitInfo, 100))
            {
                LeftClick leftClick = gameObject.GetComponent<LeftClick>();
                leftClick.LeftClickHandler(hitInfo);
            }
        }
    }
}
