using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractionCarte : MonoBehaviour, 
    IDragHandler, IBeginDragHandler, IEndDragHandler//, 
    //IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, 
    //IPointerDownHandler
{
    public UnityEvent<InteractionCarte> BeginDragEvent;
    public UnityEvent<InteractionCarte> EndDragEvent;

    private RectTransform rectTransform;
    public bool isDragging = false;
    public bool selected;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public int ParentIndex()
    {
        return transform.GetSiblingIndex();
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

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    throw new System.NotImplementedException();
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    throw new System.NotImplementedException();
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    throw new System.NotImplementedException();
    //}

}
