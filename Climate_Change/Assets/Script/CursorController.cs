using UnityEngine;

public class CursorController : MonoBehaviour
{
    private Vector3 lastMousePosition;
    private float lastMoveTime;
    private float hideDelay = 1f;

    void Update()
    {
        if (Input.mousePosition != lastMousePosition)
        {
            Cursor.visible = true;
            lastMoveTime = Time.time;
            lastMousePosition = Input.mousePosition;
        }
        else if (Cursor.visible && Time.time - lastMoveTime > hideDelay)
        {
            Cursor.visible = false;
        }
    }
}