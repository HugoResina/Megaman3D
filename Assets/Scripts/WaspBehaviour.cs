using System.Collections;
using UnityEngine;
public enum EnemyStates
{
    Idle,
    Attack,
    Chase,
    Reposition
}
public class WaspBehaviour : MonoBehaviour
{
    private float ShootRate = 1f;
    private bool CanAttack = true;
    private bool isAttacking = false;
    private Vector3 playerLastPosition;
    private Vector3 waspPositionBeforeAttack;
    private Vector3 attackDirection;
    [SerializeField]
    private LayerMask PlayerLayer;
    public float speed = 1f;
    private float startTime;
    private float journeyLength = 10f;
    [SerializeField]
    private float lookDistance = 8f;
    private float fracJourney;
    private float distCovered;
    public EnemyStates CurrentState;
        
    private void Start()
    {
        CurrentState = EnemyStates.Idle;
        startTime = Time.time;
    }
    private void Update()
    {
        Debug.Log(CurrentState);
        //if (isAttacking)
        //{
        //     distCovered = (Time.time - startTime) * speed;
        //     fracJourney = distCovered / journeyLength;
        //}
        //determine state

        switch (CurrentState)
        {
            case EnemyStates.Idle:
                //awdf
                break;
            case EnemyStates.Attack:
                //asdf
                //distCovered = (Time.time - startTime) * speed;
                fracJourney = 0.008f;
                //Debug.Log("cuanto? " +  fracJourney);
                Attack();
                break;
            case EnemyStates.Chase:
                Chase();
                break;
            case EnemyStates.Reposition:
                Reposition();
                break;
            default:
                break;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Debug.Log(other.gameObject.name);
            bool hit;
            hit = Physics.Raycast(transform.position, (other.transform.position - transform.position), lookDistance, PlayerLayer);

            //if hit player, attack
            //else move to 
            transform.LookAt(other.transform.position);
            playerLastPosition = other.transform.position;
            //Debug.Log("hit: " +  hit);
            if (hit)
            {
                //Debug.Log("CHOKE ON ME");
                if (CanAttack)
                {
                    //Debug.Log("canAttack");
                   

                    waspPositionBeforeAttack = transform.position;
                    
                    CanAttack = false;
                    CurrentState = EnemyStates.Attack;

                    //StartCoroutine(ShootPlayer());
                }
            }
            else
            {
                CurrentState = EnemyStates.Chase;
            }
                Debug.DrawRay(transform.position, (playerLastPosition - transform.position), Color.red);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 0 )
        {
            CurrentState = EnemyStates.Reposition;
            
            //SI LAYER 3 DAÑO PLAYER
        }
    }
    private IEnumerator ShootPlayer()
    {
        //yield return new WaitForSeconds(ShootRate);

        var waitForGrounded = new WaitUntil(() => CanAttack);
        

        yield return new WaitForSeconds(ShootRate);
        CanAttack = true ;
    }
    private IEnumerator RecoverTimeFromAttack()
    {
        //yield return new WaitForSeconds(ShootRate);

        yield return new WaitForSeconds(4);
        CurrentState = EnemyStates.Reposition;
    }
    private void Attack()
    {
        //Debug.Log("ataco");
        isAttacking = true;
        //attackDirection = playerLastPosition - transform.position;
        //transform.position = Vector3.MoveTowards(transform.position, playerLastPosition, 15);
        StartCoroutine(RecoverTimeFromAttack());
        transform.position = Vector3.Lerp(transform.position,playerLastPosition, fracJourney);
    }
    private void Chase()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(playerLastPosition.x, transform.position.y, playerLastPosition.z), 0.004f);
        CanAttack = true;
    }
    private void Reposition()
    {
        transform.position = Vector3.Lerp(transform.position, waspPositionBeforeAttack, 0.004f);
        Vector3 dist = transform.position - waspPositionBeforeAttack;
        if (dist.magnitude < 0.5f)
        {
            CurrentState = EnemyStates.Chase;
            CanAttack = true ;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(waspPositionBeforeAttack, 1f);
    }
}
