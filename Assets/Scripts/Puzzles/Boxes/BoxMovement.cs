using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BoxMovement : MonoBehaviour
{

    public DisplayKey displayKey;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TargetTrigger"))
        {
            displayKey.AddBox(1);
            Debug.Log("box +1");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("TargetTrigger"))
        {
            displayKey.AddBox(-1);
            Debug.Log("box -1");
        }
    }
}
