using UnityEngine;

public class Explosion : MonoBehaviour, IPoolable
{
    [SerializeField]
    ParticleSystem explosionParticleSystem;
    [SerializeField]
    int numberOfParticles;

    [SerializeField]
    float countdownToDisable;

    float countdown = 0;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public static void CreateExplosion(Vector3 position, Quaternion rotation, float size)
    {
        Explosion ex = ObjectPool.SpawnObject<Explosion>("Explosion");
        ex.transform.position = position;
        ex.transform.localScale = new Vector3(size, size, size);
        ex.transform.rotation = rotation;
        ex.explosionParticleSystem.Emit(ex.numberOfParticles);
        ex.countdown = ex.countdownToDisable;
        ex.animator.Rebind();
        ex.animator.Update(0);
    }

    public void ResetPoolable()
    {
        //Pass
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
