using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EItemChangeType
{
    None = -1,
    Movement,
    Delete,
    Create
}

public class SessionHandler : MonoBehaviour
{
    [SerializeField] private SessionDataSOBluePrint privateSessionData;
    public delegate void SessionDataChangeDelegate();
    public event SessionDataChangeDelegate OnSessionDataEmpty;
    public event SessionDataChangeDelegate OnEntryFirstSessionData;

    public delegate void LoadOldSession(SerializedSessionDataList previousSessionDataList, Action<List<GameObject>> OnLoadFinished);
    public event LoadOldSession OnLoadOldSession;

    private SerializedSessionDataList allSessionDataListToSave;

    private void Start()
    {
        allSessionDataListToSave = new SerializedSessionDataList(new List<SerializedSessionData>());
    }

    public SessionData sessionData
    {
        get
        {
            if (privateSessionData.Value.Count > 0)
            {
                SessionData data = privateSessionData.Value.Pop();
                if (privateSessionData.Value.Count == 0)
                    OnSessionDataEmpty?.Invoke();
                return data;
            }
            else
                return null;
        }
    }

    public void DoLoadOldSession()
    {
        Debug.Log(Application.persistentDataPath);
        OnLoadOldSession?.Invoke((SerializedSessionDataList)FileHandler.LoadData("GameState", allSessionDataListToSave), OnFinishedLoadingData);
    }

    private void OnFinishedLoadingData(List<GameObject> OldSessionObjects)
    {
        allSessionDataListToSave = new SerializedSessionDataList(new List<SerializedSessionData>());
        foreach (GameObject gameObj in OldSessionObjects)
            SaveSessionDataToLocal(gameObj, EItemChangeType.Create);
    }

    public void SetSessionData(EItemChangeType changeType = EItemChangeType.Movement)
    {
        GameObject currentObject = GameManager.Instance._currentSelectedItem.Value.gameObject;
        if (currentObject == null)
            return;
        if (privateSessionData.Value == null)
            privateSessionData.Value = new StackExtention<SessionData>();
        if (privateSessionData.Value.Count == 0)
            OnEntryFirstSessionData?.Invoke();
        SessionData newData = new SessionData(currentObject, currentObject.transform.position, currentObject.transform.localScale, changeType);
        privateSessionData.Value.Push(newData);
    }



    public void SaveSessionDataToLocal(GameObject item, EItemChangeType changeType = EItemChangeType.Movement)
    {
        SerializedSessionData localData = new SerializedSessionData(item.GetInstanceID(), item.tag, item.transform.position, item.transform.localScale, changeType);
        allSessionDataListToSave.allDataList.Add(localData);
        FileHandler.SaveData("GameState", allSessionDataListToSave);
    }

}

[Serializable]
public class SerializedSessionDataList
{
    public List<SerializedSessionData> allDataList;

    public SerializedSessionDataList(List<SerializedSessionData> allSessionList)
    {
        this.allDataList = allSessionList;
    }
}

[Serializable]
public class SerializedSessionData
{
    public int id;
    public string itemTag;
    public float[] itemPosition;
    public float itemScale;
    public EItemChangeType itemChangeType;

    public SerializedSessionData(int id, string itemTag, Vector3 itemPosition, Vector3 itemScale, EItemChangeType changeType)
    {
        this.id = id;
        this.itemTag = itemTag;
        this.itemPosition = new float[3];
        this.itemPosition[0] = itemPosition.x;
        this.itemPosition[1] = itemPosition.y;
        this.itemPosition[2] = itemPosition.z;
        this.itemScale = itemScale.x;
        this.itemChangeType = changeType;
    }
}

[Serializable]
public class SessionData
{
    public GameObject item;
    public Vector3 itemPosition;
    public Vector3 itemScale;
    public EItemChangeType itemChangeType;

    public SessionData(GameObject item, Vector3 itemPosition, Vector3 itemScale, EItemChangeType changeType)
    {
        this.item = item;
        this.itemPosition = itemPosition;
        this.itemScale = itemScale;
        this.itemChangeType = changeType;
    }
}