using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1000)]
public class GameController : MonoBehaviour
{
    [SerializeField] string endScreenScene;

    public static GameController Instance { get; private set; }
    [SerializeField]
    WorldModelInstance worldModelInstance;
    public WorldModelInstance WorldModelInstance => worldModelInstance;

    public WorldModel worldModel;
    public GamePlan gamePlan;

    public event Action<GameObject, int> OnHitDamage;
    public void DoHitDamage(GameObject hit, int damage)
    {
        OnHitDamage?.Invoke(hit, damage);
    }

    public Vector3 WarpPosition => worldModel.WarpPositions[Mathf.Max(0,(int)TimeLeft.TotalSeconds/ 15)];
    public TimeSpan TimeTilWarp => TimeSpan.FromSeconds(TimeLeft.TotalSeconds % 15);

    public TimeSpan TimeInGame => StartTime.HasValue ? DateTime.UtcNow - StartTime.Value : TimeSpan.FromMinutes(0);
    public TimeSpan TimeLeft => TimeSpan.FromMinutes(5) - TimeInGame;
    DateTime? StartTime;

    public bool HasStarted => StartTime.HasValue;
    public static int Score { get; private set; }

    public event Action<int> OnScoreChange;

    private void Awake()
    {
        Instance = this;
        Score = 0;
    }


    private void Start()
    {
        GenerateGame();

    }

    private void Update()
    {
        if (HasStarted)
        {
            gamePlan.Update(TimeInGame);
        }
        if (TimeLeft < TimeSpan.Zero)
        {
            SceneManager.LoadScene(endScreenScene);
        }
    }

    public void GenerateGame()
    {
        int seed = 0;
        foreach (char c in LobbyController.code)
        {
            seed += c;
        }
        System.Random random = new(seed);
        worldModel = new WorldModel(random);
        worldModelInstance.Instantiate(worldModel);
        gamePlan = new GamePlan(random, worldModelInstance.stations.Count);
    }


    public void StartGame()
    {
        //TODO
        if (MenuScene.OnPc && PilotController.Instance != null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
        }
        gamePlan.StartGame();

        StartTime = DateTime.UtcNow;

    }

    public void EarnScore(int scoreIncrease)
    {
        Score += scoreIncrease;
        OnScoreChange?.Invoke(Score);
    }
}


