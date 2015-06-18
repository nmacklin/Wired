using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Vector3 myPos;
    public Transform myPlay;
    public int lookSensitivity;

    public Texture2D cursorTexture;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void Update()
    {
        transform.position = myPlay.position + myPos;
        transform.Rotate(Input.GetAxis("Mouse Y") * -1 * lookSensitivity, Input.GetAxis("Mouse X") * lookSensitivity, 0);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }
}
