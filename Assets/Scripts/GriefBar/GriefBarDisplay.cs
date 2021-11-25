using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GriefBarDisplay : MonoBehaviour
{
    public Slider slider;
    public GriefBarObject griefBarHuman;
    public GriefBarObject griefBarGhost;
    public float maxGrief = 100;
    public float currentGrief;

    public RoomManager room;

    /// <summary>
    /// 
    /// 
    /// 
    /// PLEASE BE AWARE I HAVE CHANGED THE BELOW:
    /// 
    /// ADDED A NEW METHOD FOR CALLING THE PLAYER DISTANCE FROM SCENE SETTINGS
    /// 
    /// CHANGED THE INTERGER TO FLOAT SO THAT THE CANDLE HEIGHT IS SMOOTHER
    /// 
    /// 
    /// 
    /// 
    /// </summary>



    public void Start()
    {
        currentGrief = maxGrief;
        SetMaxGrief(maxGrief);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ReduceGriefBar(20);
        }

        GriefBasedOnDistance();

    }

    public void CreateHumanCandle()
    {
        var obj = Instantiate(griefBarHuman.griefBarPrefab, Vector3.zero, Quaternion.identity, transform);
        obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        slider.fillRect = obj.GetComponent<RectTransform>();
        slider.direction = Slider.Direction.BottomToTop;                                            //G - I have added this line to ensure the slide goes up and down and not left to right
    }

    public void CreateGhostCandle()
    {
        var obj = Instantiate(griefBarGhost.griefBarPrefab, Vector3.zero, Quaternion.identity, transform);
        obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        slider.fillRect = obj.GetComponent<RectTransform>();
        slider.direction = Slider.Direction.BottomToTop;                                            //G - I have added this line to ensure the slide goes up and down and not left to right
    }

    private void SetMaxGrief(float grief)
    {
        slider.maxValue = grief;
        slider.value = grief;
    }

    private void SetGrief(float grief)
    {
        slider.value = grief;
    }

    private void ReduceGriefBar(float reduction)
    {


        currentGrief -= reduction;
        SetGrief(currentGrief);
    }


    // I have added the below method that links in with 'scenesettings'. The 'sceneSettings' script judges the distance of the players, this is used to determine the length of the candle. 

    public void GriefBasedOnDistance ()
    {
        currentGrief = SceneSettings.Instance.playerdistance;
        print(currentGrief);
        SetGrief(currentGrief);
    }
}
