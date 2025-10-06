using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractionCarte : MonoBehaviour, 
    IDragHandler, IBeginDragHandler, IEndDragHandler, 
    IPointerEnterHandler, IPointerExitHandler//, IPointerUpHandler, 
    //IPointerDownHandler
{
    [HideInInspector] public UnityEvent<InteractionCarte> PointerEnterEvent;
    [HideInInspector] public UnityEvent<InteractionCarte> PointerExitEvent;
    [HideInInspector] public UnityEvent<InteractionCarte> BeginDragEvent;
    [HideInInspector] public UnityEvent<InteractionCarte> EndDragEvent;

    private RectTransform rectTransform;

    public bool selected;
    public float selectionOffset = 50;

    public bool isHovering;
    public bool isDragging;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragEvent.Invoke(this);
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDragEvent.Invoke(this);
        isDragging = false;
        this.transform.localPosition = Vector2.zero;
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    throw new System.NotImplementedException();
    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent.Invoke(this);
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerEnterEvent.Invoke(this);
        isHovering = false;
    }


    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    throw new System.NotImplementedException();
    //}

    public int SiblingAmount()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.parent.childCount - 1 : 0;
    }
    public int ParentIndex()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
    }

}
