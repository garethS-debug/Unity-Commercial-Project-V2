using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public List<string> dialog = new List<string>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player Found");
            if (DialogHint.Instance != null)
            {
                DialogHint.Instance.TriggerDialogOnAllClients(dialog);
                Debug.Log("Dialog Found");
            }
             
            if (DialogHint.Instance == null)
            {
                Debug.LogError("No Instance of DialogHint");
            }

        }

        else
        {
            Debug.Log("Player Not Found");
        }
    }



    
}
