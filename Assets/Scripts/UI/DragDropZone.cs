using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropZone : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        DragDropObject d = eventData.pointerDrag.GetComponent<DragDropObject>();
        if (d != null)
        {
            d.transform.SetParent(transform);
            //GetComponent<CanvasGroup>().blocksRaycasts = true;
            d.transform.localPosition = Vector3.zero;
            d.defaultPosition = transform.position;
            d.parent = transform;
        }

    }
}