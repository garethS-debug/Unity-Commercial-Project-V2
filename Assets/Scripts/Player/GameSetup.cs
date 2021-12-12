using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSetup : MonoBehaviour
{

	//Check for save game

	// if no save game, then ask player to select character. 

	// Save that characte choice


	//Lobby check's save file for player choice and uses that info

	[Header("Static Variables")]
	public static int PlayerCharacter;
	public static SaveGameManager.SaveFile playerSaveFile;
	public static PlayerSO staticPlayerData;
	public static string playerName;


	[Header("UI")]
	public GameObject selectCharacterUI;
	public GameObject StartUI;
	public TMP_InputField GhostNameInput;
	public TMP_InputField ChildNameInput;



	[Header("Scene Ref")]
	public SceneReference levelToLoad;
	
	
	[Header("SO")]
	public PlayerSO playerSOData;

	[Header("Tutorial")]
	public MainMenu_Cutscene tutorialCutscene;

	// Start is called before the first frame update
	void Start()
    {
		staticPlayerData = playerSOData;


		print("example text in bold".Bold());
		print("example text in italics".Italic());
		print("example in Color".Color("red"));
		print("combine two or more in one".Bold().Color("yellow"));
		print("size also but be careful".Size(20));
	}

    // Update is called once per frame
    void Update()
    {
        
    }



	// Note: The Awake() on this script must run before the SaveGameManager.Awake().
	internal static void LoadDataFromSaveFile(SaveGameManager.SaveFile saveFile)                                                                                        //Method for loading information from the save file. 
	{

		//print("Selected character number " +  saveFile.slectedCharacter);

		playerSaveFile = saveFile;
		PlayerCharacter = saveFile.slectedCharacter;

		

		/*
		// Handle StepRecords - x1 for loop
		foreach (StepRecord sRec in saveFile.stepRecords)                                                                                               //1 Foreach Loop using a dictionary
		{                                                                                                                                               //accessing the information using a dictionary to 'target' the info
			if (STEP_REC_DICT.ContainsKey(sRec.type))
			{

				STEP_REC_DICT[sRec.type].num = sRec.num;                                                                                                //Cannot be accessed if the 'record' is a struct. Has to be a class
			}
		}

		// Handle Achievements - x2 for loops
		foreach (Achievement achievementSave in saveFile.achievements)                                                                                  //2 nested foreach loops (an alternative way to access the information)
		{
			//	This nested loop is not an efficient way to do this, but the number of 
			//  Achievements is so small that it will work fine. I could have made
			//  a Dictionary<string,Achievement> for Achievements as I did with 
			//  StepRecords, but I wanted to show both ways of doing this.
			foreach (Achievement achievement in Instance.achievements)
			{
				if (achievementSave.name == achievement.name)                                                                                           // This is the same Achievement
				{

					achievement.completed = achievementSave.completed;                                                                                  //Information can only be altered if its a 'class'. Struct inforamtion cannot be altered
				}
			}
		}
		*/

	}


	public void SAVE_BUTTON()
	{
		SaveGameManager.Save();
	}

	public void LOAD_BUTTON()
	{
		SaveGameManager.Load();
	}


	public void SELECT_CHILDCHARACTER()
    {

		PlayerCharacter = 0;                                            //Human = 1
		playerSOData.PlayerCharacterChoise = PlayerCharacter;
		//Debug.Log("Saving....");
		playerName = ChildNameInput.text;
		SaveGameManager.Save();
		SceneManager.LoadScene(levelToLoad);
	}


	public void SELECT_GHOSTCHARACTER()
	{
		PlayerCharacter = 1;										//Ghost = 2
		playerSOData.PlayerCharacterChoise = PlayerCharacter;
		//Debug.Log("Saving....");
		playerName = GhostNameInput.text;
		SaveGameManager.Save();
		SceneManager.LoadScene(levelToLoad);
	}


	public void m_START_GAME()
    {
	 bool isSave =	SaveGameManager.CheckforSaveGame();
	if (isSave)
        {
		//	Debug.Log("Continue to game with your character...");
			SaveGameManager.Load();

			UpdatePlayerSaveSO(1);                      //1 = multiplayer

			///---- StartTutorialCutScene--->
			///SELECT_CHILDCHARACTER();
			tutorialCutscene.gamesetup = this.gameObject.GetComponent<GameSetup>();
			tutorialCutscene.continue_StartTutorialCutscene();
		
		}




	else
        {
			//Moving character select to lobby creation -------------> 
			/*
			selectCharacterUI.SetActive(true);
			StartUI.SetActive(false);
			*/
			///SELECT_CHILDCHARACTER();
			PlayerCharacter = 0;                                            //Human = 0
			playerSOData.PlayerCharacterChoise = PlayerCharacter;
			//Debug.Log("Saving....");
			playerName = ChildNameInput.text;
			SaveGameManager.Save();

			///---- StartTutorialCutScene--->
			
			tutorialCutscene.gamesetup = this.gameObject.GetComponent<GameSetup>();
			tutorialCutscene.start_StartTutorialCutscene();
		}

	}


	public void s_START_GAME()
	{
		bool isSave = SaveGameManager.CheckforSaveGame();

		if (isSave)
		{
			//	Debug.Log("Continue to game with your character...");
			SaveGameManager.Load();
			UpdatePlayerSaveSO(2);                  //2 = Single Player

			///---- StartTutorialCutScene--->
			///SELECT_CHILDCHARACTER();
			tutorialCutscene.gamesetup = this.gameObject.GetComponent<GameSetup>();
			tutorialCutscene.continue_StartTutorialCutscene();
		}
		else
		{
			//Moving character select to lobby creation -------------> 
			/*
			selectCharacterUI.SetActive(true);
			StartUI.SetActive(false);
			*/

			///---- StartTutorialCutScene--->
			///SELECT_CHILDCHARACTER();
			tutorialCutscene.gamesetup = this.gameObject.GetComponent<GameSetup>();
			tutorialCutscene.start_StartTutorialCutscene();

		}

	}



	public  static void UpdatePlayerSaveSO(int SingleorMulti)
    {
		
		//Insert other variable information -- e.g. level progression
		staticPlayerData.PlayerCharacterChoise = playerSaveFile.slectedCharacter;
		staticPlayerData.PlayerName = playerSaveFile.playerName;
		staticPlayerData.SingleOrMultiPlayer = SingleorMulti;

		Debug.Log("Updating SO......." + playerSaveFile.slectedCharacter);

		

	}

	public void LoadNextScene()
    {
		SceneManager.LoadScene(levelToLoad);
	}
}
