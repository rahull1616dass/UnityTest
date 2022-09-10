using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIRectAreaOf3DObject 
{
    public static Rect CovertObjectToRect(Camera cam, GameObject ObjectIn3DSpace)
    {
        var renderer = ObjectIn3DSpace.GetComponent<Renderer>();
        Vector3 centerOfTheObject = renderer.bounds.center;
        Vector3 extentionOfThePbjectFromCenter = renderer.bounds.extents;
        Vector2[] extentPoints = new Vector2[8]
        {
         cam.WorldToScreenPoint(new Vector3(centerOfTheObject.x-extentionOfThePbjectFromCenter.x, 
         centerOfTheObject.y-extentionOfThePbjectFromCenter.y, centerOfTheObject.z-extentionOfThePbjectFromCenter.z)),
         cam.WorldToScreenPoint(new Vector3(centerOfTheObject.x+extentionOfThePbjectFromCenter.x, 
         centerOfTheObject.y-extentionOfThePbjectFromCenter.y, centerOfTheObject.z-extentionOfThePbjectFromCenter.z)),
         cam.WorldToScreenPoint(new Vector3(centerOfTheObject.x-extentionOfThePbjectFromCenter.x, 
         centerOfTheObject.y-extentionOfThePbjectFromCenter.y, centerOfTheObject.z+extentionOfThePbjectFromCenter.z)),
         cam.WorldToScreenPoint(new Vector3(centerOfTheObject.x+extentionOfThePbjectFromCenter.x, 
         centerOfTheObject.y-extentionOfThePbjectFromCenter.y, centerOfTheObject.z+extentionOfThePbjectFromCenter.z)),
         cam.WorldToScreenPoint(new Vector3(centerOfTheObject.x-extentionOfThePbjectFromCenter.x, 
         centerOfTheObject.y+extentionOfThePbjectFromCenter.y, centerOfTheObject.z-extentionOfThePbjectFromCenter.z)),
         cam.WorldToScreenPoint(new Vector3(centerOfTheObject.x+extentionOfThePbjectFromCenter.x, 
         centerOfTheObject.y+extentionOfThePbjectFromCenter.y, centerOfTheObject.z-extentionOfThePbjectFromCenter.z)),
         cam.WorldToScreenPoint(new Vector3(centerOfTheObject.x-extentionOfThePbjectFromCenter.x, 
         centerOfTheObject.y+extentionOfThePbjectFromCenter.y, centerOfTheObject.z+extentionOfThePbjectFromCenter.z)),
         cam.WorldToScreenPoint(new Vector3(centerOfTheObject.x+extentionOfThePbjectFromCenter.x, 
         centerOfTheObject.y+extentionOfThePbjectFromCenter.y, centerOfTheObject.z+extentionOfThePbjectFromCenter.z))
        };
        Vector2 min = extentPoints[0];
        Vector2 max = extentPoints[0];
        foreach (Vector2 v in extentPoints)
        {
            min = Vector2.Min(min, v);
            max = Vector2.Max(max, v);
        }
        return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }
}


