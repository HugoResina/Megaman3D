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
    private BossStates CurrentState;
    private Transform LastPlayerPosition;
    [SerializeField]
    private Transform LookPoint;
    private float shootDistance = 20f;
    private float attackDistance;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    void Update()
    {

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
        if(other.gameObject.layer == 3)
        {
            LastPlayerPosition = other.transform;
            RaycastHit hit;
            Vector3 direction = (other.transform.position - LookPoint.position);
            Physics.Raycast(LookPoint.position, direction, out hit);
            if(hit.distance < shootDistance && hit.distance > attackDistance)
            {
                CurrentState = BossStates.Shoot;
            }

        }
    }
    private void Attack()
    {
        //animacion
        //
    }

    private void Shoot()
    {
        
        _animator.SetBool("isShooting", true);
       
        transform.LookAt(LastPlayerPosition);


    }
    private void Chase()
    {

    }
    private void Charge()
    {

    }
    private void LeftFistDamage()
    {

    }
    private void RightFistDamage()
    {

    }
    private void Idle()
    {

    }

}
