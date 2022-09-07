using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/3D Item List")]
public class ItemsSOBluePrint : SerializedValue<List<GameItems>>
{   
}

[System.Serializable]
public class GameItems
{
    public GameObject itemPrefab;
    public Sprite itemImage;
}
