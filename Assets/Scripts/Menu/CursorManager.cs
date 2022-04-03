using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public GameObject cursor;
    public Canvas canvas;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update(){
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);

        cursor.transform.position = canvas.transform.TransformPoint(pos);
    }
}
