using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class CameraController : MonoBehaviour {

    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float speedRot = 10.0f;
    [SerializeField] private float speedScroll = 1.5f;

    void Update()
    {
        float moveMouse = Input.GetAxisRaw("Mouse X") * speedRot;
        float moveScroll = Input.GetAxisRaw("Mouse ScrollWheel") * speedScroll;
        float moveHorizontal = Input.GetAxis("Horizontal") * speed;
        float moveVertical = Input.GetAxis("Vertical") * speed;

        moveHorizontal *= Time.deltaTime;
        moveVertical *= Time.deltaTime;

        transform.Translate(0, 0, moveScroll);

        transform.parent.Translate(moveHorizontal, 0, moveVertical);

        if (Input.GetMouseButton(2))
        {
            transform.parent.Rotate(0, moveMouse, 0, Space.World);
        }
    }
}
*/
public class CameraController : MonoBehaviour
{
    [Header("Camera Speed")]
    [SerializeField] private float panSpeed = 20f;
    [SerializeField] private float panBorderThickness = 10f;
    [SerializeField] private float speedScroll = 20f;
    [Space]
    [Header("Scroll Limit")]
    [SerializeField] private float minY = 20f;
    [SerializeField] private float maxY = 120f;
    [Space]
    [Header("Pan Limit")]
    [SerializeField] private Vector2 panLimit;

    private void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("z"))
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("q"))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        float Scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        pos.y -= Scroll * speedScroll * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
    }
}
