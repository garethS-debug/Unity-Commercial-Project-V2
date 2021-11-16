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
        //CreateDisplay();

  
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
        


        GameObject player = GameObject.FindGameObjectWithTag("Player");

        print("I FOUND " + player.name);

        // else
        //  {
        //  print("is null");
        //  }


        
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterID>().isHumanCharater)
        {
     
        }

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterID>().isGhostCharacer)
        {

        }
        

    




}

    public void CreateHumanCandle()
    {
        print("isHuman");

        var obj = Instantiate(griefBarHuman.griefBarPrefab, Vector3.zero, Quaternion.identity, transform);
        obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        slider.fillRect = obj.GetComponent<RectTransform>();
    }


    public void CreateGhostCandle()
    {
        print("isGhost");
        var obj = Instantiate(griefBarGhost.griefBarPrefab, Vector3.zero, Quaternion.identity, transform);
        obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        slider.fillRect = obj.GetComponent<RectTransform>();
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