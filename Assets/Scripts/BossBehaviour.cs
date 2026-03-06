using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;

public enum BossStates
{
    Idle,
    Attack,
    Chase, 
    Charge,
    Shoot

}

//este script funciona
public class BossBehaviour : MonoBehaviour
{
    private BossStates CurrentState = BossStates.Idle;
    private Transform LastPlayerPosition;
    [SerializeField] private Transform LookPoint;

    private float shootDistance = 12f;
    private float attackDistance = 6f;
    private float moveSpeed = 3f; 

    private Animator _animator;
    private bool isPerformingAction = false;
    private bool lookAtPlayer = true;

    [Header("Combat Settings")]
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private Transform hitPointL; 
    [SerializeField] private Transform hitPointR; 

    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform ShootPoint; 

    private bool canAttack = true;
    [SerializeField] private float attackCooldown = 1.5f;
  

    public float Health = 100;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleRotation();

        switch (CurrentState)
        {
            case BossStates.Chase: Chase(); break;
            case BossStates.Charge: Charge(); break;
            case BossStates.Attack: Attack(); break;
            case BossStates.Shoot: ShootAnimation(); break;
            case BossStates.Idle: /*idle*/ break;
        }
    }

    public void CheckHitL()
    {
        Collider[] hitColliders = Physics.OverlapSphere(hitPointL.position, attackRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.layer == 3)
            {
                Debug.Log("¡Golpe al jugador!");
               

                
                break;
            }
        }
    }
    public void CheckHitR()
    {
        Collider[] hitColliders = Physics.OverlapSphere(hitPointR.position, attackRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.layer == 3)
            {
                Debug.Log("¡Golpe al jugador!");
                

                
                break;
            }
        }
    }

  
    private void OnDrawGizmosSelected()
    {
        if (hitPointR != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitPointR.position, attackRadius);
        }
        if (hitPointL != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitPointL.position, attackRadius);
        }
    }
    private void HandleRotation()
    {
        if (lookAtPlayer && LastPlayerPosition != null)
        {
            Vector3 targetPos = new Vector3(LastPlayerPosition.position.x, transform.position.y, LastPlayerPosition.position.z);
            transform.LookAt(targetPos);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            LastPlayerPosition = other.transform;
            if (isPerformingAction) return;

            float distance = Vector3.Distance(LookPoint.position, other.transform.position);

            
            if (distance <= attackDistance && canAttack)
            {
                ChangeState(BossStates.Attack);
            }
            else if (distance < shootDistance && canAttack)
            {
                ChangeState(BossStates.Shoot);
            }
            else if (Health < 50 && Health > 0 && distance > shootDistance && canAttack)
            {
                ChangeState(BossStates.Charge);
            }
            else
            {
                ChangeState(BossStates.Chase);
            }
        }
    }

    private void ChangeState(BossStates newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;
    }

    private void Attack()
    {
        if (isPerformingAction) return;
        StartAction(1.2f, 2f);
        _animator.SetTrigger("isAttacking");
    }

    private void ShootAnimation()
    {
        if (isPerformingAction) return;
        StartAction(1f, 4f);
        _animator.SetTrigger("isShooting");
    }

    private void Charge()
    {
        isPerformingAction = true;
        lookAtPlayer = false;
        _animator.SetTrigger("isCharging");

        Vector3 targetPos = new Vector3(LastPlayerPosition.position.x, transform.position.y, LastPlayerPosition.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 10f * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            EndAction();
        }
    }
    public void Shoot()
    {
        //Debug.Log("pam");
        BossBullet.Create(Bullet, ShootPoint.position, ShootPoint.rotation, 10f, 3f);
    }
    private void StartAction(float lookDelay, float totalDuration)
    {
        isPerformingAction = true;
        canAttack = false; 
        lookAtPlayer = false;

        Invoke(nameof(RestoreLook), lookDelay);
        Invoke(nameof(EndAction), totalDuration);
    }

    private void RestoreLook() => lookAtPlayer = true;

    private void EndAction()
    {
        isPerformingAction = false;
        lookAtPlayer = true;
        CurrentState = BossStates.Idle;

       
        Invoke(nameof(ResetCooldown), attackCooldown);
    }

    private void ResetCooldown()
    {
        canAttack = true;
    }

    private void Chase()
    {
        if (LastPlayerPosition == null) return;

        Vector3 target = new Vector3(LastPlayerPosition.position.x, transform.position.y, LastPlayerPosition.position.z);
        float distance = Vector3.Distance(transform.position, target);

        if (canAttack && distance <= attackDistance)
        {
            EndAction();
            ChangeState(BossStates.Attack);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

    }
}