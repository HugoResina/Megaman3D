// Checkpoint.cs
// Attach to a GameObject with a Trigger Collider.
// When the player walks through, this becomes the active respawn point.
// Multiple checkpoints can exist in the scene — only the latest reached is used.
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Respawn position (defaults to this object's position if left empty)")]
    [SerializeField] private Transform respawnAnchor;

    [Header("Optional visual feedback")]
    [SerializeField] private GameObject inactiveVisual;   // shown before activation
    [SerializeField] private GameObject activeVisual;     // shown after activation

    private bool _activated = false;

    private void Start()
    {
        SetVisual(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_activated) return;
        if (!other.CompareTag("Player")) return;

        _activated = true;
        SetVisual(true);

        // Register with CheckpointManager
        Vector3 spawnPos = respawnAnchor != null ? respawnAnchor.position : transform.position;
        CheckpointManager.Instance.RegisterCheckpoint(spawnPos);
    }

    private void SetVisual(bool active)
    {
        if (inactiveVisual != null) inactiveVisual.SetActive(!active);
        if (activeVisual != null) activeVisual.SetActive(active);
    }
}