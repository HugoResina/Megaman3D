using UnityEngine;

public class DoorCloseTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 3) return;

        GayManager.Instance.CloseDoor();
    }
}