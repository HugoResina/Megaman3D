using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInputs))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera;

    [Header("Movement Settings")]
    public float moveSpeed = 3.0f;
    public float sprintSpeed = 6.0f;

    [Header("Look Settings")]
    public float lookSensitivityH = 0.1f;
    public float lookSensitivityV = 0.1f;
    public float looklimitV = 80.0f;

    [Header("Gravity Settings")]
    public float gravity = 9.81f;

    private PlayerInputs _playerInputs;
    private Vector3 _currentMovement = Vector3.zero;
    private Vector2 _cameraRotation = Vector2.zero;
    private bool _isGrounded;
    private bool _isGrabbedToWall;
    [Header("Jump Settings")]
    public float jumpForce = 5f;

    private float verticalVelocity = -1f;
    public float jumpCutMultiplier = 0.5f;

    public LayerMask JumpableLayer;




    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
    

    private void Update()
    {
            IsGroundedAndWalls();
            
            Jump();
            CutJump();
            Movement();
            
            Debug.Log(verticalVelocity);
    }


    private void IsGroundedAndWalls()
    {

        RaycastHit hit;
        _isGrabbedToWall = Physics.Raycast(transform.position, transform.forward, out hit, 2f, JumpableLayer);
        _isGrounded = ( _isGrabbedToWall || _characterController.isGrounded);
        


    }
    private void LateUpdate()
    {
       
            Look();
    }

    private void Movement()
    {
        float speed = moveSpeed;

        float horizontal = _playerInputs.MoveInput.x * speed;
        float vertical = _playerInputs.MoveInput.y * speed;

        Vector3 move = new Vector3(horizontal, 0, vertical);
        move = transform.rotation * move;

      
        
       
            
            verticalVelocity -= 0.05f;
            if (verticalVelocity <= -2f && _isGrabbedToWall && !_isGrounded)
            {
                verticalVelocity = -2f;
            }
            else if (verticalVelocity <= -10f)
            {
            verticalVelocity = -10f; 
            }



            move.y = verticalVelocity;

        _characterController.Move(move * Time.deltaTime);
    }

    private void Look()
    {
        _cameraRotation.x = _playerInputs.LookInput.x * lookSensitivityH;
        _cameraRotation.y -= _playerInputs.LookInput.y * lookSensitivityV;
        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y, -looklimitV, looklimitV);

        transform.Rotate(0, _cameraRotation.x, 0);
        _playerCamera.transform.localRotation = Quaternion.Euler(_cameraRotation.y, 0, 0);
    }
    private void Jump()
    {
     

        if (_isGrounded && _playerInputs.JumpInput > 0)
        {
            Debug.Log("puc saltar: ");
            float a = _isGrabbedToWall ? jumpForce/2 : jumpForce; 
            verticalVelocity = Mathf.Sqrt(a * 2f * gravity);
            Debug.Log(jumpForce);
           
        }
        
    }
    private void CutJump()
    {
    
        if (verticalVelocity > 0 && !_playerInputs.isJumpHeld)
        {
            verticalVelocity *= jumpCutMultiplier;
        }
    }


}