using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum RobotStates
{
    Attack,
    patrol
}
public class RobotBehaviour : MonoBehaviour
{
    private float ShootRate = 1f;
    private bool CanAttack = true;
    private bool isAttacking = false;
    private Vector3 playerLastPosition;
    private Vector3 attackDirection;
    [SerializeField]
    private LayerMask PlayerLayer;
    [SerializeField]
    private float lookDistance = 8f;
    RobotStates CurrentState;
    RobotShooter shooter;
    [SerializeField]
    Transform[] route;
    private float nextShootTime = 1f;
    private int PatrolIndex = 0;
    void Start()
    {
        CurrentState = RobotStates.patrol;
        shooter = GetComponent<RobotShooter>();
    }

    void Update()
    {
        Debug.Log("puedo atacar: " + CanAttack);
        //Debug.Log("bot" + CurrentState);
        
        switch (CurrentState)
        {

            case RobotStates.Attack:
               
                break;
            
            case RobotStates.patrol:
                Patrol();
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
                
                

                if (hit.collider.gameObject.layer == 3)
                {
                    playerLastPosition = other.transform.position;
                    playerLastPosition.y = 0f;
                    transform.LookAt(playerLastPosition);

                    if (CanAttack)
                    {
                       
                        CurrentState = RobotStates.Attack;
                       
                        Attack(other.transform);
                        CanAttack = false;

                    }

                }
                else
                {

                    CurrentState = RobotStates.patrol;
                    Patrol();
                }
                
            }
        }
    }
    public void Attack(Transform objectiu)
    {

        if (CanAttack)
        {
            shooter.Shoot(objectiu);
            //nextShootTime = Time.time + ShootRate; 
            StartCoroutine(ShootCooldown());
        }
    }
    public void Patrol()
    {

        Vector3 CurrentPoint = route[PatrolIndex].position;
        CurrentPoint.y = 0f;
        

            transform.position = Vector3.MoveTowards(transform.position, CurrentPoint, 2f * Time.deltaTime);
            transform.LookAt(CurrentPoint);
            if(Vector3.Distance(transform.position, CurrentPoint) < 0.05f)
            {
                PatrolIndex = (PatrolIndex +1) % route.Length +1;
            }
        
    }
    public IEnumerator ShootCooldown()
    {
        CanAttack = false;
        yield return new WaitForSeconds(3f);
        CanAttack = true;
        
    }
}
