using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayingCardSlot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{

    public bool isPlayable;
    public bool isOccupied;

    [Header("Events")]
    [HideInInspector] public UnityEvent<InteractionCarte> PointerEnterEvent;
    [HideInInspector] public UnityEvent<InteractionCarte> PointerExitEvent;
    public GameObject currentCardObject;


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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<InteractionCarte>())
        {
            //isOccupied = true;
            //currentCardObject = collision.gameObject; 
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<InteractionCarte>())
        {
            //isOccupied = false;
            //currentCardObject = null;
        }
    }
}
