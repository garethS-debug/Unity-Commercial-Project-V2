using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SceneSettings : MonoBehaviour
{
    private static SceneSettings _instance;

    public static SceneSettings Instance { get { return _instance; } }


    public PlayerSO playerSOData;


    public bool isSinglePlayer;

    public bool isMultiPlayer;


    public bool DebugMode;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;


            if (playerSOData.SingleOrMultiPlayer == 1)
            {
                // 1 = multiplayer
                //2 = single player
             isMultiPlayer = true;
                isSinglePlayer = false;

            }

            else if (playerSOData.SingleOrMultiPlayer == 2)
            {
              isSinglePlayer = true;
                isMultiPlayer = false;
            }



            if (isSinglePlayer == false && isMultiPlayer == false)
            {
                Debug.LogError("SceneSettings : Set bool to either single or multiplayer");
            }
        }
    }


    public void RemoveMultiplayerScript(GameObject subject)
    {
        if (subject.gameObject.GetComponent<PhotonTransformView>() == true)
        {
            Destroy(subject.gameObject.GetComponent<PhotonTransformView>());
        }

        if (subject.gameObject.GetComponent<PhotonView>() == true)
        {
            Destroy(subject.gameObject.GetComponent<PhotonView>());
        }
    }

}

