using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerBonfireTrigger : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiTriggerMessage;
    public GameObject uiLevelSelect;
    public bool displayUI;



    // Start is called before the first frame update
    void Start()
    {
        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            Destroy(this.gameObject);
        }


            uiTriggerMessage.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            uiTriggerMessage.gameObject.SetActive(true);
          
        }
    

      
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            print("performing Action? : " + other.gameObject.GetComponent<NetworkedPlayerController>().PermormingAction);


            if (other.gameObject.GetComponent<NetworkedPlayerController>().PermormingAction == true)
            {
                print("Display Level Select UI");

               

                    uiLevelSelect.SetActive(true);
                    displayUI = true;
             
        

            }
        }
      
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            uiTriggerMessage.gameObject.SetActive(false);
        }
    }

}
