using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
public class DialogHint : MonoBehaviour
{
    private static DialogHint _instance;

    public static DialogHint Instance { get { return _instance; } }


    public Animator anim;

    public TMP_Text _textComponent;

    [Header("Dialog Rate")]
    public float secondsBetweenCharacters = 0.15f;
    public float characterRateMultiplayer = 0.5f;

    [Header("Skip Dialog")]
    public KeyCode DialogueInput = KeyCode.Return;

    [Header("Bools")]
    public bool _isStringBeingRevealed = false;
    public bool _isDialoguePlaying = false;
    public bool _isEndOfDialogue = false;
    public bool isIntroScene;
    public bool isEndofEntireDialog;
    [TextArea]
    public string[] DialogueStrings;

    public List<string> dialogList = new List<string>();

    //  [Header("Icons")]
    //  public GameObject ContinueIcon;
    //   public GameObject StopIcon;


    // Start is called before the first frame update
    void Start()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }



            if (isIntroScene == true)
        {
            this.gameObject.SetActive(false);
        }
        //testing
     //   DialogEntry();
   //     HideIcons();
        _textComponent.text = "";
    }

    // Update is called once per frame
    void Update()
    {
     
    }






    public void StartDialog()
    {
        
        if (isIntroScene == true)
        {
            this.gameObject.SetActive(true);
        }

        anim.SetBool("DialogEntry", true);
        anim.SetBool("DialogExit", false);
        



        if (!_isDialoguePlaying)
        {
            isEndofEntireDialog = false;
            _isDialoguePlaying = true;
            StartCoroutine(StartDialogue());


        }
       
    }

    public void DialogExit()
    {
        Debug.Log("Dialog Exit");
     //   isEndofEntireDialog = true;
        anim.SetBool("DialogEntry", false);
        anim.SetBool("DialogExit", true);
        
      //  _isStringBeingRevealed = false;
      //  _isEndOfDialogue = false;
      //  _isDialoguePlaying = false;
      //  _textComponent.text = "";
      //  dialogList.Clear();

    }



    private IEnumerator StartDialogue()
    {

        yield return new WaitForSeconds(2.5f);

        //int dialogueLength = DialogueStrings.Length;
        int dialogueLength = dialogList.Count;

        int currentDialogueIndex = 0;

        while (currentDialogueIndex < dialogueLength || !_isStringBeingRevealed)
        {
            if (!_isStringBeingRevealed)
            {
                _isStringBeingRevealed = true;

                // StartCoroutine(DisplayString(DialogueStrings[currentDialogueIndex++]));
                StartCoroutine(DisplayString(dialogList[currentDialogueIndex++]));

                if (currentDialogueIndex >= dialogueLength)
                {
                    _isEndOfDialogue = true;
                    Debug.Log("End of Dialog".Bold());
                       ShowIcon();
               
                }
            }

            yield return 0;
        }

        while (true)
        {
           // if (Input.GetKeyDown(DialogueInput))
           // {
              //  break;
          //  }

            yield return 0;
        }

        HideIcons();
        _isEndOfDialogue = false;
        _isDialoguePlaying = false;
    }

    private IEnumerator DisplayString(string stringToDisplay)
    {
        int stringLength = stringToDisplay.Length;
        int currentCharacterIndex = 0;

        HideIcons();

        _textComponent.text = "";

        while (currentCharacterIndex < stringLength)
        {
            _textComponent.text += stringToDisplay[currentCharacterIndex];
            currentCharacterIndex++;

            if (currentCharacterIndex < stringLength)
            {
                if (Input.GetKey(DialogueInput))
                {
                    yield return new WaitForSeconds(secondsBetweenCharacters * characterRateMultiplayer);
                }
                else
                {
                    yield return new WaitForSeconds(secondsBetweenCharacters);
                }
            }
            else
            {
                break;
            }
        }

        ShowIcon();

        while (true)
        {
           //  if (Input.GetKeyDown(DialogueInput))
           //   {
            yield return new WaitForSeconds(1);
            break;
           // }

           // yield return 0;
        }

        HideIcons();

        _isStringBeingRevealed = false;
        _textComponent.text = "";
    }

    private void HideIcons()
    {
     //   ContinueIcon.SetActive(false);
      //  StopIcon.SetActive(false);
    }

    private void ShowIcon()
    {
        if (_isEndOfDialogue)
        {
            //  StopIcon.SetActive(true);
            Debug.Log("Dialog Exit call".Bold());
            DialogExit();




            return;
        }
    
        //   ContinueIcon.SetActive(true);
    }



    [PunRPC]
   public void TriggerDialogOnAllClients(List<string> dialog)
    {
        ///Need some text to populate
        ///DialogueStrings
        Debug.Log("Running Dialog");


        dialogList.Clear();

        foreach (string dilaog in dialog)
        {
            dialogList.Add(dilaog);
        }
        StartDialog();
    }




}


