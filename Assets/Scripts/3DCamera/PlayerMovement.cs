using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    [SerializeField] float _speed = 5f;
    [SerializeField] LayerMask _aimLayerMask;
    
    Animator _animator;

    [SerializeField]
    private InputActionReference _movementControl;
    [SerializeField]
    private InputActionReference _lookControl;


    private void OnEnable()
    {
        _movementControl.action.Enable();
        _lookControl.action.Enable();
    }


    private void OnDisable()
    {
        _movementControl.action.Disable();
        _lookControl.action.Disable();
    }


    void Awake()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
    }


    void Update()
    {
        AimTowardMouse();

        // Reading the Input
        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");

        Vector2 movement_tmp = _movementControl.action.ReadValue<Vector2>();
        float horizontal = movement_tmp.x;
        float vertical = movement_tmp.y;

        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        // Moving
        if (movement.magnitude > 0)
        {
            movement.Normalize();
            movement *= _speed * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }

        // Animating
        float velocityZ = Vector3.Dot(movement.normalized, transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, transform.right);
        
        _animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        _animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
    }


    void AimTowardMouse()
    {

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector2 mousePosition = _lookControl.action.ReadValue<Vector2>();
        //Debug.Log("mousePosition: " + mousePosition);

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _aimLayerMask))
        {

            var direction = hitInfo.point - transform.position;
            direction.y = 0f;
            direction.Normalize();
            transform.forward = direction;

        }

    }

}
