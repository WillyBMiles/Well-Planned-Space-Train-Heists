using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NavigatorUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI timerText;

    [SerializeField]
    CanvasGroup mainGroup;

    [SerializeField]
    TextMeshProUGUI showText;
    [SerializeField]
    TextMeshProUGUI bottomText;

    [SerializeField]
    Button resetButton;

    public event Action OnResetView;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resetButton.onClick.AddListener( ResetView);
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = string.Format(@"{0:m\:ss}", GameController.Instance.TimeLeft);
    }

    public void ShowText(string text, string bottomText)
    {
        if (showText.text == "")
        {
            mainGroup.DOFade(1f, .4f);
        }
        showText.text = text;
        this.bottomText.text = bottomText;

        resetButton.gameObject.SetActive(true);
    }
    public void ResetView()
    {
        OnResetView?.Invoke();
    }


    public void HideText()
    {
        mainGroup.DOFade(0f, .4f);
        showText.text = "";
        bottomText.text = "";
        resetButton.gameObject.SetActive(false);
    }
}
