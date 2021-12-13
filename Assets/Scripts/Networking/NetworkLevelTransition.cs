using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkLevelTransition : MonoBehaviour
{

    public static NetworkLevelTransition Instance;
    //    public string PlayerName

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;

        }

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
