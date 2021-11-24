using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player_Data", menuName = "puzzle_data/puzzle_Data_SO")]
public  class PuzzleItemInfo : ScriptableObject

{
    public string objectName;
    public GameObject prefab;
    public Sprite uiImage;
    public Item inventoryItem;
    //public PuzzleType type;


   // public string description;
}
public enum PuzzleType
    {
        PuzzlePiece1,
    PuzzlePiece2,
    PuzzlePiece3,
    PuzzlePiece4,
        PuzzlePiece5,
    PuzzlePiece6,
        Default
    }

