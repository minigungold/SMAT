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
        grid.Add(new Vector2(0, 0), firstIntersection);

        //cardHolder.InstantiateCard();

        //Instancier les premières cartes;
        InstantiateCard(firstIntersection, firstIntersection.gauche);
        InstantiateCard(firstIntersection, firstIntersection.droite);


        //distribue les premieres cartes
        firstIntersection.gauche.Carte = deck.Piger();
        firstIntersection.droite.Carte = deck.Piger();
    }

    public void InstantiateCard(Intersection intersection, CardSlot cardSlot)
    {
        GameObject cardGameObject = Instantiate(slotPrefab, placedCards.transform);

        cardGameObject.GetComponentInParent<RectTransform>().position = cardSlot.transform.position;
        cardGameObject.GetComponentInChildren<InteractionCarte>().isPlaced = true;
        cardGameObject.GetComponentInChildren<InteractionCarte>().playingSlotTransform = intersection.transform;
        cardGameObject.GetComponentInChildren<InteractionCarte>().zRotation = cardSlot.transform.rotation.eulerAngles.z;

        cardSlot.GetComponent<PlayingCardSlot>().isOccupied = true;
        cardSlot.GetComponent<PlayingCardSlot>().currentCardObject = cardGameObject.transform.GetChild(0).gameObject;
        cardSlot.GetComponent<PlayingCardSlot>().enabled = false;
        cardSlot.GetComponent<BoxCollider2D>().enabled = false;
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
        playedCard.playingCardSlot.GetComponent<BoxCollider2D>().enabled = false;
        playedCard.cardVisual.canvas.overrideSorting = true;
        playedCard.cardVisual.canvas.sortingLayerID = -1;

        cardHolder.DisableCard(cardHolder.playedCard);
        cardHolder.cards.Remove(cardHolder.playedCard);
        //Destroy(parent.gameObject);
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
