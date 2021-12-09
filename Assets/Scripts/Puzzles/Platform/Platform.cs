using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
