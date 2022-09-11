using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EItemChangeType
{
    None = -1,
    Movement,
    Delete
}

[DefaultExecutionOrder(-2)]
public class SessionManager : Singleton<SessionManager>
{
    private StackExtention<SessionData> privateSessionData = new StackExtention<SessionData>();

    [SerializeField] private int MaxSession = 10;
    public delegate void SessionDataChangeDelegate();
    public event SessionDataChangeDelegate OnSessionDataEmpty;
    public event SessionDataChangeDelegate OnEntryFirstSessionData;

    public SessionData sessionData
    {
        get
        {
            if (privateSessionData.Count > 0)
            {
                SessionData data = privateSessionData.Pop();
                if (privateSessionData.Count == 0)
                    OnSessionDataEmpty?.Invoke();
                return data;
            }
            else
                return null;
        }
    }

    public void SetSessionData(EItemChangeType changeType = EItemChangeType.Movement)
    {
        Transform currentObject = GameManager.Instance._currentSelectedItem.Value.transform;
        if (currentObject == null)
            return;
        if (privateSessionData == null)
            privateSessionData = new StackExtention<SessionData>();
        if (privateSessionData.Count == 0)
            OnEntryFirstSessionData?.Invoke();
        SessionData newData = new SessionData(currentObject, currentObject.position, currentObject.localScale, changeType);
        if(privateSessionData.Count> MaxSession)
        {
            privateSessionData.Remove(privateSessionData.Count - 1);
        }
        privateSessionData.Push(newData);
    }

}


[Serializable]
public class SessionData
{
    public Transform item;
    public Vector3 itemPosition;
    public Vector3 itemScale;
    public EItemChangeType itemChangeType;

    public SessionData(Transform item, Vector3 itemPosition, Vector3 itemScale, EItemChangeType changeType)
    {
        this.item = item;
        this.itemPosition = itemPosition;
        this.itemScale = itemScale;
        this.itemChangeType = changeType;
    }
}