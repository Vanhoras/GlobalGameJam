using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    private Player1InputActions inputActions1;
    private Player2InputActions inputActions2;

    [SerializeField] private float walkForce;
    [SerializeField] private float maxWalkSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float drag;
    [SerializeField] private float snapForce;
    [SerializeField] private Transform groundCast;
    [SerializeField] private DirectionE initialDirection;

    [SerializeField] private PlayerAnimationManager animationManager;

    private PlayerMetadata _player;

    private bool _canJump;
    private bool _canWalk;

    private bool _isWalk;
    private bool _isJump;

    private DirectionE _currentDirection;
    
    private float _gunRotation;
    private float _startScale;
    private Rigidbody2D _playerRigidbody;
    private RaycastHit2D[] _hit;

    Vector2 lastFacedDirection = Vector2.zero;

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

        LookAtDirection(initialDirection);
    }

    private void OnDestroy()
    {
        inputActions1.Player.Jump.performed -= OnJump;
        inputActions2.Player.Jump.performed -= OnJump;
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

        SoundController.Instance.PlaySound(SfxIdentifier.Jump);
    }

    void FixedUpdate()
    {
        DetermineDirection();
        Move();

        if (_isJump)
        {
            _playerRigidbody.AddForce(new Vector2(0, jumpForce));
            // TODO: Play Jump Animation
            _isJump = false;
        }
    }

    private void DetermineDirection()
    {
        DirectionE direction;

        Vector2 inputVector;
        if (_player.Player == Player.Player1)
        {
            Vector3 gampadVector = inputActions1.Player.Aim.ReadValue<Vector2>();
            inputVector = new Vector2(gampadVector.x, gampadVector.y);

            if (inputVector.x == 0 && inputVector.y == 0)
            {
                inputVector = lastFacedDirection;
            }
        }
        else
        {
            Vector2 mousePosition = inputActions2.Player.Aim.ReadValue<Vector2>();
            Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            inputVector = worldMousePosition - (Vector2)transform.position;
        }

        if (inputVector.x > 0)
        {
            direction = DirectionE.Right;
        }
        else if (inputVector.x < 0)
        {
            direction = DirectionE.Left;
        }
        else
        {
            direction = _currentDirection;
        }

        animationManager.FacingDirection = inputVector;

        lastFacedDirection = inputVector;

        LookAtDirection(direction);
    }

    private void LookAtDirection(DirectionE direction)
    {
        _currentDirection = direction;

        animationManager.IsFacingLeft = _currentDirection == DirectionE.Left;
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
            bool velocityPositive = _playerRigidbody.velocity.x > 0;
            bool velocityNegative = _playerRigidbody.velocity.x < 0;

            if ( velocityPositive && inputVector.x < 0)
            {
                _playerRigidbody.velocity = new Vector2(Mathf.Min(_playerRigidbody.velocity.x, 1), _playerRigidbody.velocity.y);
            } else if (velocityNegative && inputVector.x > 0)
            {
                _playerRigidbody.velocity = new Vector2(Mathf.Max(_playerRigidbody.velocity.x, -1), _playerRigidbody.velocity.y);
            }


            _playerRigidbody.AddForce(new Vector2(inputVector.x * walkForce, 0));
        } else
        {
            if (Mathf.Abs(_playerRigidbody.velocity.x) <= snapForce)
            {
                _playerRigidbody.velocity = new Vector2(0, _playerRigidbody.velocity.y);
            } else {
                float dragForce = -1 * drag * _playerRigidbody.velocity.x * _playerRigidbody.velocity.magnitude;
                _playerRigidbody.AddForce(new Vector2(dragForce, 0));
            }
        }

        if (Mathf.Abs(_playerRigidbody.velocity.x) >= maxWalkSpeed)
        {
            int direction = _playerRigidbody.velocity.x > 0 ? 1 : -1;
            _playerRigidbody.velocity = new Vector2(direction * maxWalkSpeed, _playerRigidbody.velocity.y);
        }

        animationManager.PlayerSpeed = _playerRigidbody.velocity.x;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, groundCast.position);
    }

    public void Knockback(float force, int direction)
    {
        _playerRigidbody.AddForce(new Vector2(direction * force, 0));
    }
}
