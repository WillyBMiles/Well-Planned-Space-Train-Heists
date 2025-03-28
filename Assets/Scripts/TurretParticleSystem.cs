using UnityEngine;

public class TurretParticleSystem : MonoBehaviour
{
    Turret parent;
    private void Awake()
    {
        parent = GetComponentInParent<Turret>();
    }


    [SerializeField]
    float maxHitTimer = .5f;
    float lastHitTime = int.MinValue;

    void OnParticleCollision(GameObject other)
    {
        if (lastHitTime + maxHitTimer > Time.time)
        {
            return;
        }
        if (other == PilotController.Instance.gameObject)
        {
            lastHitTime = maxHitTimer;
        }
        parent.Hit(other);
    }
}
