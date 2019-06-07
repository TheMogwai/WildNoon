using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverClass : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("FullCover"))
            {
                other.GetComponentInParent<UnitCara>().fullCover.SetActive(true);
            }
            else if (gameObject.CompareTag("HalfCover"))
            {
                other.GetComponentInParent<UnitCara>().halfCover.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("FullCover"))
            {
                other.GetComponentInParent<UnitCara>().fullCover.SetActive(false);
            }
            else if (gameObject.CompareTag("HalfCover"))
            {
                other.GetComponentInParent<UnitCara>().halfCover.SetActive(false);
            }
        }
    }
}
