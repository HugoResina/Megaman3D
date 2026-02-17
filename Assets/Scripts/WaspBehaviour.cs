using System.Collections;
using UnityEngine;
public enum WaspStates
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
    public WaspStates CurrentState;
        
    private void Start()
    {
        CurrentState = WaspStates.Idle;
        startTime = Time.time;
    }
    private void Update()
    {
        //Debug.Log(CurrentState);
        //if (isAttacking)
        //{
        //     distCovered = (Time.time - startTime) * speed;
        //     fracJourney = distCovered / journeyLength;
        //}
        //determine state

        switch (CurrentState)
        {
            case WaspStates.Idle:
                //awdf
                break;
            case WaspStates.Attack:
                //asdf
                //distCovered = (Time.time - startTime) * speed;
                fracJourney = 0.008f;
                //Debug.Log("cuanto? " +  fracJourney);
                Attack();
                break;
            case WaspStates.Chase:
                Chase();
                break;
            case WaspStates.Reposition:
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
            RaycastHit hit;
            Vector3 direction = (other.transform.position - transform.position);

            

            if (Physics.Raycast(transform.position, direction, out hit))
            {
                Debug.DrawRay(transform.position, direction * lookDistance, Color.red);
                Debug.Log("is player hit:" + (hit.collider.gameObject.layer == 3));
                if (hit.collider.gameObject.layer == 3) 
                {
                    playerLastPosition = other.transform.position;
                    transform.LookAt(playerLastPosition);

                    if (CanAttack)
                    {
                        waspPositionBeforeAttack = transform.position;
                        CanAttack = false;
                        CurrentState = WaspStates.Attack;
                    }
                }
                else
                {
                    CurrentState = WaspStates.Chase; 
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 0 )
        {
            CurrentState = WaspStates.Reposition;
            
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
        CurrentState = WaspStates.Reposition;
    }
    private void Attack()
    {
       
        isAttacking = true;
        //attackDirection = playerLastPosition - transform.position;
        //transform.position = Vector3.MoveTowards(transform.position, playerLastPosition, 15);
        StartCoroutine(RecoverTimeFromAttack());
        transform.position = Vector3.Lerp(transform.position,playerLastPosition, fracJourney);
    }
    private void Chase()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(playerLastPosition.x, transform.position.y, playerLastPosition.z), 0.003f);
        CanAttack = true;
    }
    private void Reposition()
    {
        transform.position = Vector3.Lerp(transform.position, waspPositionBeforeAttack, 0.004f);
        Vector3 dist = transform.position - waspPositionBeforeAttack;
        if (dist.magnitude < 0.5f)
        {
            CurrentState = WaspStates.Chase;
            CanAttack = true ;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(waspPositionBeforeAttack, 1f);
    }
}
