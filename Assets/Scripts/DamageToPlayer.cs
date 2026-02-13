using UnityEngine;

public class DamageToPlayer : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 25f; // Amount of damage to apply
    private void OnCollisionEnter(Collision collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damageAmount);
        }
    }
}
