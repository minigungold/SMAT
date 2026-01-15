using UnityEngine;

public class Intersection : MonoBehaviour
{
    //private GameObject cardSlot;

    public CardSlot haut;
    public CardSlot bas;
    public CardSlot gauche;
    public CardSlot droite;

    private void Awake()
    {
        transform.SetSiblingIndex(0);
    }

    public void DeactivateCardSlots()
    {
        haut.gameObject.SetActive(false);
        bas.gameObject.SetActive(false);
        gauche.gameObject.SetActive(false);
        droite.gameObject.SetActive(false);
    }
    public void DeactivateCardSlots(CardSlot cardslot)  //Désactive tout les cardSlots sauf celui opposé à la carte placée
    {
        haut.gameObject.SetActive(false);
        bas.gameObject.SetActive(false);
        gauche.gameObject.SetActive(false);
        droite.gameObject.SetActive(false);

        cardslot.gameObject.SetActive(true);
    }

    public void ActivateCardSlots()
    {
        haut.gameObject.SetActive(true);
        bas.gameObject.SetActive(true);
        gauche.gameObject.SetActive(true);
        droite.gameObject.SetActive(true);
    }

    public Intersection()
    {
        //haut = Instantiate(cardSlot).GetComponent<CardSlot>();
        //bas = Instantiate(cardSlot).GetComponent<CardSlot>();
        //gauche = Instantiate(cardSlot).GetComponent<CardSlot>();
        //droite = Instantiate(cardSlot).GetComponent<CardSlot>();
    }
    public Intersection(Carte carteBase, Intersection intersectionBase)
    {
        //haut = Instantiate(cardSlot).GetComponent<CardSlot>();
        //bas = Instantiate(cardSlot).GetComponent<CardSlot>();
        //gauche = Instantiate(cardSlot).GetComponent<CardSlot>();
        //droite = Instantiate(cardSlot).GetComponent<CardSlot>();

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
