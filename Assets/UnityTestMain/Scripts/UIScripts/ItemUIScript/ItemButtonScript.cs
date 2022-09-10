using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonScript : MonoBehaviour
{
    public delegate void InstantiateItem(ItemController item);

    public InstantiateItem OnItemInstantiate;

    public void CreateButton(Sprite buttonImage, GameObject objectPrefab)
    {
        GetComponent<Image>().sprite = buttonImage;
        GetComponent<Button>().onClick.AddListener(() => { OnItemInstantiate?.Invoke(objectPrefab.GetComponent<ItemController>()); });
    }
}
