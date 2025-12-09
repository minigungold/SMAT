using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class HorizontalCardHolder : MonoBehaviour
{

    [SerializeField] private InteractionCarte selectedCard;
    [SerializeReference] private InteractionCarte hoveredCard;

    [SerializeField] private GameObject slotPrefab;
    private RectTransform rect;

    [Header("Spawn Settings")]
    [SerializeField] private int cardsToSpawn = 7;
    public List<InteractionCarte> cards;
    private int cardCount = 0;

    bool isCrossing = false;
    [SerializeField] private bool tweenCardReturn = true;

    void Start()
    {

        for (int i = 0; i < cardsToSpawn; i++)
        {
            Instantiate(slotPrefab, transform);
        }

        rect = GetComponent<RectTransform>();
        cards = GetComponentsInChildren<InteractionCarte>().ToList();
        cardCount = 0;
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


        StartCoroutine(Frame());

        IEnumerator Frame()
        {
            yield return new WaitForSecondsRealtime(.1f);
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].cardVisual != null)
                    cards[i].cardVisual.UpdateIndex(transform.childCount);
            }
        }
    }



    public void InstantiateCard()
    {
        GameObject cardGameObject = Instantiate(slotPrefab, transform);

        InteractionCarte card = cardGameObject.GetComponentInChildren<InteractionCarte>();
        // Ajout des Event Listeners pour suivre les évènements de chaque cartes
        card.PointerEnterEvent.AddListener(CardPointerEnter);
        card.PointerExitEvent.AddListener(CardPointerExit);
        card.BeginDragEvent.AddListener(BeginDrag);
        card.EndDragEvent.AddListener(EndDrag);

        //Change le nom de la carte
        card.name = cardCount.ToString();
        cardCount++;

        cards.Add(card);
    }


    private void BeginDrag(InteractionCarte interactionCarte)
    {
        selectedCard = interactionCarte;
    }

    void EndDrag(InteractionCarte interactionCarte)
    {
        if(selectedCard == null) return;

        selectedCard.transform.DOLocalMove(selectedCard.selected ? new Vector3(0, selectedCard.selectionOffset, 0) : Vector3.zero, tweenCardReturn ? .15f : 0).SetEase(Ease.OutBack);

        rect.sizeDelta += Vector2.right;
        rect.sizeDelta -= Vector2.right;

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
        if (Input.GetKeyUp(KeyCode.Q))
        {
            InstantiateCard();
        }

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

        if (cards[index].cardVisual == null)
            return;

        bool swapIsRight = cards[index].ParentIndex() > selectedCard.ParentIndex();
        cards[index].cardVisual.Swap(swapIsRight ? -1 : 1);

        //Updated Visual Indexes
        foreach (InteractionCarte card in cards)
        {
            card.cardVisual.UpdateIndex(transform.childCount);
        }
    }




}
