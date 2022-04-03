using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMover : MonoBehaviour
{
    public Canvas canvas;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update(){
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);

        transform.position = canvas.transform.TransformPoint(pos);
    }
}
