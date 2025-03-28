using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    float projectileSpeed;
    [SerializeField]
    ParticleSystem rightParticleSystem;
    [SerializeField]
    ParticleSystem leftParticleSystem;

    [SerializeField]
    AudioSource shootSound;
    Animator animator;

    [SerializeField]
    float engageRange = 100;

    Enemy enemy;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnDisable()
    {
        animator.SetBool("Shooting", false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PilotController.Instance == null)
            return;

        float distance = Vector3.Distance(transform.position, PilotController.Instance.transform.position);
        if (distance > engageRange)
        {
            animator.SetBool("Shooting", false);
            return;
        }
        
        if (enemy != null)
        {
            if (distance < 50 || enemy.Health < enemy.MaxHealth)
            {
                animator.SetBool("Shooting", true);
            }
        }
        else
        {
            animator.SetBool("Shooting", true);
        }


        if (!turretActuallyFiring)
        {
            transform.rotation = Quaternion.LookRotation((PilotController.Instance.transform.position +
    PilotController.Movement * distance / projectileSpeed) - transform.position, transform.parent.up);
            if (transform.localRotation.eulerAngles.x > 30 && transform.localRotation.eulerAngles.x < 90)
            {
                transform.localRotation = Quaternion.Euler(30, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }


    }

    bool turretActuallyFiring;
    public void StartFiring()
    {
        turretActuallyFiring = true;
    }

    public void FinishFiring()
    {
        turretActuallyFiring = false;
    }

    public void ShootRightBarrel()
    {

        shootSound.pitch = Random.Range(.9f, 1.1f);
        shootSound.Play();
        rightParticleSystem.Emit(1);
    }

    public void ShootLeftBarrel()
    {

        shootSound.pitch = Random.Range(.9f, 1.1f);
        shootSound.Play();
        leftParticleSystem.Emit(1);
    }

    public void Hit(GameObject gameObject)
    {
        if (1 << gameObject.layer != LayerMask.GetMask("Enemy"))
            GameController.Instance.DoHitDamage(gameObject, 1);
    }
}
