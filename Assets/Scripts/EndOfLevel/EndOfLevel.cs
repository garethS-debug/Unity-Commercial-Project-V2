using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class EndOfLevel : MonoBehaviourPunCallbacks
{


    public SceneReference levelToLoad;

    public float cutSceneDelayAtStart = 5;


    public GameObject thankYouUI;

    // Start is called before the first frame update
    void Start()
    {
        thankYouUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine("CutSceneCoRoutine");
        }
       
    }



    IEnumerator CutSceneCoRoutine()
    {
        thankYouUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(cutSceneDelayAtStart);
        RestartGame();
    }




    public void RestartGame()
    {

        if (SceneSettings.Instance.isSinglePlayer)
        {
            SceneManager.LoadScene(levelToLoad);
        }

        if (SceneSettings.Instance.isMultiPlayer)
        {
            // PhotonNetwork.LeaveRoom();

            SwitchLevel(levelToLoad);
       
        }



    }


    //public override void OnLeftRoom()
    //{
    //    base.OnLeftRoom();
    //    //StartCoroutine(DisconnectAndLoad());
    //    SceneManager.LoadScene(levelToLoad);
    //}



    public void SwitchLevel(SceneReference level)
    {
        StartCoroutine(DoSwitchLevel(level));
    }

    IEnumerator DoSwitchLevel(string level)
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        SceneManager.LoadScene(levelToLoad);
    }

}

