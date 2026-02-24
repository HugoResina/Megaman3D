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
    
    private bool CanAttack = true;
   
    private Vector3 playerLastPosition;
    
    [SerializeField]
    private LayerMask PlayerLayer;
    [SerializeField]
    private float lookDistance = 8f;
    RobotStates CurrentState;
    RobotShooter shooter;
    [SerializeField]
    Transform[] route;
    
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
        else
        {
            CurrentState = RobotStates.patrol;
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

        Vector3 CurrentPoint = route[PatrolIndex % route.Length].position ;
        CurrentPoint.y = 0f;
        
            Vector3 posToGoTo = new Vector3(CurrentPoint.x, transform.position.y, CurrentPoint.z);
            transform.position = Vector3.MoveTowards(transform.position, posToGoTo, 0.45f * Time.deltaTime);
            transform.LookAt(posToGoTo);
        if (Vector3.Distance(transform.position, posToGoTo) < 0.05f)
            {
            PatrolIndex++;
            }
        
    }
    public IEnumerator ShootCooldown()
    {
        CanAttack = false;
        yield return new WaitForSeconds(3f);
        CanAttack = true;
        
    }
}
