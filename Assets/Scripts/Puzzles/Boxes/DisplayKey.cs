using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DisplayKey : MonoBehaviour
{
    [SerializeField] private int countBox = 0;
    [SerializeField] public int nbOfBoxes = 8;

    void Update()
    {
        Debug.Log("countbox " + countBox);
        if (countBox == nbOfBoxes)
        {
            gameObject.SetActive(false);
        }
    }

    public void AddBox(int _box)
    {
        countBox += _box;
        Debug.Log("display key" + countBox);
    }
}
