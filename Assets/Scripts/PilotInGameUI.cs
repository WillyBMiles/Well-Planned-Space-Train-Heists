using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PilotInGameUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreText;

    [SerializeField]
    TextMeshProUGUI locationText;
    [SerializeField]
    TextMeshProUGUI headingText;

    [SerializeField]
    TextMeshProUGUI timerText;


    [SerializeField]
    CanvasGroup DamageCanvasGroup;

    [SerializeField]
    CanvasGroup disabledCanvasGroup;
    [SerializeField]
    Image rebootImage;
    [SerializeField]
    Image healthImage;

    [SerializeField]
    Button shootButton;

    [SerializeField]
    Button warpButton;



    int localScore = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameController.Instance.OnScoreChange += OnScoreChange;
        PilotController.Instance.OnHit += PilotHit;

        shootButton.onClick.AddListener(PilotController.Instance.Shoot);
        warpButton.onClick.AddListener(PilotController.Instance.Warp);
    }

    Sequence hitSequence;
    private void PilotHit()
    {
        if (hitSequence != null && hitSequence.IsPlaying())
            hitSequence.Kill();
        DamageCanvasGroup.alpha = 1f;
        hitSequence = DOTween.Sequence();
        hitSequence.AppendInterval(1f);
        hitSequence.Append(DamageCanvasGroup.DOFade(0f, 1f));
        hitSequence.Play();
    }

    Sequence previousScoreChange;
    private void OnScoreChange(int score)
    {
        if (previousScoreChange != null && previousScoreChange.IsPlaying())
        {
            previousScoreChange.Kill();
        }
        previousScoreChange = DOTween.Sequence();
        previousScoreChange.Append(scoreText.transform.DOScale(1.1f, .5f));
        previousScoreChange.Join(DOTween.To(() => localScore, (value) =>
        {
            localScore = value;
            scoreText.text = localScore.ToString();
        }, score, 1f));
        previousScoreChange.Append(scoreText.transform.DOScale(1, .5f));

        previousScoreChange.Play();
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = string.Format(@"{0:m\:ss}", GameController.Instance.TimeLeft);

        if (PilotController.Instance.CurrentRebootTimer > 0f)
        {
            rebootImage.fillAmount = 1 - PilotController.Instance.CurrentRebootTimer / PilotController.Instance.RebootTime;
            disabledCanvasGroup.alpha = 1f;
        }
        else
        {
            disabledCanvasGroup.alpha = 0f;
        }
        locationText.text = WorldModelInstance.Instance.WhereAmIString(PilotController.Instance.transform.position);
        headingText.text = "Toward: " + GetHeading(PilotController.Instance.transform.forward);

        healthImage.transform.localScale = new Vector3(Mathf.Max(0f, ((float) PilotController.Instance.Health) / PilotController.Instance.MaxHealth), 1f, 1f);
    }

    string GetHeading(Vector3 input)
    {
        float max = Mathf.Max(Mathf.Abs(input.x), Mathf.Abs(input.y), Mathf.Abs(input.z));
        if (max == Mathf.Abs(input.x))
        {
            return input.x > 0 ? "5, , " : "1, , "; 
        }
        if (max == Mathf.Abs(input.y))
        {
            return input.y > 0 ? ", E, " : ", A, ";
        }
        else
        {
            return input.z > 0 ? ", , V" : ", , I";
        }
    }
}
