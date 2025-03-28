using UnityEngine;
using System.Collections.Generic;

public class RewardCube : MonoBehaviour, IPoolable
{
    LineRenderer lineRenderer;
    [SerializeField]
    AudioClip clip;

    public void ResetPoolable()
    {
        //pass for now
        //score = 0;

        lineRenderer.positionCount = 0;
        positions = new()
        {
            new(),
            new()
        };
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        ResetPoolable();
    }

    const float DISAPPEAR_RANGE = 300f * 300f;
    const float TRACTOR_RANGE = 10f * 10f;
    const float TRACTOR_SPEED = 5f;

    List<Vector3> positions = new();
    private void Update()
    {
        float dist = Vector3.SqrMagnitude(transform.position - PilotController.Instance.transform.position);
        if (dist > DISAPPEAR_RANGE)
        {
            gameObject.SetActive(false);
            return;
        }
        if (dist < TRACTOR_RANGE)
        {
            transform.position = Vector3.Lerp(transform.position, PilotController.Instance.transform.position, Time.deltaTime * TRACTOR_SPEED);
            positions[0] = transform.position;
            positions[1] = PilotController.Instance.transform.position - PilotController.Instance.transform.up;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, positions[0]);
            lineRenderer.SetPosition(1, positions[1]);
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
        if (dist < 1f)
        {
            PickUp();
            gameObject.SetActive(false);
        }
    }

    void PickUp()
    {
        GameController.Instance.EarnScore(1);
        AudioSource.PlayClipAtPoint(clip, transform.position, .4f);
    }
}
