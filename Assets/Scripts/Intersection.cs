using UnityEngine;

public class Intersection : MonoBehaviour
{
    [SerializeField]
    private GameObject CardSlot;

    public CardSlot haut;
    public CardSlot bas;
    public CardSlot gauche;
    public CardSlot droite;

    public Intersection()
    {
        haut = Instantiate(CardSlot).GetComponent<CardSlot>();
        bas = Instantiate(CardSlot).GetComponent<CardSlot>();
        gauche = Instantiate(CardSlot).GetComponent<CardSlot>();
        droite = Instantiate(CardSlot).GetComponent<CardSlot>();
    }
    public Intersection(Carte Base)
    {
        haut = Instantiate(CardSlot).GetComponent<CardSlot>();
        bas = Instantiate(CardSlot).GetComponent<CardSlot>();
        gauche = Instantiate(CardSlot).GetComponent<CardSlot>();
        droite = Instantiate(CardSlot).GetComponent<CardSlot>();
    }
}
