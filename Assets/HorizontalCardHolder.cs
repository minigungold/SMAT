using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class HorizontalCardHolder : MonoBehaviour
{

    [SerializeField] private InteractionCarte selectedCard;
    [SerializeReference] private InteractionCarte hoveredCard;

    [SerializeField] private GameObject slotPrefab;
    private RectTransform rect;


    [SerializeField] private int cardsToSpawn = 7;
    public List<InteractionCarte> cards;

    bool isCrossing = false;

    void Start()
    {

        for (int i = 0; i < cardsToSpawn; i++)
        {
            Instantiate(slotPrefab, transform);
        }

        rect = GetComponent<RectTransform>();
        cards = GetComponentsInChildren<InteractionCarte>().ToList();

        int cardCount = 0;

        foreach (InteractionCarte interactionCarte in cards)
        {
            // Ajout des Event Listeners pour suivre les évènements de chaque cartes
            interactionCarte.PointerEnterEvent.AddListener(CardPointerEnter);
            interactionCarte.PointerExitEvent.AddListener(CardPointerExit);
            interactionCarte.BeginDragEvent.AddListener(BeginDrag);
            interactionCarte.EndDragEvent.AddListener(EndDrag);

            //Change le nom de la carte
            interactionCarte.name = cardCount.ToString();
            cardCount++;
        }


       /*for (int i = 0; i < cardsToSpawn; i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(this.transform);

            InteractionCarte interactionCarte = instance.GetComponentInChildren<InteractionCarte>();

            //Change le nom du gameObject
            interactionCarte.gameObject.name = i.ToString();

            // Ajout des Event Listeners pour suivre les évènements de chaque cartes
            interactionCarte.PointerEnterEvent.AddListener(CardPointerEnter);
            interactionCarte.PointerEnterEvent.AddListener(CardPointerExit);
            interactionCarte.BeginDragEvent.AddListener(BeginDrag);
            interactionCarte.EndDragEvent.AddListener(EndDrag);


            cards.Add(interactionCarte);
        }*/




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

    // Update is called once per frame
    void Update()
    {

        if (selectedCard == null)
            return;

        if (isCrossing)
            return;

        for (int i = 0; i < cards.Count; i++)
        {

            if (selectedCard.transform.position.x > cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() < cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }

            if (selectedCard.transform.position.x < cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() > cards[i].ParentIndex())
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
        Transform crossedParent = cards[index].transform.parent;

        cards[index].transform.SetParent(focusedParent);
        cards[index].transform.localPosition = cards[index].selected ? new Vector3(0, cards[index].selectionOffset, 0) : Vector3.zero;
        selectedCard.transform.SetParent(crossedParent);

        isCrossing = false;

        bool swapIsRight = cards[index].ParentIndex() > selectedCard.ParentIndex();
    }




}
