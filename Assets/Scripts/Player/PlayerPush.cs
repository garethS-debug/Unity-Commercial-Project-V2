using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    float pushPower = 4.0f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == "Box")
        {
            Rigidbody boxRb = hit.collider.attachedRigidbody;

            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            boxRb.velocity = pushDir * pushPower;
        }
    }
}
