using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogHint : MonoBehaviour
{

    public Animator anim;

    public TMP_Text text;

    public float secondsBetweenCharacters = 0.15f;

    [TextArea]
    public string[] dialogStrings;

    // Start is called before the first frame update
    void Start()
    {
        //testing
        DialogEntry();

        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void DialogEntry()
    {

        anim.SetBool("DialogEntry", true);
        anim.SetBool("DialogExit", false);
        StartCoroutine(DisplayString(dialogStrings[0]));
    }

    public void DialogExit()
    {

        anim.SetBool("DialogEntry", false);
        anim.SetBool("DialogExit", true);
    }


    IEnumerator DisplayString(string StringToDisplay)
    {

        int stringLength = StringToDisplay.Length;
        int currentCharacterIndex = 0;


        text.text = "";


        while (currentCharacterIndex < stringLength)
        {
            int dialogueLength = StringToDisplay.Length;
            int currentDialogueIndex = 0;

            while (currentDialogueIndex < dialogueLength)
            {
                text.text += StringToDisplay[currentCharacterIndex];
                currentCharacterIndex++;

                if (currentCharacterIndex < stringLength)
                {
                    yield return new WaitForSeconds(secondsBetweenCharacters);
                }

                else
                {
                    break;
                }
            }

            text.text = "";


        }

    }
}


