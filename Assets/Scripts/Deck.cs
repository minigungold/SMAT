using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

public class Deck
{
    private List<Carte> Cartes;
    public List<Carte> cartes
    {
        get => Cartes; set => Cartes = value;
    }
    private List<Carte> Discarte;

    internal List<Carte> DefaultDeck;

    /// <summary>
    /// Mélange le deck
    /// </summary>
    /// <remarks>Employs the Fisher-Yates algorithm for shuffling</remarks>
    public void Shuffle()
    {
        for (int i = Cartes.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (Cartes[i], Cartes[j]) = (Cartes[j], Cartes[i]);
        }
    }

    /// <summary>
    /// réinitialize le deck a la version par défault
    /// </summary>
    public void Reinitailize()
    {
        Cartes = DefaultDeck;
    }

    public void Discarter(Carte carte)
    {
        Discarte.Add(carte);
    }

    /// <summary>
    /// Pige une carte et enl'eve lacarte pigerdu deck
    /// </summary>
    /// <returns>la carte Piger</returns>
    public Carte Piger()
    {
        Carte carte = Cartes.Last();
        Cartes.Remove(carte);
        return carte;
    }

    public Deck()
    {
        Cartes = new List<Carte>();
        Discarte = new List<Carte>();
        DefaultDeck = new List<Carte>();
    }
}
