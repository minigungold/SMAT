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
    public Intersection(Carte carteBase, Intersection intersectionBase)
    {
        haut = Instantiate(CardSlot).GetComponent<CardSlot>();
        bas = Instantiate(CardSlot).GetComponent<CardSlot>();
        gauche = Instantiate(CardSlot).GetComponent<CardSlot>();
        droite = Instantiate(CardSlot).GetComponent<CardSlot>();

        //met la carte de base dans l'intersection approprie selon ou est place la carte qui la cree
        if(intersectionBase.bas.Carte == carteBase)
        {
            haut.Carte = carteBase;
        }
        else if (intersectionBase.gauche.Carte == carteBase)
        {
            droite.Carte = carteBase;
        }
        else if (intersectionBase.droite.Carte == carteBase)
        {
             gauche.Carte = carteBase;
        }
        else
        {
            bas.Carte = carteBase;
        }

    }
}
