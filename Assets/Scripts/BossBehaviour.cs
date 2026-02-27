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
public class BossBehaviour : MonoBehaviour
{
    private BossStates CurrentState = BossStates.Idle;
    private Transform LastPlayerPosition;
    [SerializeField]
    private Transform LookPoint;
    private float shootDistance = 12f;
    private float attackDistance = 6;
    private Animator _animator;
    private bool isShooting = false;
    private bool isAttacking = false;
    private bool isCharging = false;
    private bool lookAtPlayer = true;
    public float Health = 100;
    private Vector3 PlayerPosition;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        Debug.Log(isCharging);
        switch (CurrentState)
        {
            case BossStates.Idle:
                //
                break;
            case BossStates.Attack:
                Attack();
                break;
            case BossStates.Chase:
                Chase();
                break;
            case BossStates.Charge:
                Charge();
                break;
            case BossStates.Shoot:
                Shoot();
                break;
            default:
                break;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3)
        {

            if (isCharging || isShooting || isAttacking) return;

            LastPlayerPosition = other.transform;
            if (lookAtPlayer)
            {
                 PlayerPosition = new Vector3(LastPlayerPosition.position.x, transform.position.y, LastPlayerPosition.position.z);
                transform.LookAt(PlayerPosition);
            }
             PlayerPosition = new Vector3(LastPlayerPosition.position.x, transform.position.y, LastPlayerPosition.position.z);
           

            RaycastHit hit;
            Vector3 direction = (other.transform.position - LookPoint.position);
            Physics.Raycast(LookPoint.position, direction, out hit);

            Debug.Log("charge cond: " + (Health < 50 && Health > 0 && hit.distance > shootDistance));


            if (Health < 50 && Health > 0 && hit.distance > shootDistance) 
            {
                CurrentState = BossStates.Charge;
            }
            else if (hit.distance <= attackDistance)
            {
                CurrentState = BossStates.Attack;
            }
            else if (hit.distance < shootDistance)
            {
                CurrentState = BossStates.Shoot;
            }
            else
            {
                CurrentState = BossStates.Chase;
            }
        }
    }
    private void Attack()
    {

        if (!isAttacking && !isCharging)
        {
            lookAtPlayer = false;
            isAttacking = true;
            _animator.SetTrigger("isAttacking");
            Invoke("LookAgain", 1.2f);
            Invoke("EndAttack", 2f);
        }
        
    }

    private void Shoot()
    {
        if (!isShooting && !isCharging)
        {
            lookAtPlayer= false;
            isShooting = true;
            _animator.SetTrigger("isShooting");
            Invoke("LookAgain", 1f);
            Invoke("EndShoot", 4f);
        }
        
    }

    private void LookAgain()
    {
        lookAtPlayer = true;
    }
    private void EndShoot()
    {
        isShooting = false;
        CurrentState = BossStates.Chase; 
        
    }
    private void EndAttack()
    {
        isAttacking = false;
        CurrentState = BossStates.Chase;

    }

    private void Charge()
    {
        if (!isCharging)
        {
            lookAtPlayer = false;
            isCharging = true;
            _animator.SetTrigger("isCharging");
            //Debug.Log("Iniciando carga hacia: " + LastPlayerPosition.position);
        }

        float chargeSpeed = 10f;
        Vector3 PlayerPosition = new Vector3(LastPlayerPosition.position.x, transform.position.y, LastPlayerPosition.position.z);

        transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, chargeSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, PlayerPosition) < 0.1f)
        {
            EndCharge();
        }
    }
    private void EndCharge()
    {
        isCharging = false;
        CurrentState = BossStates.Idle;
        lookAtPlayer = true;
        Debug.Log("Ending Charge");

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isCharging && collision.gameObject.layer != 7)
        {
            Debug.Log("no me digas ");
            isCharging= false;
            Invoke("LookAgain", 0f);
            Invoke("EndCharge", 0f);
            CurrentState = BossStates.Idle;

        }
    }
    private void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(LastPlayerPosition.position.x, transform.position.y, LastPlayerPosition.position.z), 2f * Time.deltaTime);

    }

    private void Idle()
    {

    }

}
