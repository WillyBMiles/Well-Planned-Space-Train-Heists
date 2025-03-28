using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RectRotater : MonoBehaviour
{
    [SerializeField]
    bool testRotated;
    [SerializeField]
    bool reverseScaler;

    CanvasScaler scaler;
    Canvas canvas;

    private void Awake()
    {
        scaler = GetComponentInParent<CanvasScaler>();
        canvas = GetComponentInParent<Canvas>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
#if UNITY_EDITOR
        return;
#endif
#pragma warning disable CS0162 // Unreachable code detected
        testRotated = false;
#pragma warning restore CS0162 // Unreachable code detected
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rect = transform as RectTransform;
        if (Rotater.Rotated || testRotated)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            Vector2 sizeDelta = (canvas.transform as RectTransform).sizeDelta;
            rect.sizeDelta = new Vector2(sizeDelta.y, sizeDelta.x);
            scaler.matchWidthOrHeight = reverseScaler ? 0f : 1f;
        }
        else
        {
            rect.sizeDelta = new Vector2(500, 1000);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            rect.sizeDelta = (canvas.transform as RectTransform).sizeDelta;
            scaler.matchWidthOrHeight = reverseScaler ? 1f : 0f;

        }
    }
}
