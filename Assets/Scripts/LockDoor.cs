// LockDoor.cs
// Attach to the boss door (or any one-way door).
//
// Setup in Inspector:
//   • doorMesh       – the visual door object (will animate shut)
//   • blocker        – a thin collider-only GameObject placed just inside the
//                      door frame; starts DISABLED, gets ENABLED after lock
//   • closedPosition – local position the doorMesh moves to when it closes
//   • closeSpeed     – how fast the door slides shut
//
// The trigger collider on THIS GameObject detects the player entering.
// Once triggered, the door closes and the blocker prevents going back.

using System.Collections;
using UnityEngine;

public class LockDoor : MonoBehaviour
{
    [Header("Door visual (the mesh/model that moves)")]
    [SerializeField] private GameObject doorMesh;

    [Header("Invisible blocker placed on the entry side of the door")]
    [SerializeField] private GameObject blocker;

    [Header("Where the door moves to when closed (local space of doorMesh's parent)")]
    [SerializeField] private Vector3 closedLocalPosition;

    [Header("Animation")]
    [SerializeField] private float closeSpeed = 2f;
    [SerializeField] private float delayBeforeClose = 0.5f;   // seconds after player enters

    private bool _locked = false;
    private Vector3 _openLocalPosition;

    private void Start()
    {
        if (doorMesh != null)
            _openLocalPosition = doorMesh.transform.localPosition;

        // Blocker starts inactive — player can walk through freely
        if (blocker != null)
            blocker.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_locked) return;
        if (!other.CompareTag("Player")) return;

        _locked = true;
        StartCoroutine(CloseDoorRoutine());
    }

    private IEnumerator CloseDoorRoutine()
    {
        yield return new WaitForSeconds(delayBeforeClose);

        // Activate the blocker so the player can't walk back out
        if (blocker != null)
            blocker.SetActive(true);

        // Slide the door to its closed position
        if (doorMesh != null)
        {
            float elapsed = 0f;
            Vector3 startPos = doorMesh.transform.localPosition;

            while (elapsed < 1f)
            {
                elapsed += Time.deltaTime * closeSpeed;
                doorMesh.transform.localPosition = Vector3.Lerp(startPos, closedLocalPosition, elapsed);
                yield return null;
            }
            doorMesh.transform.localPosition = closedLocalPosition;
        }

        Debug.Log("[LockDoor] Door locked.");
    }

    // Editor helper: lets you preview the closed position as a gizmo
    private void OnDrawGizmosSelected()
    {
        if (doorMesh == null) return;
        Gizmos.color = Color.red;
        Vector3 worldClosed = doorMesh.transform.parent != null
            ? doorMesh.transform.parent.TransformPoint(closedLocalPosition)
            : closedLocalPosition;
        Gizmos.DrawWireCube(worldClosed, doorMesh.transform.localScale);
    }
}