using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GriefBarDisplay : MonoBehaviour
{
    public Slider slider;
    public GriefBarObject griefBarHuman;
    public GriefBarObject griefBarGhost;
    public int maxGrief = 100;
    public int currentGrief;

    public RoomManager room;


    public void Start()
    {
       
        currentGrief = maxGrief;
        SetMaxGrief(maxGrief);
     //   CreateDisplay();          // moving this to the room manager to ensure it runs after player spawn
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ReduceGriefBar(20);
        }
    }

    public void CreateDisplay()
    {

       // if (room.networkedPlayer!= null)
       // {

            if (room.networkedPlayer.gameObject.GetComponent<CharacterID>().isHumanCharater)
            {
                print("Setting Human Cnadle");
                var obj = Instantiate(griefBarHuman.griefBarPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
                slider.fillRect = obj.GetComponent<RectTransform>();
            }

            if (room.networkedPlayer.gameObject.GetComponent<CharacterID>().isGhostCharacer)
            {

                print("Setting Ghost Cnadle");
                var obj = Instantiate(griefBarGhost.griefBarPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
                slider.fillRect = obj.GetComponent<RectTransform>();
          //  }


        }

       // else
      //  {
          //  print("is null");
      //  }


        /*
        if (GameObject.Find("HumanPlayerCharacter"))
        {
            var obj = Instantiate(griefBarHuman.griefBarPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
            slider.fillRect = obj.GetComponent<RectTransform>();
        }

        if (GameObject.Find("GhostCharacter"))
        {
            var obj = Instantiate(griefBarGhost.griefBarPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
            slider.fillRect = obj.GetComponent<RectTransform>();
        }
        */
    }

    private void SetMaxGrief(int grief)
    {
        slider.maxValue = grief;
        slider.value = grief;
    }

    private void SetGrief(int grief)
    {
        slider.value = grief;
    }

    private void ReduceGriefBar(int reduction)
    {
        currentGrief -= reduction;
        SetGrief(currentGrief);
    }
}
