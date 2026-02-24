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
    public float gravity = 12f;

    private PlayerInputs _playerInputs;
    private Vector3 _currentMovement = Vector3.zero;
    private Vector2 _cameraRotation = Vector2.zero;
    private bool _isGrounded;
    private bool _isGrabbedToWall;
    private bool _isHoveringItem;
    [Header("Jump Settings")]
    public float jumpForce = 7f;

    private float verticalVelocity = -1f;
    public float jumpCutMultiplier = 0.5f;


    bool isItem = false;
    bool isDoor = false;
    public LayerMask JumpableLayer;
    public LayerMask ItemLayer;
    public LayerMask DoorLayer;

    RaycastHit hit;
    RaycastHit a;




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
    private void OnEnable()
    {
        PlayerInputs.HasInteracted += Interact;
    }
    private void OnDisable()
    {
        PlayerInputs.HasInteracted -= Interact;
    }
    private void FixedUpdate()
    {
        
    }
    private void Update()
    {
        IsGroundedAndWalls();

        Jump();
        CutJump();

        Movement();

    }


    private void IsGroundedAndWalls()
    {

     
        _isGrabbedToWall = Physics.Raycast(transform.position, transform.forward, out hit, 3f, JumpableLayer);
        _isGrounded = (_isGrabbedToWall || _characterController.isGrounded);



    }
    private void LateUpdate()
    {

        Look();
    }

    private void Movement()
    {
       

        Vector3 moveDir = new Vector3(_playerInputs.MoveInput.x, 0, _playerInputs.MoveInput.y);
        moveDir = transform.TransformDirection(moveDir) * moveSpeed;

        if (_isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity -= gravity * Time.deltaTime;

        if (verticalVelocity < -20f) verticalVelocity = -20f;

        Vector3 finalVelocity = moveDir;
        finalVelocity.y = verticalVelocity;

        _characterController.Move(finalVelocity * Time.deltaTime);
    }

    private void Look()
    {
        _cameraRotation.x = _playerInputs.LookInput.x * lookSensitivityH;
        _cameraRotation.y -= _playerInputs.LookInput.y * lookSensitivityV;
        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y, -looklimitV, looklimitV);

        transform.Rotate(0, _cameraRotation.x, 0);
        _playerCamera.transform.localRotation = Quaternion.Euler(_cameraRotation.y, 0, 0);
    }

    private void Interact()
    {
        Debug.Log("interactuo");
      
        if (isItem)
        {

            bool hititem;
            hititem = Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out a, 3f, ItemLayer);
            Debug.Log("Interactuo con item");
            a.transform.gameObject.SetActive(false);
            GayManager.Instance.HasKey = true;
        }
        else if (isDoor)
        {
            if (GayManager.Instance.HasKey)
            {
                Debug.Log("Abro");
            }
            else if(!GayManager.Instance.HasKey)
            {
                Debug.Log("Falta llave");
            }
        }
    }
    private void Jump()
    {


        if (_isGrounded && _playerInputs.JumpInput > 0)
        {
            Debug.Log("puc saltar: ");
            float a = _isGrabbedToWall ? jumpForce / 2 : jumpForce;
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