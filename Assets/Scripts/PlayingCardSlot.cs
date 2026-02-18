using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayingCardSlot : MonoBehaviour
{

    public bool isOccupied;
    public GameObject currentCardObject;

    public void ResetCardSlot()
    {
        isOccupied = false;
        currentCardObject = null;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    //private void FixedUpdate()
    //{
    //    if (isOccupied)
    //    {
    //        GetComponent<Canvas>().overrideSorting = false;
    //    }
    //}

    //void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.gameObject.GetComponent<InteractionCarte>())
    //    {
    //        if (this.transform.GetSiblingIndex() > other.transform.GetSiblingIndex())
    //        {
    //            other.gameObject.SetActive(false);

    //            other.GetComponentInParent<Intersection>().DeactivateCardSlots();
    //        }


    //    }


    //    //Debug.Log("touching " + other.gameObject.name);
    //}

}
