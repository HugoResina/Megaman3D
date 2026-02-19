using System.Collections;
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
    Transform[] route;
    private float nextShootTime = 1f;
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
                Attack();
                break;
            
            case RobotStates.patrol:
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
                    transform.LookAt(playerLastPosition);

                    if (CanAttack)
                    {
                       
                        CurrentState = RobotStates.Attack;
                       
                        Attack();
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
    public void Attack()
    {

        if (CanAttack)
        {
            shooter.Shoot();
        //nextShootTime = Time.time + ShootRate; 
        StartCoroutine(ShootCooldown());
        }
    }
    public void Patrol()
    {
        //se mueve periodicamente entre los puntos de route
    }
    public IEnumerator ShootCooldown()
    {
        CanAttack = false;
        yield return new WaitForSeconds(3f);
        CanAttack = true;
        
    }
}
