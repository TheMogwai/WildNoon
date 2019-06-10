using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoMode : MonoBehaviour
{

    public GameObject Canvas;
    public GameObject NormalCam;
    public GameObject PhotoCam;
    Transform[] allInCanvas;

    private Vector2 MouseAxis
    {
        get { return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); }
    }
    private void Start()
    {
        allInCanvas = Canvas.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
        {
            for (int i = 0, l = allInCanvas.Length; i < l; ++i)
            {
                if (allInCanvas[i].gameObject.activeSelf)
                {
                    NormalCam.SetActive(false);
                    PhotoCam.SetActive(true);
                    allInCanvas[i].gameObject.SetActive(false);
                }
                else
                {
                    NormalCam.SetActive(true);
                    PhotoCam.SetActive(false);
                    allInCanvas[i].gameObject.SetActive(true);
                }
            }
        }

        if (PhotoCam.activeSelf)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                PhotoCam.transform.rotation = Quaternion.identity;
            }

            if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Mouse2))
            {
                PhotoCam.transform.Rotate(Vector3.up, -MouseAxis.y * Time.deltaTime * -100, Space.World);
            }
            else if (Input.GetKey(KeyCode.Mouse2))
            {
                PhotoCam.transform.Rotate(Vector3.left, MouseAxis.y * Time.deltaTime * 100, Space.Self);
            }
        }
    }
}
