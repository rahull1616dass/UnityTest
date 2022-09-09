using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEditorUIScript : MonoBehaviour
{
    [SerializeField] private RectTransform m_AreaOfEditor;
    public void EnableOrDisableTheItemEditor(bool state)
    {
        gameObject.SetActive(state);
    }

    public void PositionTheUIArea(Rect areaRect)
    {
        m_AreaOfEditor.rect.Set(areaRect.x, areaRect.y, areaRect.width, areaRect.height);
    }
}
