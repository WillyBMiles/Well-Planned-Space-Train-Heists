using TMPro;
using UnityEngine;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField]
    CanvasGroup pilot;
    [SerializeField]
    TextMeshProUGUI pilotText;
    [SerializeField, TextArea]
    string pilotTextFormat;

    [SerializeField]
    CanvasGroup navigator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (MenuScene.OnPc)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        if (LobbyController.isPilot)
        {
            pilot.alpha = 1f;
            navigator.alpha = 0f;
            pilotText.text = string.Format(pilotTextFormat, GameController.Score);
            
        }
        else
        {
            pilot.alpha = 0f;
            navigator.alpha = 1f;
        }
    }

}
