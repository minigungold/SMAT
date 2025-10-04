using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

public class Deck : MonoBehaviour
{
    private List<Carte> Cartes;
    public List<Carte> cartes
    {
        get => Cartes; set => Cartes = value;
    }

    internal Stack<Carte> Default;

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
}
