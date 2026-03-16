// CheckpointManager.cs
// Singleton that tracks the last activated checkpoint and handles player respawning.
// Replaces the ad-hoc respawn logic spread across GayManager and MenuManager.
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [Header("Fallback spawn used before any checkpoint is reached")]
    [SerializeField] private Transform defaultSpawn;

    private Vector3? _lastCheckpointPos = null;

    public bool HasCheckpoint => _lastCheckpointPos.HasValue;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>Called by Checkpoint when the player touches it.</summary>
    public void RegisterCheckpoint(Vector3 position)
    {
        _lastCheckpointPos = position;
        Debug.Log($"[CheckpointManager] Checkpoint saved at {position}");
    }

    /// <summary>
    /// Teleports the player to the last checkpoint (or default spawn).
    /// Also restores full health.
    /// </summary>
    public void RespawnPlayer(GameObject player)
    {
        if (player == null) return;

        // Determine spawn position
        Vector3 spawnPos = _lastCheckpointPos.HasValue
            ? _lastCheckpointPos.Value
            : (defaultSpawn != null ? defaultSpawn.position : Vector3.zero);

        // Disable CharacterController temporarily so we can teleport
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = spawnPos;

        if (cc != null) cc.enabled = true;

        // Restore health
        Health health = player.GetComponent<Health>();
        if (health != null)
            health.Heal(health.GetMaxHealth());

        Debug.Log($"[CheckpointManager] Player respawned at {spawnPos}");
    }
}