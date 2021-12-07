using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogHint : MonoBehaviour
{

    public Animator anim;

    public TMP_Text _textComponent;

    [Header("Dialog Rate")]
    public float secondsBetweenCharacters = 0.15f;
    public float characterRateMultiplayer = 0.5f;

    [Header("Skip Dialog")]
    public KeyCode DialogueInput = KeyCode.Return;

    [Header("Bools")]
    private bool _isStringBeingRevealed = false;
    private bool _isDialoguePlaying = false;
    private bool _isEndOfDialogue = false;
    public bool isIntroScene;
    public bool isEndofEntireDialog;
    [TextArea]
    public string[] DialogueStrings;


  //  [Header("Icons")]
  //  public GameObject ContinueIcon;
 //   public GameObject StopIcon;


    // Start is called before the first frame update
    void Start()
    {

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
        isEndofEntireDialog = true;
       anim.SetBool("DialogEntry", false);
        anim.SetBool("DialogExit", true);
    }



    private IEnumerator StartDialogue()
    {
        int dialogueLength = DialogueStrings.Length;
        int currentDialogueIndex = 0;

        while (currentDialogueIndex < dialogueLength || !_isStringBeingRevealed)
        {
            if (!_isStringBeingRevealed)
            {
                _isStringBeingRevealed = true;
                StartCoroutine(DisplayString(DialogueStrings[currentDialogueIndex++]));

                if (currentDialogueIndex >= dialogueLength)
                {
                    _isEndOfDialogue = true;
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
            // if (Input.GetKeyDown(DialogueInput))
            //  {
            yield return new WaitForSeconds(2);
            break;
          //  }

            yield return 0;
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
            Debug.Log("Dialog Exit call");
            DialogExit();




            return;
        }
    
        //   ContinueIcon.SetActive(true);
    }




}


