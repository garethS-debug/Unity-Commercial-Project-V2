using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayKey : MonoBehaviour
{
    int countBox = 0;

    void Update()
    {
        if (countBox == 2)
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
