using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public static string code = "UNSET";
    public static bool codeSet;

    public static bool isPilot;

    [SerializeField]
    string nextScene;

    [SerializeField]
    TextMeshProUGUI codeText;
    [SerializeField]
    TextMeshProUGUI roleText;

    [SerializeField]
    Button pilotButton;
    [SerializeField]
    Button navigatorButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pilotButton.onClick.AddListener(PilotButton);
        navigatorButton.onClick.AddListener(NavigatorButton);

        codeText.text = code;
        if (codeSet)
            NavigatorButton();
        else
            PilotButton();
    }

    void PilotButton()
    {
        isPilot = true;
        pilotButton.interactable = false;
        navigatorButton.interactable = true;
        roleText.text = "PILOT";
    }
    void NavigatorButton()
    {
        isPilot = false;
        pilotButton.interactable = true;
        navigatorButton.interactable = false;
        roleText.text = "NAVIGATOR";
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
