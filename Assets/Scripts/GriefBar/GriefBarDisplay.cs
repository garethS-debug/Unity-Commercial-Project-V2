using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GriefBarDisplay : MonoBehaviour
{
    public Slider slider;
    public GriefBarObject griefBarHuman;
    public GriefBarObject griefBarGhost;
    public float maxGrief = 100f;
    public float currentGrief;
    private float decreasePerSecond = 0.1f;

    public RoomManager room;

    public void Start()
    {
        currentGrief = maxGrief;
        SetMaxGrief(maxGrief);
    }

    private void Update()
    {
        ReduceGriefBar();
    }

    public void CreateHumanCandle()
    {
        var obj = Instantiate(griefBarHuman.griefBarPrefab, Vector3.zero, Quaternion.identity, transform);
        obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        slider.fillRect = obj.GetComponent<RectTransform>();
        slider.direction = Slider.Direction.BottomToTop;
    }

    public void CreateGhostCandle()
    {
        var obj = Instantiate(griefBarGhost.griefBarPrefab, Vector3.zero, Quaternion.identity, transform);
        obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        slider.fillRect = obj.GetComponent<RectTransform>();
        slider.direction = Slider.Direction.BottomToTop;
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
    private void ReduceGriefBar()
    {
        currentGrief -= Time.deltaTime * decreasePerSecond;
        SetGrief(currentGrief);
    }


    // I have added the below method that links in with 'scenesettings'. The 'sceneSettings' script judges the distance of the players, this is used to determine the length of the candle. 

    public void GriefBasedOnDistance ()
    {
        if (SceneSettings.Instance.isMultiPlayer == true)
        {
            currentGrief = SceneSettings.Instance.playerdistance;
            SetGrief(currentGrief);           
        }
    }
}
