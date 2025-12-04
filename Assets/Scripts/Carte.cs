using UnityEngine;

public class Carte
{
    /// <summary>
    /// Nombre en haut de la carte
    /// </summary>
    private int Num1;
    public int num1
    {
        get => Num1;
        set => Num1 = value;
    }
    /// <summary>
    /// nombre en bas de la carte
    /// </summary>
    private int Num2;
    public int num2
    {
        get => Num2;
        set => Num2 = value;
    }
       
    public Carte(int num1, int num2)
    {
        Num1 = num1;
        Num2 = num2;
    }
}
