using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    private float forceMagnitude = 1f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == "Box")
        {
            Rigidbody boxRb = hit.collider.attachedRigidbody;

            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            if (Mathf.Abs(forceDirection.x) > Mathf.Abs(forceDirection.z))
            {
                forceDirection.z = 0f;
            }

            else if (Mathf.Abs(forceDirection.z) > Mathf.Abs(forceDirection.x))
            {
                forceDirection.x = 0f;
            }

            boxRb.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);

        }
    }
}
