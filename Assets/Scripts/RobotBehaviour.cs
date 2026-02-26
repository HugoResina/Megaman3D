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
    
    private bool CanAttack = false;
   
    private Vector3 playerLastPosition;
    
    [SerializeField]
    private LayerMask PlayerLayer;
    [SerializeField]
    private float lookDistance = 8f;
    RobotStates CurrentState;
    RobotShooter shooter;
    [SerializeField]
    Transform[] route;
    public bool isAttacking = false;
    private Animator _animator;
    [SerializeField]
    private Transform LookPoint;
    
    private int PatrolIndex = 0;
    void Start()
    {
        CurrentState = RobotStates.patrol;
        shooter = GetComponent<RobotShooter>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {

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

        
        {
            RaycastHit hit;
            Vector3 direction = (other.transform.position - LookPoint.position);
            Physics.Raycast(LookPoint.position, direction, out hit, 10000f, ~7);

            if (hit.transform.gameObject.layer != 3 && hit.transform.gameObject.layer != 7)
            {
                CanAttack = false;
                _animator.SetBool("isAttacking", false);
                isAttacking = false;
                CurrentState = RobotStates.patrol;
                Patrol();
            }
            else if (hit.transform.gameObject.layer == 3)
            {
                isAttacking = true;
                _animator.SetBool("isAttacking", true);
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
        }
            
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            CanAttack = false;
            _animator.SetBool("isAttacking", false);
            isAttacking = false;
            CurrentState = RobotStates.patrol;
            Patrol();
        }
    }
   

    public void Attack(Transform objectiu)
    {

        if (CanAttack)
        {
            shooter.Shoot(objectiu);
        }
    }
    public void Patrol()
    {

        if (!isAttacking)
        {


            Vector3 CurrentPoint = route[PatrolIndex % route.Length].position;
            CurrentPoint.y = 0f;

            Vector3 posToGoTo = new Vector3(CurrentPoint.x, transform.position.y, CurrentPoint.z);
            transform.position = Vector3.MoveTowards(transform.position, posToGoTo, 0.45f * Time.deltaTime);
            transform.LookAt(posToGoTo);
            if (Vector3.Distance(transform.position, posToGoTo) < 0.05f)
            {
                PatrolIndex++;
            }

        }
        
    }
   
    public void SyncShootAnimation()
    {
        CanAttack = true;
    }
}
