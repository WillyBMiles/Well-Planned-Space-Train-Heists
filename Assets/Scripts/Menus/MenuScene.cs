using DG.Tweening;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    public static bool OnPc;

    [SerializeField]
    string lobbyScene;
    [SerializeField]
    CanvasGroup lobbyPopup;
    [SerializeField]
    TMPro.TMP_InputField inputField;

    [SerializeField]
    Button pcButton;

    private void Start()
    {
        ObjectPool.FlushPool();
        inputField.characterLimit = 6;
        inputField.characterValidation = TMPro.TMP_InputField.CharacterValidation.Alphanumeric;
        Application.targetFrameRate = 30;


        if (OnPc)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        try
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
        }
        catch
        {

        }
        var text = pcButton.GetComponentInChildren<TextMeshProUGUI>();
        text.text = OnPc ? "I'm currently on PC\n<size=50%>tap here to change" : "I'm currently on Phone\n<size=50%>Click here to change";
        pcButton.onClick.AddListener(() =>
        {
            OnPc = !OnPc;
            var text = pcButton.GetComponentInChildren<TextMeshProUGUI>();
            text.text = OnPc ? "I'm currently on PC\n<size=50%>tap here to change" : "I'm currently on Phone\n<size=50%>Click here to change";
        });
    }

    public void CreateCode()
    {
        StringBuilder codeStringBuilder = new();
        for (int i = 0; i < 6; i++)
        {
            codeStringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"[Random.Range(0, 36)]);
        }

        LobbyController.code = codeStringBuilder.ToString();
        LobbyController.codeSet = false;
        SceneManager.LoadScene(lobbyScene);
    }

    public void EnterCode()
    {
        lobbyPopup.DOFade(1f, .25f);
        lobbyPopup.blocksRaycasts = true;
    }

    public void ConfirmCode()
    {

        if (inputField.text.ToUpper().Length != 6)
        {
            return;
        }
        LobbyController.code = inputField.text.ToUpper();
        LobbyController.codeSet = true;

        SceneManager.LoadScene(lobbyScene);
    }

    public void BackToMenu()
    {
        lobbyPopup.DOFade(0f, .25f);
        lobbyPopup.blocksRaycasts = false;

    }
}
