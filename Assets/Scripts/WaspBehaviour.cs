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
    private float fracJourney = 0.008f;
    //[SerializeField]
    private float health = 50;
    [SerializeField]
    private GameObject explosion;

    private float distCovered;
    public WaspStates CurrentState;

    private void Start()
    {
        CurrentState = WaspStates.Idle;
        startTime = Time.time;
    }
    private void Update()
    {
        //Debug.Log("avisjpa : " + CurrentState);
        

        switch (CurrentState)
        {
            case WaspStates.Idle:
                break;
            case WaspStates.Attack:
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
            if (CurrentState == WaspStates.Attack || CurrentState == WaspStates.Reposition)
                return;

            if(CurrentState != WaspStates.Attack)
            {
                CurrentState = WaspStates.Chase;
            }

            Vector3 direction = (other.transform.position - transform.position);
            playerLastPosition = other.transform.position;
            transform.LookAt(playerLastPosition);

            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, direction, out hit, lookDistance))
            {
                if (hit.collider.gameObject.layer == 3) 
                {
                    if (CanAttack)
                    {
                        
                        StartAttackSequence();
                    }
                    else
                    {
                        CurrentState = WaspStates.Chase;
                    }
                }
                else
                {
                  
                    CurrentState = WaspStates.Idle;
                }
            }
        }
    }

    private void StartAttackSequence()
    {
        CurrentState = WaspStates.Attack;
        CanAttack = false;
        isAttacking = true;
        waspPositionBeforeAttack = transform.position;

        StopAllCoroutines();
        StartCoroutine(RecoverTimeFromAttack());
    }

    private void Attack()
    {
     
        transform.position = Vector3.MoveTowards(transform.position, playerLastPosition, speed * Time.deltaTime * 8);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 0)
        {
            CurrentState = WaspStates.Reposition;
            if(collision.gameObject.layer == 3)
            {
                Health h = collision.gameObject.GetComponent<Health>();
                h.TakeDamage(20);
            }   
        }
        if (collision.gameObject.layer == 7)
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            Debug.Log(bullet.damage);
            health = health - bullet.damage;
            //Debug.Log(health);
            if (health <= 0)
            {
                
                //sonido
                Instantiate(explosion, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }

    
   
    private IEnumerator RecoverTimeFromAttack()
    {
       
        isAttacking = false;
        yield return new WaitForSeconds(2);
        CurrentState = WaspStates.Reposition;
    }
   
    private void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerLastPosition.x, transform.position.y, playerLastPosition.z), speed * Time.deltaTime * 6);
        CanAttack = true;
    }
    private void Reposition()
    {
        transform.position = Vector3.MoveTowards(transform.position, waspPositionBeforeAttack, speed * Time.deltaTime * 4);
        Vector3 dist = transform.position - waspPositionBeforeAttack;
        if (dist.magnitude < 0.5f)
        {
            //transform.LookAt(new Vector3(playerLastPosition.x, transform.rotation.y, playerLastPosition.z));
            //transform.rotation = Quaternion.Euler(playerLastPosition.x, 0, playerLastPosition.z);

            CurrentState = WaspStates.Idle;
            CanAttack = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(waspPositionBeforeAttack, 1f);
    }
}