using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    [SerializeField] private float pushPower = 3.0f;
    [SerializeField] private float forceMagnitude;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == "Box")
        {
            Rigidbody boxRb = hit.collider.attachedRigidbody;

            //Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            //boxRb.velocity = pushDir * pushPower;

            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            boxRb.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);

        }
    }
}
