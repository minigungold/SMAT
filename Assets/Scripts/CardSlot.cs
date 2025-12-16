using UnityEngine;

public class CardSlot : MonoBehaviour
{
    private Carte carte;
    public Carte Carte
    {
        get {  return carte; }
        set
        {
            if (carte != null)
            {
                carte = value;
                Selectionable = false;
            }

        }
    }



    public bool Selectionable = true;
}
