using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class EndOfLevel : MonoBehaviourPunCallbacks
{
    public static EndOfLevel Instance;

    public SceneReference levelToLoad;

    public float cutSceneDelayAtStart = 5;


    public GameObject thankYouUI;

    public GameObject whiteBackground;
    public GameObject Laoding;
    public GameObject tree;


    private void Awake()
    {
        if (Instance)                                   //If room manager already in scene
        {
            Destroy(gameObject);                        //If yes destroy
            print("Destroying this copy");
            return;

        }

        else
        {
            Instance = this;
            print("I am the only 1");

        }
    }
    // Start is called before the first frame update
    void Start()
    {
       // thankYouUI.gameObject.SetActive(false);


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
            if (SceneSettings.Instance.isMultiPlayer)
            {
                // PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel(levelToLoad);
                Debug.Log("Running Switch Level call");
            }
        }
       
    }



    IEnumerator CutSceneCoRoutine()
    {
       
        yield return new WaitForSeconds(1);


        whiteBackground.gameObject.GetComponent<Animator>().SetBool("Entry", true);
        tree.gameObject.gameObject.GetComponent<Animator>().SetBool("Entry", true);
        whiteBackground.gameObject.GetComponent<Animator>().SetBool("Exit", false);
        tree.gameObject.gameObject.GetComponent<Animator>().SetBool("Exit", false);
        yield return new WaitForSeconds(0.1f);
        whiteBackground.gameObject.GetComponent<Animator>().SetBool("Entry", false);
        tree.gameObject.gameObject.GetComponent<Animator>().SetBool("Entry", false);
        whiteBackground.gameObject.GetComponent<Animator>().SetBool("Exit", true);
        tree.gameObject.gameObject.GetComponent<Animator>().SetBool("Exit", true);
        yield return new WaitForSeconds(0.01f);


        Laoding.gameObject.SetActive(true);
        whiteBackground.gameObject.SetActive(true);
        tree.gameObject.SetActive(true);






        yield return new WaitForSeconds(3);
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

            SwitchLevel();
            //  photonView.RPC("RPC_SwitchLevel", RpcTarget.All/* tempHit.GetPhotonView().viewID*/ );
            PhotonNetwork.LoadLevel(levelToLoad);
            Debug.Log("Running Switch Level call");
        }



    }


    //public override void OnLeftRoom()
    //{
    //    base.OnLeftRoom();
    //    //StartCoroutine(DisconnectAndLoad());
    //    SceneManager.LoadScene(levelToLoad);
    //}


   
    public void SwitchLevel()
    {
        StartCoroutine(DoSwitchLevel(levelToLoad));
        Debug.Log("Running Switch Level method");
    }


    IEnumerator DoSwitchLevel(string level)
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Running Switch Level Co-routine");
        //PhotonNetwork.Disconnect();
   
       // while (PhotonNetwork.IsConnected)
       //     yield return null;
        Debug.Log("Running Switch level");

        if (SceneSettings.Instance.isSinglePlayer)
        {
            SceneManager.LoadScene(levelToLoad);
        }

        if (SceneSettings.Instance.isMultiPlayer)
        {
            Debug.Log("Running Switch level - multiplayer");
            PhotonNetwork.AutomaticallySyncScene = true;

            Debug.Log("Running Switch level - PhotonNetworkLevelToLoad");
            PhotonNetwork.LoadLevel(levelToLoad);
        }

    }







    }

