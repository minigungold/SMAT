using UnityEngine;

public class UICollisionDetector : MonoBehaviour
{
    [SerializeField] private InteractionCarte carte;
    [SerializeField] public GameObject targetObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carte = GetComponent<InteractionCarte>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("UI Element collided with: " + other.gameObject.name);
        if (carte.isDragging)
        {
            carte.isPlayable = true;
            targetObject = other.gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        carte.isPlayable = false;
        targetObject = null;
    }

}
