using UnityEngine;

public class UICollisionDetector : MonoBehaviour
{
    [SerializeField] private InteractionCarte card;
    [SerializeField] private CardVisual cardVisual;
    public GameObject targetObject;

    //[SerializeField] private float dist = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        card = GetComponent<InteractionCarte>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + (dist - 1)), transform.TransformDirection(Vector2.up), dist);

        //if (card.isDragging)
        //{
        //    Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + (dist - 1)), transform.TransformDirection(Vector2.up) * dist, Color.yellow);

        //    if (hit.collider != null && hit.collider.GetComponent<PlayingCardSlot>())
        //        Debug.Log(hit.collider.name);
        //}

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("UI Element collided with: " + other.gameObject.name);
        if (card.isDragging && other.gameObject.GetComponent<PlayingCardSlot>())
        {
            card.isPlayable = true;
            targetObject = other.gameObject;
            card.playingSlotTransform = other.transform;
            card.playingCardSlot = other.GetComponent<PlayingCardSlot>();
            card.cardVisual.playingSlotTransform = other.transform;
            //other.GetComponent<PlayingCardSlot>().currentCardObject = card.gameObject;
        }

        //if (card.isPlaced && other.gameObject.GetComponent<PlayingCardSlot>())
        //{
        //    if (other.gameObject.GetComponent<PlayingCardSlot>() != card.playingCardSlot)
        //    {
        //        Debug.Log(other.gameObject.name);
        //        other.gameObject.SetActive(false);
        //        other.GetComponentInParent<Intersection>().DeactivateCardSlots();
        //    }
        //}
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayingCardSlot>())
        {
            PlayingCardSlot playingCardSlot = other.gameObject.GetComponent<PlayingCardSlot>();
            if (playingCardSlot.currentCardObject == this)
            {
                playingCardSlot.currentCardObject = null;
            }
        }
        if (card.isPlaced) return;
        card.isPlayable = false;
        targetObject = null;
        card.playingSlotTransform = null;
        card.playingCardSlot = null;
        card.cardVisual.playingSlotTransform = null;
    }
}


