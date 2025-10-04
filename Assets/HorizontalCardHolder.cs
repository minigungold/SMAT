using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HorizontalCardHolder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private InteractionCarte selectedCard;
    public InteractionCarte hoveredCard;
    public GameObject slotPrefab;

    public int cardsToSpawn = 5;

    public List<InteractionCarte> interactionCartes = new List<InteractionCarte>();

    bool isCrossing = false;

    void Start()
    {

        for (int i = 0; i < cardsToSpawn; i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(this.transform);

            InteractionCarte interactionCarte = instance.GetComponentInChildren<InteractionCarte>();

            //Change le nom du gameObject
            interactionCarte.gameObject.name = i.ToString();

            // Ajout des Event Listeners pour suivre les évènements de chaque cartes
            interactionCarte.BeginDragEvent.AddListener(BeginDrag);
            interactionCarte.EndDragEvent.AddListener(EndDrag);


            interactionCartes.Add(interactionCarte);


        }
    }

    // Update is called once per frame
    void Update()
    {

        if (selectedCard == null)
            return;

        if (isCrossing)
            return;

        for (int i = 0; i < interactionCartes.Count; i++)
        {

            if (selectedCard.transform.position.x > interactionCartes[i].transform.position.x)
            {
                if(selectedCard.ParentIndex() > interactionCartes[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }

        }

    }

    void Swap(int index)
    {
        isCrossing = true;

        Transform focusedParent = selectedCard.transform.parent;
        Transform crossedParent = selectedCard.transform.parent;
    }

    private void BeginDrag(InteractionCarte interactionCarte)
    {
        selectedCard = interactionCarte;

    }

    void EndDrag(InteractionCarte interactionCarte)
    {
        selectedCard = null;
    }

    void CardPointerEnter(InteractionCarte interactionCarte)
    {
        hoveredCard = interactionCarte;
    }

    void CardPointerExit(InteractionCarte interactionCarte)
    {
        hoveredCard = null;
    }


}
