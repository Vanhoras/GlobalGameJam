using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    private Player1InputActions inputActions1;
    private Player2InputActions inputActions2;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCast;

    private PlayerMetadata _player;

    private bool _canJump;
    private bool _canWalk;

    private bool _isWalk;
    private bool _isJump;

    private bool _isMirrored;
    
    private float _gunRotation;
    private float _startScale;
    private Rigidbody2D _playerRigidbody;
    private RaycastHit2D[] _hit;

    void Start()
    {
        _player = GetComponent<PlayerMetadata>();

        inputActions1 = new Player1InputActions();
        inputActions2 = new Player2InputActions();

        if (_player.Player == Player.Player1)
        {
            inputActions1.Player.Enable();
            inputActions1.Player.Jump.performed += OnJump;
        }
        else
        {
            inputActions2.Player.Enable();
            inputActions2.Player.Jump.performed += OnJump;
        }

        _playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        _startScale = transform.localScale.x;
       
        _canJump = true;
    }

    private void OnDestroy()
    {
        inputActions1.Player.Shoot.performed -= OnJump;
        inputActions2.Player.Shoot.performed -= OnJump;
    }

    private void OnJump(InputAction.CallbackContext input)
    {

        _hit = Physics2D.LinecastAll(new Vector2(groundCast.position.x, groundCast.position.y + 0.2f), new Vector2(groundCast.position.x, groundCast.position.y - 0.2f));

        foreach (RaycastHit2D hit in _hit)
        {
            if (hit.transform.tag == "Ground")
            {
                _canJump = true;

                break;
            }
        }

        if (!_canJump) return;

        _canJump = false;
        _canWalk = false;
        _isJump = true;
    }

    void FixedUpdate()
    {
        Mirror();

        Move();

        if (_isJump)
        {
            _playerRigidbody.AddForce(new Vector2(0, jumpForce));
            // TODO: Play Jump Animation
            _isJump = false;
        }
    }

    private void Mirror()
    {
        Vector2 inputVector;
        if (_player.Player == Player.Player1)
        {
            Vector3 gampadVector = inputActions1.Player.Aim.ReadValue<Vector2>();
            inputVector = new Vector2(gampadVector.x, gampadVector.y);
        }
        else
        {
            Vector2 mousePosition = inputActions2.Player.Aim.ReadValue<Vector2>();
            inputVector = Camera.main.ScreenToWorldPoint(mousePosition);
            inputVector.Normalize();
        }

        if (inputVector.x > transform.position.x + 0.2f)
            _isMirrored = false;
        if (inputVector.x < transform.position.x - 0.2f)
            _isMirrored = true;

        if (!_isMirrored)
        {
            _gunRotation = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;
            transform.localScale = new Vector3(_startScale, _startScale, 1);
        }
        if (_isMirrored)
        {
            _gunRotation = Mathf.Atan2(-inputVector.y, -inputVector.x) * Mathf.Rad2Deg;
            transform.localScale = new Vector3(-_startScale, _startScale, 1);
        }
    }


    private void Move()
    {
        Vector2 inputVector;
        if (_player.Player == Player.Player1)
        {
            inputVector = inputActions1.Player.Move.ReadValue<Vector2>();
        }
        else
        {
            inputVector = inputActions2.Player.Move.ReadValue<Vector2>();
        }

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
