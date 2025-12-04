using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayingCardSlot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{

    public bool isPlayable;

    [Header("Events")]
    [HideInInspector] public UnityEvent<InteractionCarte> PointerEnterEvent;
    [HideInInspector] public UnityEvent<InteractionCarte> PointerExitEvent;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isPlayable = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPlayable = false;
    }
}
