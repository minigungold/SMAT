using UnityEngine;

public class UICollisionDetector : MonoBehaviour
{
    [SerializeField] private InteractionCarte card;
    [SerializeField] private CardVisual cardVisual;
    [SerializeField] public GameObject targetObject;

    [SerializeField] private float dist = 2f;
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
        Debug.Log("UI Element collided with: " + other.gameObject.name);
        if (card.isDragging && other.gameObject.GetComponent<PlayingCardSlot>())
        {
            card.isPlayable = true;
            targetObject = other.gameObject;
            card.playingSlotTransform = other.transform;
            card.cardVisual.playingSlotTransform = other.transform;
            //other.GetComponent<PlayingCardSlot>().currentCardObject = card.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayingCardSlot>())
        {
            PlayingCardSlot playingCardSlot = other.gameObject.GetComponent<PlayingCardSlot>();
            if (playingCardSlot.currentCardObject == this)
            {
                playingCardSlot.isOccupied = false;
                playingCardSlot.currentCardObject = null;
            }
        }
        card.isPlayable = false;
        targetObject = null;
        card.playingSlotTransform = null;
        card.cardVisual.playingSlotTransform = null;
    }
}


