using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropObject : MonoBehaviour ,IDragHandler,IPointerDownHandler,IPointerUpHandler{
    [HideInInspector]
    public Vector3 defaultPosition;
    [HideInInspector]
    public Transform parent;
    public bool isDrag = false;
    
    // Use this for initialization

    private void OnEnable()
    {
        defaultPosition = transform.localPosition;
        parent = transform.parent;
    }

    // Update is called once per frame
    void Update () {
		
	}
   

    public void OnDrag(PointerEventData eventData)
    {
        var position = Input.mousePosition;
        position.z = 0.0f;
        transform.position = position;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        defaultPosition = transform.localPosition;
        parent = transform.parent;
        transform.SetParent(transform.parent);

        //GetComponent<CanvasGroup>().blocksRaycasts = false;
        isDrag = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        transform.localPosition = defaultPosition;
        isDrag = false;
    }
}
