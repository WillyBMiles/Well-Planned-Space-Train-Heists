using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlignmentSceneUIController : MonoBehaviour
{
    [SerializeField]
    CanvasGroup alignGyroCanvasGroup;

    [SerializeField]
    CanvasGroup readyCanvasGroup;


    [SerializeField] string pilotScene;
    [SerializeField] string navigatorScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAlignGyro()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(alignGyroCanvasGroup.DOFade(0f, .25f));
        sequence.AppendCallback(() => alignGyroCanvasGroup.blocksRaycasts = false);
        sequence.Append(readyCanvasGroup.DOFade(1f, .25f));
        sequence.AppendCallback(() => readyCanvasGroup.blocksRaycasts = true);
    }

    public void OnGo()
    {
        if (LobbyController.isPilot)
        {
            SceneManager.LoadScene(pilotScene);
        }
        else
        {
            SceneManager.LoadScene(navigatorScene);
        }
        
    }
}
