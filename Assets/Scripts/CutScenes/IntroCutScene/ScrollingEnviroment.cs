using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingEnviroment : MonoBehaviour
{
    [Header("Distance")]
    public float totalDistance = 0;
    public float maxDistnace = 300f;

    private Vector3 previousLoc;

    [Header("Objects to Respawn")]
    public GameObject[] sceneObjects;
    public float xOffset;
    public float yOffset;
    public float zOffset;

    public GameObject previouslyInstantiated;
    public List<GameObject> instantiatedItems = new List<GameObject>();

    public GameObject train;


    // Start is called before the first frame update
    void Start()
    {
        if (previouslyInstantiated != null)
        {
            instantiatedItems.Add(previouslyInstantiated);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RecordDistance();
    }


    void RecordDistance()
    {
        totalDistance += Vector3.Distance(train.transform.position, previousLoc);
        previousLoc = train.transform.position;
     

        if (totalDistance >= maxDistnace)
        {
            totalDistance = 0;
            Debug.Log("MoveScene");
            //trigger moving Scene

            foreach (GameObject sceneObj in sceneObjects)
            {
                if (previouslyInstantiated != null)
                {
                    GameObject newScenery = Instantiate(sceneObj, new Vector3(previouslyInstantiated.transform.position.x + xOffset, previouslyInstantiated.transform.position.y + yOffset, previouslyInstantiated.transform.position.z + zOffset), Quaternion.identity);
                    previouslyInstantiated = newScenery;
                    newScenery.AddComponent<destroyBackgroundItem>();
                    instantiatedItems.Add(newScenery);
                }

                if (previouslyInstantiated == null)
                {
                    GameObject newScenery = Instantiate(sceneObj, new Vector3(sceneObj.transform.position.x + xOffset, sceneObj.transform.position.y + yOffset, sceneObj.transform.position.z + zOffset), Quaternion.identity);
                    previouslyInstantiated = newScenery;
                    newScenery.AddComponent<destroyBackgroundItem>();
                    instantiatedItems.Add(newScenery);
                }
               
            }

           // RemoveListItem();

        }
    }

    public void RemoveListItem()
    {
        if (instantiatedItems.Count >= 3)
        {
            Destroy(instantiatedItems[0]);


          
         
        }
    }

}
