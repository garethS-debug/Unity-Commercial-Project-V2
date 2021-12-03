using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class Level01Cutscene : MonoBehaviour
{

    [Header("Camera")]
    public Camera cutsceneCamera;
    public Animator camAnim;

    [Header("Fog")]
    public Animator fogAnim;

    [Header("UI")]
    public GameObject skipButton;
    public Image Fadeout;
    public Animator fadeanimator;

    [Header("Intro")]
    public GameObject intro;
    public TMP_Text txt;
    public GameObject textObj;
    [TextArea]
    public string story;


    [Header("Delays")]
    public float cutSceneDelayAtStart = 2;
    public float cutSceneDelay = 5;
    public float cameraandFogDelay = 5;


    [Header("RommManager")]
    public RoomManager roomManager;





    void Awake()
    {
        //txt = txt.GetComponent<TMP_Text>();
        //  story = txt.text;
        txt.text = "";

        // TODO: add optional delay when to start

    }

    // Start is called before the first frame update
    void Start()
    {
        Fadeout = Fadeout.GetComponent<Image>();
        fadeanimator = fadeanimator.GetComponent<Animator>();

        skipButton.gameObject.SetActive(true);


    }

    // Update is called once per frame
    void Update()
    {
        
    }



    IEnumerator CutSceneCoRoutine()
    {

        StartCoroutine("PlayText");
        intro.gameObject.SetActive(true);


       // Fadeout.alpha = Mathf.Lerp(Fadeout.alpha, 0, Time.deltaTime * fadeDelay);



        //Print the time of when the function is first called.

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(cutSceneDelayAtStart);
        camAnim.SetBool("Move", true);
        fadeanimator.SetBool("fade", true);             //fade out and pan camera down
    
      
      
        //Print the time of when the function is first called.
        // trainAnimator.SetBool("StartEntry", true);


        //After we have waited 5 seconds print the time again.
        yield return new WaitForSeconds(cutSceneDelay);
        textObj.SetActive(false);


        fogAnim.SetBool("moveFog", true);




        //After we have waited 5 seconds print the time again.


        yield return new WaitForSeconds(cameraandFogDelay);

        // lobbyManager.OnTriggerSpawnPlayers();
        roomManager.spawnPlayers();
        intro.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
    }



    public void StartCoRoutine()
    {
        StartCoroutine("CutSceneCoRoutine");
    }

    public void SkipTheCutScene()
    {
        intro.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        roomManager.spawnPlayers();


    }

    IEnumerator PlayText()
    {
        foreach (char c in story)
        {
            txt.text += c;
            yield return new WaitForSeconds(0.075f);

    
        }
    }




}
