using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpScript : MonoBehaviour
{
    public Transform ConnectedWarp;
    public string TagList = "Units";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("touché pd");
        if (TagList.Contains(string.Format("Units", other.tag)))
        {
            Debug.Log("touché fdp");
            other.transform.position = ConnectedWarp.transform.position;
            other.transform.rotation = ConnectedWarp.transform.rotation;
        }
    }
}
