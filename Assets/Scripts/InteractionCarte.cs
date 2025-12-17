using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractionCarte : MonoBehaviour,
    IDragHandler, IBeginDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler,
    IPointerDownHandler
{
    private Canvas canvas;
    private Image imageComponent;
    [SerializeField] private bool instantiateVisual = true;
    private VisualCardsHandler visualHandler;
    private Vector3 offset;
    private UICollisionDetector collisionDetector;

    public Transform playingSlotTransform;
    public RectTransform rectTransform;

    [SerializeField] private float positionZ = 89f;

    [Header("Movement")]
    [SerializeField] private float moveSpeedLimit = 50;

    [Header("Selection")]
    public bool selected;
    public float selectionOffset = 50;
    private float pointerDownTime;
    private float pointerUpTime;

    [Header("Visual")]
    [SerializeField] private GameObject cardVisualPrefab;
    [HideInInspector] public CardVisual cardVisual;

    [Header("States")]
    public bool isHovering;
    public bool isDragging;
    [HideInInspector] public bool wasDragged;
    public bool isPlayable;
    public bool isPlaying;
    public bool isPlaced = false;

    [Header("Events")]
    [HideInInspector] public UnityEvent<InteractionCarte> PointerEnterEvent;
    [HideInInspector] public UnityEvent<InteractionCarte> PointerExitEvent;
    [HideInInspector] public UnityEvent<InteractionCarte, bool> PointerUpEvent;
    [HideInInspector] public UnityEvent<InteractionCarte> PointerDownEvent;
    [HideInInspector] public UnityEvent<InteractionCarte> BeginDragEvent;
    [HideInInspector] public UnityEvent<InteractionCarte> OnDragEvent;
    [HideInInspector] public UnityEvent<InteractionCarte> EndDragEvent;
    [HideInInspector] public UnityEvent<InteractionCarte, bool> SelectEvent;



    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        imageComponent = GetComponent<Image>();
        collisionDetector = GetComponent<UICollisionDetector>();
        rectTransform = GetComponent<RectTransform>();
        if (!instantiateVisual)
            return;

        visualHandler = FindFirstObjectByType<VisualCardsHandler>();
        cardVisual = Instantiate(cardVisualPrefab, visualHandler ? visualHandler.transform : canvas.transform).GetComponent<CardVisual>();
        cardVisual.Initialize(this);
    }

    void Update()
    {
        if (isPlaced && playingSlotTransform != null)
        {
            GetComponentInParent<Transform>().localPosition = new Vector3(playingSlotTransform.position.x, playingSlotTransform.position.y, positionZ);
            //transform.position = new Vector3(playingSlotTransform.position.x, playingSlotTransform.position.y, positionZ);
            cardVisual.transform.position = transform.position;
            gameObject.SetActive(false);
            return;
        }

        ClampPosition();

        if (isDragging && isPlaying == false)
        {
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            Vector2 velocity = direction * Mathf.Min(moveSpeedLimit, Vector2.Distance(transform.position, targetPosition) / Time.deltaTime);
            transform.Translate(velocity * Time.deltaTime);
        }

        if (isDragging == false && isPlaying && playingSlotTransform != null)
        {
            transform.position = new Vector3(playingSlotTransform.position.x, playingSlotTransform.position.y, positionZ);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            ReturnToHand();
        }
    }

    void ClampPosition()
    {
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);
        transform.position = new Vector3(clampedPosition.x, clampedPosition.y, positionZ); //89 pour éviter d'avoir 9000 en z (s'adapte avec le canvas position.z)
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragEvent.Invoke(this);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = mousePosition - (Vector2)transform.position;
        isDragging = true;
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
        imageComponent.raycastTarget = false;

        wasDragged = true;
        isPlaying = false; // Reset le isPlaying lorsque qu'on drag la carte
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent.Invoke(this);

        //rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDragEvent.Invoke(this);
        canvas.GetComponent<GraphicRaycaster>().enabled = true;
        imageComponent.raycastTarget = true;

        isDragging = false;             //Continuer Ici
        isPlaying = isPlayable ? true : false;

        GetComponentInParent<HorizontalCardHolder>().ReturnCardsToHand(this);
        ChangePlayedCard();

        StartCoroutine(FrameWait());

        IEnumerator FrameWait()
        {
            yield return new WaitForEndOfFrame();
            wasDragged = false;
        }

        if (selected)
        {
            transform.localPosition = Vector3.zero + (transform.up * selectionOffset);
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent.Invoke(this);
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitEvent.Invoke(this);
        isHovering = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        PointerDownEvent.Invoke(this);
        pointerDownTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        pointerUpTime = Time.time;

        PointerUpEvent.Invoke(this, pointerUpTime - pointerDownTime > .2f);

        if (pointerUpTime - pointerDownTime > .2f)
            return;

        if (wasDragged)
            return;

        if (isPlaying)
            return;

        selected = !selected;
        isPlayable = false;
        SelectEvent.Invoke(this, selected);

        if (selected)
            transform.localPosition += (cardVisual.transform.up * selectionOffset);
        else
            transform.localPosition = Vector3.zero;
    }

    public void Deselect()
    {
        if (selected)
        {
            selected = false;
            if (selected)
                transform.localPosition += (cardVisual.transform.up * 50);
            else
                transform.localPosition = Vector3.zero;
        }
    }

    public int SiblingAmount()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.parent.childCount - 1 : 0;
    }
    public int ParentIndex()
    {
        //Obtient l'index de l'objet par rapport aux autre dans PlayingCardGroup
        return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
    }

    public float NormalizedPosition()
    {
        return transform.parent.CompareTag("Slot") ? ExtensionMethods.Remap((float)ParentIndex(), 0, (float)(transform.parent.parent.childCount - 1), 0, 1) : 0;
    }

    public void ReturnToHand()
    {
        isDragging = false;
        isPlayable = false;
        isPlaying = false;

        cardVisual.cardImage.transform.rotation = cardVisual.tiltParent.rotation;

        if (selected)
        {
            transform.localPosition = Vector3.zero + (transform.up * selectionOffset);
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }

        if (collisionDetector.targetObject != null && collisionDetector.targetObject.GetComponent<PlayingCardSlot>())
        {
            PlayingCardSlot playingCardSlot = collisionDetector.targetObject.GetComponent<PlayingCardSlot>();


            playingCardSlot.currentCardObject = null;
            playingCardSlot.isOccupied = false;

        }
    }

    public void ChangePlayedCard()
    {
        if (collisionDetector.targetObject != null && collisionDetector.targetObject.GetComponent<PlayingCardSlot>())
        {
            PlayingCardSlot playingCardSlot = collisionDetector.targetObject.GetComponent<PlayingCardSlot>();

            //if (playingCardSlot.currentCardObject != this.gameObject && playingCardSlot.isOccupied)
            //{
            //    playingCardSlot.currentCardObject.GetComponent<InteractionCarte>().ReturnToHand();
            //    playingCardSlot.currentCardObject = null;
            //    playingCardSlot.isOccupied = false;
            //}

            playingCardSlot.currentCardObject = this.gameObject;
            playingCardSlot.isOccupied = true;
        }
    }

}
