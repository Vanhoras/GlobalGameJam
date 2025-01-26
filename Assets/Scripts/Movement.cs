using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        }
        else
        {
            Vector2 mousePosition = inputActions2.Player.Aim.ReadValue<Vector2>();
            Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            inputVector = worldMousePosition - (Vector2)transform.position;
        }

        if (inputVector.x > 0.1)
        {
            direction = DirectionE.Right;
        }
        else if (inputVector.x < -0.1)
        {
            direction = DirectionE.Left;
        }
        else
        {
            direction = _currentDirection;
        }

        LookAtDirection(direction);
    }

    private void LookAtDirection(DirectionE direction)
    {
        _currentDirection = direction;

        if (direction == DirectionE.Left)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (direction == DirectionE.Right)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
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

        float healthSlowdown = Mathf.Lerp(0.6f, 1, (((float)_player.Health.CurrentHealth) / _player.Health.MaxHealth));

        if (inputVector.x != 0)
        {
            _playerRigidbody.AddForce(new Vector2(inputVector.x * walkForce * healthSlowdown, 0));

            if (_canWalk)
            {
                // TODO: Play Walk Animation
            }
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

        if (Mathf.Abs(_playerRigidbody.velocity.x) >= maxWalkSpeed * healthSlowdown)
        {
            int direction = _playerRigidbody.velocity.x > 0 ? 1 : -1;
            _playerRigidbody.velocity = new Vector2(direction * maxWalkSpeed * healthSlowdown, _playerRigidbody.velocity.y);
        }
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
