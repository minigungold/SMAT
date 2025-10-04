using UnityEngine;

public class Carte : MonoBehaviour
{
    private int Num1;
    public int num1
    {
        get => Num1;
        set => Num1 = value;
    }
    private int Num2;
    public int num2
    {
        get => Num2;
        set => Num2 = value;
    }

    Carte(int num1, int num2)
    {
        Num1 = num1;
        Num2 = num2;
    }
}
