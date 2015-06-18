using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Vector3 myPos;
    public Transform player;
    public int lookSensitivity;

    public Texture2D cursorTexture;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        // Lock cursor position to middle of screen with custom texture. 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void Update()
    {
        // Tracks camera to player location.
        transform.position = player.position + myPos;

        // Rotates camera based on mouse movement and keeps z rotation at 0 to prevent "leaning" during rotation.
        transform.Rotate(Input.GetAxis("Mouse Y") * -1 * lookSensitivity, Input.GetAxis("Mouse X") * lookSensitivity, 0);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0); 
    }
}
