using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    
    private Player1InputActions inputActions;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCast;
    [SerializeField] private Camera mainCamera;

    private bool _canJump;
    private bool _canWalk;

    private bool _isWalk;
    private bool _isJump;

    private bool _isMirrored;

    private float _gunRotation;
    private float _startScale;
    private Rigidbody2D _playerRigidbody;
    private RaycastHit2D _hit;

    void Start()
    {
        inputActions = new Player1InputActions();
        inputActions.Player.Enable();

        _playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        _startScale = transform.localScale.x;
    }

    void Update()
    {
        if (_hit = Physics2D.Linecast(new Vector2(groundCast.position.x, groundCast.position.y + 0.2f), groundCast.position))
        {
            if (!_hit.transform.CompareTag("Player"))
            {
                _canJump = true;
                _canWalk = true;
            }
        }
        else _canJump = false;
    }


    private void OnJump()
    {
        Debug.Log("OnJump");
        _canWalk = false;
        _isJump = true;
    }

    void FixedUpdate()
    {
        Rotate();

        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        Move(inputVector);

        if (_isJump)
        {
            _playerRigidbody.AddForce(new Vector2(0, jumpForce));
            // TODO: Play Jump Animation
            _canJump = false;
            _isJump = false;
        }
    }

    private void Rotate()
    {
        Vector2 inputVector = inputActions.Player.Aim.ReadValue<Vector2>();
        Vector2 mousePosition = playerCamera.ScreenToWorldPoint(inputVector) - gun.transform.position;

        mousePosition.Normalize();

        if (playerCamera.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x + 0.2f)
            _isMirrored = false;
        if (mainCamera.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x - 0.2f)
            _isMirrored = true;

        if (!_isMirrored)
        {
            _gunRotation = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
            transform.localScale = new Vector3(_startScale, _startScale, 1);
        }
        if (_isMirrored)
        {
            _gunRotation = Mathf.Atan2(-mousePosition.y, -mousePosition.x) * Mathf.Rad2Deg;
            transform.localScale = new Vector3(-_startScale, _startScale, 1);
        }
    }


    private void Move(Vector2 inputVector)
    {
        if (inputVector.x != 0)
        {
            _playerRigidbody.velocity = new Vector2(inputVector.x * walkSpeed * Time.deltaTime, _playerRigidbody.velocity.y);

            if (_canWalk)
            {
                // TODO: Play Walk Animation
            }
        }
        else
        {
            _playerRigidbody.velocity = new Vector2(0, _playerRigidbody.velocity.y);
        }
    }

    public bool IsMirror()
    {
        return _isMirrored;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, groundCast.position);
    }
}
