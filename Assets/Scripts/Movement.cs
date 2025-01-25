using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
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
    private Vector2 _inputAxis;
    private RaycastHit2D _hit;

    void Start()
    {
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
        else
        {
            _canJump = false;
        };

        _inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (_inputAxis.y > 0 && _canJump)
        {
            _canWalk = false;
            _isJump = true;
        }
    }

    void FixedUpdate()
    {
        if (mainCamera.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x + 0.2f)
            _isMirrored = false;
        if (mainCamera.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x - 0.2f)
            _isMirrored = true;

        if (!_isMirrored)
        {
            transform.localScale = new Vector3(_startScale, _startScale, 1);
        }
        if (_isMirrored)
        {
            transform.localScale = new Vector3(-_startScale, _startScale, 1);
        }

        if (_inputAxis.x != 0)
        {
            _playerRigidbody.velocity = new Vector2(_inputAxis.x * walkSpeed * Time.deltaTime, _playerRigidbody.velocity.y);

            if (_canWalk)
            {
                // TODO: Play Walk Animation
            }
        }
        else
        {
            _playerRigidbody.velocity = new Vector2(0, _playerRigidbody.velocity.y);
        }

        if (_isJump)
        {
            _playerRigidbody.AddForce(new Vector2(0, jumpForce));
            // TODO: Play Jump Animation
            _canJump = false;
            _isJump = false;
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
