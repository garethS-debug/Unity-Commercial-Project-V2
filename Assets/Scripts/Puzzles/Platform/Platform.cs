using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Platform : MonoBehaviour
{
    private Animator platformAnim;

    void Start()
    {
        platformAnim = transform.parent.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        platformAnim.SetBool("IsOpening", true);
    }
}
