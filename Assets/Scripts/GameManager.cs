using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Deck deck;
    public Dictionary<Vector2, Intersection> grid;

    public HorizontalCardHolder cardHolder;

    [SerializeField] private GameObject placedCards;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject intersectionPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject operatorSelector;

    public float offset1 = 2.00f;
    public float offset2 = 1.10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;

        deck = new Deck();
        grid = new Dictionary<Vector2, Intersection>();

        InitializeDefaultDeck();

        GameStart();

    }
    public void GameStart()
    {
        deck.Reinitailize();
        deck.Shuffle();
        Intersection firstIntersection = Instantiate(intersectionPrefab, canvas.transform).GetComponent<Intersection>();
        //grid.Add(new Vector2(0, 0), firstIntersection);

        //cardHolder.InstantiateCard();

        //Instancier les premières cartes;
        InstantiateCard(firstIntersection, firstIntersection.gauche);
        InstantiateIntersection(firstIntersection.gauche);

        InstantiateCard(firstIntersection, firstIntersection.droite);
        InstantiateIntersection(firstIntersection.droite);

        //distribue les premieres cartes
        firstIntersection.gauche.Carte = deck.Piger();
        firstIntersection.droite.Carte = deck.Piger();

        
    }

    public void InstantiateCard(Intersection intersection, CardSlot cardSlot)
    {
        GameObject cardGameObject = Instantiate(slotPrefab, placedCards.transform);
        InteractionCarte interactionCarte = cardGameObject.GetComponentInChildren<InteractionCarte>();
        PlayingCardSlot playingCardSlot = cardSlot.GetComponent<PlayingCardSlot>();

        cardGameObject.GetComponentInParent<RectTransform>().position = cardSlot.transform.position;

        interactionCarte.isPlaced = true;
        interactionCarte.playingSlotTransform = intersection.transform;
        interactionCarte.zRotation = cardSlot.transform.rotation.eulerAngles.z;

        interactionCarte.playingCardSlot = playingCardSlot;

        playingCardSlot.isOccupied = true;
        playingCardSlot.currentCardObject = cardGameObject.transform.GetChild(0).gameObject;
        playingCardSlot.enabled = false;

        cardHolder.playedCard = interactionCarte;
        cardSlot.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void InstantiateIntersection(CardSlot cardSlot)
    {
        Intersection intersection = Instantiate(intersectionPrefab, canvas.transform).GetComponent<Intersection>();
        RectTransform rectTransform = intersection.GetComponentInParent<RectTransform>();

        string cardSlotName = cardSlot.name;

        Transform gaucheTransform = intersection.gauche.transform;
        Transform droiteTransform = intersection.droite.transform;
        Transform hautTransform = intersection.haut.transform;
        Transform basTransform = intersection.bas.transform;

        if (cardSlotName == "Haut" || cardSlotName == "Bas")   //Verticale
        {
            gaucheTransform.position = new Vector3(-offset1, gaucheTransform.position.y, gaucheTransform.position.z);
            droiteTransform.position = new Vector3(offset1, droiteTransform.position.y, droiteTransform.position.z);
            hautTransform.position = new Vector3(hautTransform.position.x, offset2, hautTransform.position.z);
            basTransform.position = new Vector3(basTransform.position.x, -offset2, basTransform.position.z);

            if (cardSlotName == "Haut")
            {
                rectTransform.transform.position = new Vector3(cardSlot.transform.position.x, cardSlot.transform.position.y + offset2, 90);
                intersection.bas.GetComponent<PlayingCardSlot>().currentCardObject = cardHolder.playedCard.gameObject;
                intersection.DeactivateCardSlots(intersection.haut);
                intersection.bas.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                rectTransform.transform.position = new Vector3(cardSlot.transform.position.x, cardSlot.transform.position.y - offset2, 90);
                intersection.haut.GetComponent<PlayingCardSlot>().currentCardObject = cardHolder.playedCard.gameObject;
                intersection.DeactivateCardSlots(intersection.bas);
                intersection.haut.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else if (cardSlotName == "Gauche")
        {
            rectTransform.transform.position = new Vector3(cardSlot.transform.position.x - offset2, cardSlot.transform.position.y, 90);
            intersection.droite.GetComponent<PlayingCardSlot>().currentCardObject = cardHolder.playedCard.gameObject;
            intersection.DeactivateCardSlots(intersection.gauche);
            intersection.droite.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            rectTransform.transform.position = new Vector3(cardSlot.transform.position.x + offset2, cardSlot.transform.position.y, 90);
            intersection.gauche.GetComponent<PlayingCardSlot>().currentCardObject = cardHolder.playedCard.gameObject;
            intersection.DeactivateCardSlots(intersection.droite);
            intersection.gauche.GetComponent<BoxCollider2D>().enabled = false;
        }



        Debug.Log(rectTransform.transform.position);
    }




    public void PlayCard()
    {

        InteractionCarte playedCard = cardHolder.playedCard;
        GameObject parent = cardHolder.playedCard.transform.parent.gameObject;

        parent.transform.SetParent(placedCards.transform, true);
        parent.transform.position = playedCard.playingSlotTransform.position;
        playedCard.isPlaced = true;
        playedCard.isPlaying = false;
        playedCard.isPlayable = false;

        //Désactiver le BoxCollider du cardSlot pour empêcher de mettre une carte en plus
        playedCard.playingCardSlot.GetComponent<BoxCollider2D>().enabled = false;

        //Override sorting layer de la carte pour qu'elle soit derrière la main du joueur
        //playedCard.cardVisual.canvas.overrideSorting = true;
        playedCard.cardVisual.transform.SetAsLastSibling();

        cardHolder.DisableCard(cardHolder.playedCard);
        cardHolder.cards.Remove(cardHolder.playedCard);
        //Destroy(parent.gameObject);

        playedCard.playingCardSlot.GetComponentInParent<Intersection>().ActivateCardSlots();

        //Active toutes les intersections touchants l'opération lorsqu'on place la carte
        InstantiateIntersection(playedCard.playingCardSlot.GetComponent<CardSlot>());

        cardHolder.playedCard = null;



    }

    public void placeCarte(Vector2 basePos, Carte baseCarte)
    {


        Intersection baseIntersection;
        grid.TryGetValue(basePos, out baseIntersection);
        //erreur I guess
        if (baseIntersection == null)
        {
            return;
        }

        Intersection temp = new Intersection();
        Vector2 newpos = basePos;
        if (baseIntersection.bas.Carte == baseCarte)
        {
            Vector2 newPos = new Vector2(basePos.x, basePos.y - 1);

            if (grid.ContainsKey(newPos))
            {
                return;
            }

            temp.haut.Carte = baseCarte;

        }
        else if (baseIntersection.haut.Carte == baseCarte)
        {
            Vector2 newPos = new Vector2(basePos.x, basePos.y + 1);

            if (grid.ContainsKey(newPos))
            {
                return;
            }

            temp.haut.Carte = baseCarte;
        }
        else if (baseIntersection.droite.Carte == baseCarte)
        {
            Vector2 newPos = new Vector2(basePos.x - 1, basePos.y);

            if (grid.ContainsKey(newPos))
            {
                return;
            }

            temp.gauche.Carte = baseCarte;
        }
        else
        {
            Vector2 newPos = new Vector2(basePos.x + 1, basePos.y);

            if (grid.ContainsKey(newPos))
            {
                return;
            }

            temp.haut.Carte = baseCarte;
        }
        grid.Add(newpos, temp);
    }
    private void InitializeDefaultDeck()
    {
        string filePath = Path.Combine(Application.dataPath + "/Cards/cards.csv");

        if (File.Exists(filePath))
        {
            List<List<string>> data = new List<List<string>>();

            using (StreamReader reader = new StreamReader(filePath))
            {

                reader.ReadLine();
                string line;


                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(',');
                    List<string> row = new List<string>(values);
                    data.Add(row);
                }
            }


            foreach (var line in data)
            {

                string sNum1 = line.ElementAt(0);
                string sNum2 = line.ElementAt(1);
                string sNumCopie = line.ElementAt(3);

                int numCopie = int.Parse(sNumCopie);

                for (int i = 0; i < numCopie; i++)
                {
                    Carte carte = new Carte(int.Parse(sNum1), int.Parse(sNum2));
                    deck.DefaultDeck.Add(carte);
                }

            }

        }

    }

}
