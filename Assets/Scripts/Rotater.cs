using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Rotater : MonoBehaviour
{
    public static bool Rotated;

    [SerializeField]
    Button toggleButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toggleButton.onClick.AddListener( Toggle );

        if (Screen.width < Screen.height && !Rotated)
        {
            Toggle();
        }
    }


    void Toggle()
    {
        Rotated = !Rotated;
    }
}
