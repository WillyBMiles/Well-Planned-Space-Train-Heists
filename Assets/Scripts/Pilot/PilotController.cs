using SK.GyroscopeWebGL;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(CharacterController))]
public class PilotController : MonoBehaviour
{
    public static PilotController Instance { get; private set; }
    public static Vector3 Movement { get; private set; }
    public static Vector3 LateralMove { get; private set; }

    public event System.Action OnHit;

    [SerializeField]
    float speed;
    [SerializeField]
    float backwardSpeed;
    [SerializeField]
    float strafeSpeed;

    [SerializeField]
    AudioSource shootSource;
    [SerializeField]
    AudioSource hitSource;
    [SerializeField]
    AudioSource moveSound;
    [SerializeField]
    AudioSource hitPlayer;
    [SerializeField]
    AudioSource warpSound;

    [SerializeField]
    int health;
    public int Health => health;
    [SerializeField]
    int maxHealth;
    public int MaxHealth => maxHealth;
    [SerializeField]
    float maxHealTimer;
    float healTimer;
    [SerializeField]
    float rebootTime;
    float currentRebootTimer;
    public float RebootTime => rebootTime;
    public float CurrentRebootTimer => currentRebootTimer;

    [SerializeField]
    LayerMask layerMask;

    [Header("Shooting")]
    [SerializeField]
    Animator[] gunAnimators;
    [SerializeField]
    ParticleSystem[] gunParticleSystems;
    int gunIndex;



    #if UNITY_EDITOR
    [SerializeField]
    bool usePCInEditor;
#endif

    CharacterController characterController;

    private void Awake()
    {
        Enemy.GotHit += Enemy_GotHit;
#if UNITY_EDITOR
        if (usePCInEditor)
        {
            MenuScene.OnPc = true;
        }
#endif
        health = maxHealth;
        characterController = GetComponent<CharacterController>();
        Instance = this;
    }

    private void OnDestroy()
    {
        Enemy.GotHit -= Enemy_GotHit;
    }

    private void Enemy_GotHit()
    {
        hitSource.pitch = Random.Range(.9f, 1.1f);
        hitSource.Play();
    }

    private void Start()
    {
        GameController.Instance.OnHitDamage += GotHit;
    }

    private void GotHit(GameObject collider, int damage)
    {
        if (collider == gameObject)
        {
            HitMe(damage);
        }
    }

    public void HitMe(int damage)
    {
        Debug.Log("Ouch");
        CameraShake.Shake(.1f);
        OnHit?.Invoke();
        //TAKE DAMAGE
        health -= damage;
        healTimer = maxHealTimer;

        if (health <= 0f)
        {
            DisableSelf();
        }
        hitPlayer.Play();
    }

    void DisableSelf()
    {
        if (currentRebootTimer <= 0f)
            currentRebootTimer = rebootTime; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.Instance.HasStarted)
            return;


        if (currentRebootTimer > 0f)
        {
            currentRebootTimer -= Time.deltaTime;
            if (currentRebootTimer < 0f)
            {
                health = maxHealth;
            }
            return;
        }

        Vector3 moveOffset = TouchUpdate();

        if (MenuScene.OnPc)
        {
            moveOffset = PCOffset();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShootGun();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                Warp();
            }
        }

        if (healTimer <= 0)
        {
            health = Mathf.Min(health + 2, maxHealth);
            healTimer = maxHealTimer;
        }
        healTimer -= Time.deltaTime;


        Vector3 finalMove;
        Vector3 lateralMove;
        if (Rotater.Rotated)
        {
            float move = moveOffset.x;
            lateralMove = -moveOffset.y * strafeSpeed * transform.right;
            finalMove = (move > 0 ? speed : backwardSpeed) * move * transform.forward + lateralMove; 

        }
        else
        {
            float move = moveOffset.y;
            lateralMove = +moveOffset.x * strafeSpeed * transform.right;
            finalMove = (move > 0 ? speed : backwardSpeed) * move * transform.forward + lateralMove;
        }


        moveSound.volume = finalMove.magnitude / speed * .3f;
        moveSound.pitch = -Vector3.Dot(finalMove / speed, transform.forward) / 10f + 1f;
        characterController.Move(finalMove * Time.deltaTime);
        Movement = finalMove;
        LateralMove = lateralMove;

        int maxDist = WorldModel.HalfWidth * WorldModel.SIZE_OF_GRID_CUBE + WorldModel.SIZE_OF_GRID_CUBE / 2;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -maxDist, maxDist),
            Mathf.Clamp(transform.position.y, -maxDist, maxDist),
            Mathf.Clamp(transform.position.z, -maxDist, maxDist));
    }

    Vector2 PCOffset()
    {
        Vector2 moveOffset;

        moveOffset =
        Input.GetAxis("Horizontal") * Vector2.right +
        Input.GetAxis("Vertical") * Vector2.up;


        if (moveOffset.sqrMagnitude > 1)
        {
            moveOffset = moveOffset.normalized;
        }
        return moveOffset;
    }

    Vector3 TouchUpdate()
    {
        Vector3 moveOffset = new();

        int leftTouches = 0;
        foreach (var touch in Input.touches)
        {
            if (Rotater.Rotated)
            {
                if (touch.position.y / Screen.height < .5f)
                {
                    RightSide(touch);
                }
                else if (touch.position.y / Screen.height > .5f)
                {
                    moveOffset = LeftSide(touch);
                    leftTouches++;
                }
            }
            else
            {
                if (touch.position.x / Screen.width > .5f)
                {
                    RightSide(touch);
                }
                else if (touch.position.x / Screen.width < .5f)
                {
                    moveOffset =LeftSide(touch);
                    leftTouches++;
                }
            }

            
        }
        

        if (leftTouches > 0)
        {
            moveOffset /= .2f;
            if (moveOffset.sqrMagnitude > 1f)
            {
                moveOffset = moveOffset.normalized;
            }
        }


        return moveOffset;

    }

    void RightSide(Touch touch)
    {
        
        //if (touch.phase == TouchPhase.Began)
        //{
        //    ShootGun();
        //}
    }

    public void Warp()
    {
        if (currentRebootTimer <= 0f && GameController.Instance.HasStarted)
        {
            CameraShake.Shake(1f);
            currentRebootTimer = rebootTime;
            characterController.enabled = false;
            transform.position = GameController.Instance.WarpPosition;
            GetComponent<Rigidbody>().MovePosition(GameController.Instance.WarpPosition);
            characterController.enabled = true;
            warpSound.Play();
        }
            
    }

    public void Shoot()
    {
        if (currentRebootTimer <= 0f && GameController.Instance.HasStarted)
        ShootGun();
    }

    void ShootGun()
    {

        gunIndex++;
        gunIndex %= gunAnimators.Length;
        gunAnimators[gunIndex].SetTrigger("Shoot");
        gunParticleSystems[gunIndex].Emit(1);

        //Debug.Log("Shoot!");
        shootSource.pitch = Random.Range(.9f, 1.1f);
        shootSource.Play();

        var ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f,Screen.height / 2f));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 150f, layerMask))
        {
            GameController.Instance.DoHitDamage(hitInfo.collider.gameObject, 1);
        }
    }

    Vector2 startOfVirtual;
    Vector2 LeftSide(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                startOfVirtual = touch.position;
                return new();
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                return (touch.position - startOfVirtual) / Screen.width;
            case TouchPhase.Canceled:
            case TouchPhase.Ended:
            default:
                return new Vector2();
        }

    }


}


/*
 using SK.GyroscopeWebGL;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PilotCamera : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float backwardSpeed;
    [SerializeField]
    float strafeSpeed;

    [SerializeField]
    AudioSource shootSource;

    CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        TouchUpdate();
    }


    void TouchUpdate()
    {
        int leftTouches = 0;
        foreach (var touch in Input.touches)
        {
            if (Rotater.Rotated)
            {
                if (touch.position.y / Screen.height < .5f)
                {
                    RightSide(touch);
                }
                else if (touch.position.y / Screen.height > .5f)
                {
                    LeftSide(touch);
                    leftTouches++;
                }
            }
            else
            {
                if (touch.position.x / Screen.width > .5f)
                {
                    RightSide(touch);
                }
                else if (touch.position.x / Screen.width < .5f)
                {
                    LeftSide(touch);
                    leftTouches++;
                }
            }

            
        }
        if (leftTouches == 0)
            moveOffset = new();

        // - y is left
        // + y is right
        // + x is backward
        // -x is forward
        Debug.Log(moveOffset.ToString());
        moveOffset /= .2f;
        if (moveOffset.sqrMagnitude > 1)
        {
            moveOffset = moveOffset.normalized;
        }

        Vector3 finalMove;
        if (Rotater.Rotated)
        {
            float move = moveOffset.x;
            finalMove = (move > 0 ? speed : backwardSpeed) * move * transform.forward - moveOffset.y * strafeSpeed * transform.right ;
            
        }
        else
        {
            float move = moveOffset.y;
            finalMove = (move > 0 ? speed : backwardSpeed) * move * transform.forward + moveOffset.x * strafeSpeed * transform.right;
        }

        characterController.Move(finalMove * Time.deltaTime);
    }

    void RightSide(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            ShootGun();
        }
    }

    void ShootGun()
    {
        Debug.Log("Shoot!");
        shootSource.Play();
    }

    Vector2 startOfVirtual;
    Vector2 moveOffset;
    void LeftSide(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                startOfVirtual = touch.position;
                break;
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                moveOffset = (touch.position - startOfVirtual) / Screen.width;
                break;
            case TouchPhase.Canceled:
            case TouchPhase.Ended:
                moveOffset = new Vector2();
                break;
        }
    }


}

 */
