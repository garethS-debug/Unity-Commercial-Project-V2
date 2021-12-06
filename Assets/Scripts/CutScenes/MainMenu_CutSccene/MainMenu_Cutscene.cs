using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class MainMenu_Cutscene : MonoBehaviour
{


    [Header("Delays")]
    public float cutSceneDelayAtStart = 2;
    public float cutSceneDelay = 5;

    [Header("References")]
    public Animator anim;
    public Animator camAnim;
    public Camera camera;

 

    [Header("Intro")]
    public GameObject whiteBackground;
    public GameObject Tree;
    public GameObject LoadingText;
    public GameObject startButton;


    [Header("UI")]
    public GameObject skipButton;

    [Header("Co-Courtines")]
    Coroutine theCutSceneCoRoutine;

   [HideInInspector] public  GameSetup gamesetup;


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

        whiteBackground.gameObject.SetActive(false);
        Tree.gameObject.SetActive(false);
        LoadingText.gameObject.SetActive(false);

     
       


    }

    // Update is called once per frame
    void Update()
    {
  
       // cutsceneCamera.transform.LookAt(trainFollowCube.transform);
    }


    /// <summary>
    /// REMMEMBER THERE ARE 2 SCENARIOs ----- START and CONTUNUE
    /// </summary>

    public void start_StartTutorialCutscene()
    {
        whiteBackground.gameObject.SetActive(true);

        //  startButton.gameObject.SetActive(false);
        startButton.gameObject.GetComponent<Button>().interactable = false;

        theCutSceneCoRoutine = StartCoroutine(start_CutSceneCoRoutine());
        
    }


    IEnumerator start_CutSceneCoRoutine()
    {

        camAnim.SetBool("PanCamera", true);


        //    intro.gameObject.SetActive(true);
        //Print the time of when the function is first called.
        // Debug.Log("Started IntroSplash Coroutine at timestamp : ".Bold().Color("yellow") + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(cutSceneDelayAtStart);

        anim.SetBool("FadeIn", true);
        Tree.gameObject.SetActive(true);
        LoadingText.gameObject.SetActive(true);
        //Print the time of when the function is first called.
        //  Debug.Log("Started Cutscene Coroutine at timestamp : ".Bold().Color("yellow") + Time.time);




        //After we have waited 5 seconds print the time again.
        //    Debug.Log("Finished Cutscene Coroutine at timestamp : ".Bold().Color("yellow") + Time.time);

        yield return new WaitForSeconds(cutSceneDelay);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Spawn Player after Cutscene Coroutine at timestamp : ".Bold().Color("yellow") + Time.time);

       gamesetup.SELECT_CHILDCHARACTER();


    }




    public void continue_StartTutorialCutscene()
    {

      
        whiteBackground.gameObject.SetActive(true);

        theCutSceneCoRoutine = StartCoroutine(continue_CutSceneCoRoutine());
        startButton.gameObject.GetComponent<Button>().interactable = false;



    }


    IEnumerator continue_CutSceneCoRoutine()
    {

        camAnim.SetBool("PanCamera", true);

        //    intro.gameObject.SetActive(true);
        //Print the time of when the function is first called.
        // Debug.Log("Started IntroSplash Coroutine at timestamp : ".Bold().Color("yellow") + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(cutSceneDelayAtStart);

        anim.SetBool("FadeIn", true);
        Tree.gameObject.SetActive(true);
        LoadingText.gameObject.SetActive(true);
        //Print the time of when the function is first called.
        //  Debug.Log("Started Cutscene Coroutine at timestamp : ".Bold().Color("yellow") + Time.time);




        //After we have waited 5 seconds print the time again.
        //    Debug.Log("Finished Cutscene Coroutine at timestamp : ".Bold().Color("yellow") + Time.time);

        yield return new WaitForSeconds(cutSceneDelay);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Spawn Player after Cutscene Coroutine at timestamp : ".Bold().Color("yellow") + Time.time);

        gamesetup.LoadNextScene();


    }



}
