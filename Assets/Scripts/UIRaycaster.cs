using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRaycaster : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; // Assign your main camera in the Inspector
    [SerializeField] private GraphicRaycaster uiRaycaster; // Assign the Graphic Raycaster from your Canvas
    [SerializeField] private InteractionCarte carte;
    [HideInInspector] public GameObject targetObject;
    private void Start()
    {
        mainCamera = Camera.main;
        uiRaycaster = GetComponentInParent<GraphicRaycaster>();
        carte = GetComponent<InteractionCarte>();
    }

    void Update()
    {
        if (carte.isDragging)
        {
            // Define the origin and direction of your ray from the 3D object
            Ray ray = new Ray(transform.position, transform.forward);

            // Convert the ray origin to screen space for UI interaction
            Vector2 screenPoint = mainCamera.WorldToScreenPoint(ray.origin);

            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = screenPoint;

            List<RaycastResult> results = new List<RaycastResult>();
            uiRaycaster.Raycast(pointerData, results);

            // Process the results
            foreach (RaycastResult result in results)
            {

                if (result.gameObject.GetComponent<PlayingCardSlot>())
                {
                    carte.isPlayable = true;
                    StartCoroutine(FrameWait());
                    // Wait for the end of frame to update isPlayable value to false (Prevents it from instantly changing in succession)
                    IEnumerator FrameWait()
                    {
                        yield return new WaitForEndOfFrame();
                        carte.isPlayable = false;
                    }

                }
            }
            if (results.Count > 1) results.RemoveAt(0);
            Debug.Log(results.ToString());
            Debug.Log(results.Count);

        }

    }
}