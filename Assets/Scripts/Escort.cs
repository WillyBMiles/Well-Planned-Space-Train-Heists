using UnityEngine;

public class Escort : MonoBehaviour, IPoolable
{
    [SerializeField] Enemy enemy;
    [SerializeField] float chaseRange;
    [SerializeField] float moveSpeed;
// Behavior:
// Follow a train or a line
// When encountering a player shoot at them while chasing until within 25 m
// Then sit still and shoot



    // Update is called once per frame
    void Update()
    {
        if (!GameController.Instance.HasStarted)
        {
            return;
        }
        transform.position += moveSpeed * Time.deltaTime * transform.forward;
        if (Vector3.Distance(transform.position, PilotController.Instance.transform.position) > 300f)
        {
            gameObject.SetActive(false);
        }
        if (Vector3.Distance(transform.position, PilotController.Instance.transform.position) < 2.5f)
        {
            PilotController.Instance.HitMe(100);
        }
    }

    public void ResetPoolable()
    {
        enemy.ResetPoolable();
    }


    public void ShouldChangeStrategies()
    {
        if (PilotController.Instance == null)
            return;
    }

    private void OnDisable()
    {
        if (PilotController.Instance == null)
            return;
        ResetPoolable();
        transform.position = PilotController.Instance.transform.position + PilotController.Instance.transform.forward * 250f;
        transform.forward =  PilotController.Instance.transform.position - transform.position;
        Invoke(nameof(Activate), .5f);
    }
    void Activate()
    {
        if (this == null)
            return;
        gameObject.SetActive(true);
    }
}
