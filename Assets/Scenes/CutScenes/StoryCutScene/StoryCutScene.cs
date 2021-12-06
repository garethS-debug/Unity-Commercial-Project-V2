using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryCutScene : MonoBehaviour
{

    [Header("Camera")]
    public Camera cutsceneCamera;

    [Header("Train")]
    public GameObject train;
    public float trainspeed = 1;
    [Header("Delays")]

    public float cutSceneDelayAtStart = 2;
    public float cutSceneDelay = 5;

    [Header("References")]
    public Animator trainAnimator;
    public LobbyManager lobbyManager;

    public GameObject trainFollowCube;

    [Header("Intro")]
    public GameObject intro;
    public TMP_Text txt;

   

    [Header("Debug")]
    public bool DebugSkipCutScene;

    [Header("UI")]
    public GameObject skipButton;

    [Header("Co-Courtines")]
    Coroutine theCutSceneCoRoutine;

    [Header("Dialog")]
    public DialogHint dialog;


    [Header("Scene Ref")]
    public SceneReference levelToLoad;


    /*
     * 
     *   
  \n    New line
  \t    Tab
  \v    Vertical Tab
  \b    Backspace
  \r    Carriage return
  \f    Formfeed
  \\    Backslash
  \'    Single quotation mark
  \"    Double quotation mark
  \d    Octal
  \xd    Hexadecimal
  \ud    Unicode character
     */

    // Start is called before the first frame update
    void Start()
    {
        //   trainAnimator.SetBool("StartEntry", false);
        //Start the coroutine we define below named ExampleCoroutine.
        if (DebugSkipCutScene == false)
        {
            theCutSceneCoRoutine = StartCoroutine(CutSceneCoRoutine());
        }

        if (DebugSkipCutScene == true)
        {
            lobbyManager.OnTriggerSpawnPlayers();
        }

        skipButton.gameObject.SetActive(true);


    }

    void Awake()
    {
        //  story
      //  txt.text = "";


    }


    // Update is called once per frame
    void Update()
    {
        // cutsceneCamera.transform.LookAt(trainFollowCube.transform);
        // Move the object forward along its z axis 1 unit/second.
       train.transform.Translate(Vector3.forward  * Time.deltaTime * trainspeed);
    }

    IEnumerator CutSceneCoRoutine()
    {
    

       
        yield return new WaitForSeconds(cutSceneDelayAtStart);
        dialog.StartDialog();
        //Print the time of when the function is first called.
        //  Debug.Log("Started Cutscene Coroutine at timestamp : ".Bold().Color("yellow") + Time.time);




        //After we have waited 5 seconds print the time again.
        //    Debug.Log("Finished Cutscene Coroutine at timestamp : ".Bold().Color("yellow") + Time.time);

        yield return new WaitForSeconds(cutSceneDelay);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Move to next scene : ".Bold().Color("yellow") + Time.time);

        skipButton.gameObject.SetActive(false);

    }









    public void SkipTheCutScene()
    {
        intro.gameObject.SetActive(false);
        trainAnimator.SetBool("StartEntry", true);

        StopCoroutine(theCutSceneCoRoutine);

        StartCoroutine(SkipCutscene());




        cutSceneDelayAtStart = 0;
        cutSceneDelay = 0;
        skipButton.gameObject.SetActive(false);
    }

    IEnumerator SkipCutscene()
    {
        yield return new WaitForSeconds(3);

        lobbyManager.OnTriggerSpawnPlayers();
    }
}
