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

[DefaultExecutionOrder(-3)]
public class SessionManager : SingletonPersistent<SessionManager>
{
    [SerializeField] private SessionDataSOBluePrint privateSessionData;
    public delegate void SessionDataChangeDelegate();
    public event SessionDataChangeDelegate OnSessionDataEmpty;
    public event SessionDataChangeDelegate OnEntryFirstSessionData;

    public delegate void LoadOldSession(SessionDataSOBluePrint sessionDataChangeDelegate);
    public event LoadOldSession OnLoadOldSession;

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
        privateSessionData = (SessionDataSOBluePrint)FileHandler.LoadData("GameState", privateSessionData);
        OnLoadOldSession?.Invoke(privateSessionData);
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
        FileHandler.SaveData("GameState", privateSessionData);
        
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