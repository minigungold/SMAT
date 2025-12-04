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
        grid.Add(new Vector2(0, 0), new Intersection());

    }


    //A FAIRE, faire spawner un go et donner la bonne position
    public void placeCarte(Vector2 basePos, Carte baseCarte)
    {
        

        Intersection baseIntersection;
        grid.TryGetValue(basePos, out baseIntersection);
        //erreur I guess
        if (baseIntersection == null)
        {
            return;
        }

        if (baseIntersection.bas.carte == baseCarte)
        {
            Vector2 newPos = new Vector2(basePos.x, basePos.y - 1);
           
            if (grid.ContainsKey(newPos))
            {
                return;
            }
            Intersection temp = new Intersection();
            temp.haut.carte = baseCarte;
            grid.Add(newPos, temp);

        }
        else if (baseIntersection.haut.carte == baseCarte)
        {

        }

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
