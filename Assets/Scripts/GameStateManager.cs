using UnityEngine;
using Unity.Cinemachine;
using QFSW.QC; // Use "using Cinemachine;" if on Cinemachine v2

public class GameStateManager : MonoBehaviour
{
    public enum GameState { Start, Overworld, Combat, Menu, Timeout }

    public static GameStateManager Instance { get; private set; }

    [Header("Menus")]
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject timeOutMenu;

    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineCamera overworldVCam;
    [SerializeField] private CinemachineCamera combatVCam;

    [Header("Controllers & Logic")]
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private MonoBehaviour overworldMovementScript;
    [SerializeField] private CombatManager combatManager;
    [SerializeField] private MusicManager musicManager;

    [Header("Entities")]
    [SerializeField] private GameObject overworldHero;
    [SerializeField] private GameObject overworldMonstersPrefab;

    [Header("Priority Settings")]
    [SerializeField] private int activePriority = 20;
    [SerializeField] private int inactivePriority = 10;

    public GameState CurrentState { get; private set; }

    private GameObject currentOverworldEnemyObject;
    private GameObject overlandMonsters;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        startMenu.SetActive(false);
        timeOutMenu.SetActive(false);

        SetState(GameState.Start);
    }

    public void StartCombatWith(MonsterData monsterData, GameObject overworldEnemyObj)
    {
        currentOverworldEnemyObject = overworldEnemyObj;

        // 1. Pass data to combat manager
        combatManager.SetupAndStartBattle(monsterData);

        // 2. Switch State & Cut Camera
        SetState(GameState.Combat);
    }

    public void EndCombat(bool playerWon)
    {

        if (playerWon)
        {
            // Remove the monster from the map upon victory
            if (currentOverworldEnemyObject != null)
            {
                Destroy(currentOverworldEnemyObject);
            }

            if (CurrentState != GameState.Combat) return;
            // Snap back to overworld
            SetState(GameState.Overworld);
        }
        else
        {
            // Handle player death / respawn logic
        }
    }

    public void TimeUp()
    {
        SetState(GameState.Timeout);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Start:
                Debug.LogWarning("State: Sart");
                musicManager.PlayMenuTrack();
                startMenu.SetActive(true);
                overworldVCam.Priority = activePriority;
                combatVCam.Priority = inactivePriority;
                break;

            case GameState.Timeout:
                Debug.LogWarning("State: Timeout");
                musicManager.PlayMenuTrack();
                timeOutMenu.SetActive(true);
                overworldVCam.Priority = activePriority;
                combatVCam.Priority = inactivePriority;
                break;

            case GameState.Overworld:
                Debug.LogWarning("State: Overworld");
                musicManager.PlayGameTrack();
                // Raise Overworld priority so Cinemachine blends/cuts to it
                overworldVCam.Priority = activePriority;
                combatVCam.Priority = inactivePriority;

                // Enable overworld player controls
                overworldMovementScript.enabled = true;
                break;

            case GameState.Combat:
                Debug.LogWarning("State: Combat");
                musicManager.PlayGameTrack();
                // Raise Combat priority to switch to fixed combat view
                combatVCam.Priority = activePriority;
                overworldVCam.Priority = inactivePriority;

                // Disable overworld controls and start battle sequence
                overworldMovementScript.enabled = false;
                break;
        }
    }

    public void StartNewGame()
    {
        // Clean up
        Destroy(overlandMonsters);

        // Reset
        countdownTimer.ResetTimer();
        countdownTimer.StartTimer();

        overworldHero.transform.position = new Vector3(0f, 10f, 0f);

        overlandMonsters = Instantiate(overworldMonstersPrefab, Vector3.zero, Quaternion.identity);
        overlandMonsters.transform.SetParent(this.transform);

        // Start Overworld
        SetState(GameState.Overworld);
    }
}
