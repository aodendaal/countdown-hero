using UnityEngine;
using Unity.Cinemachine;
using QFSW.QC; // Use "using Cinemachine;" if on Cinemachine v2

public class GameStateManager : MonoBehaviour
{
    public enum GameState { Overworld, Combat, Menu }

    public static GameStateManager Instance { get; private set; }

    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineCamera overworldVCam;
    [SerializeField] private CinemachineCamera combatVCam;

    [Header("Controllers & Logic")]
    [SerializeField] private MonoBehaviour overworldMovementScript;
    [SerializeField] private CombatManager combatManager;

    [Header("Priority Settings")]
    [SerializeField] private int activePriority = 20;
    [SerializeField] private int inactivePriority = 10;

    public GameState CurrentState { get; private set; }

    private GameObject currentOverworldEnemyObject;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetState(GameState.Overworld);
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

            // Snap back to overworld
            SetState(GameState.Overworld);
        }
        else
        {
            // Handle player death / respawn logic
        }
    }


    public void SetState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Overworld:
                // Raise Overworld priority so Cinemachine blends/cuts to it
                overworldVCam.Priority = activePriority;
                combatVCam.Priority = inactivePriority;

                // Enable overworld player controls
                overworldMovementScript.enabled = true;
                break;

            case GameState.Combat:
                // Raise Combat priority to switch to fixed combat view
                combatVCam.Priority = activePriority;
                overworldVCam.Priority = inactivePriority;

                // Disable overworld controls and start battle sequence
                overworldMovementScript.enabled = false;
                break;
        }
    }
}