using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatePuzzleObjectTrigger : MonoBehaviour
{

    bool isActive;

    [Header("PuzzleState")]
    public PuzzleObjectState state;

    [Header("PuzzleInformationContainer")]
    public PuzzleInfo objectInfo;


    public enum PuzzleObjectState
    {
        Active,
        Inactive,

        Default
    }


}
