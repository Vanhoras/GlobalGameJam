using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public float PlayerSpeed { get; set; }
    public Vector2 FacingDirection { get; set; }
    public bool IsFacingLeft { get; set; } 
    private static readonly int s_isMoving = Animator.StringToHash("IsMoving");
    private static readonly int s_facingDirection = Animator.StringToHash("FacingDirection");
    private static readonly int s_moveSpeed = Animator.StringToHash("MoveSpeed");
    private static readonly int s_shoot = Animator.StringToHash("Shoot");
    private static readonly int s_moveDirection = Animator.StringToHash("MoveDirection");

    public void TriggerShootAnimation()
    {
        _animator.SetTrigger(s_shoot);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateDirection();
        _animator.SetFloat(s_facingDirection, CalculateAnimatorDirectionParameter(FacingDirection, IsFacingLeft));
        _animator.SetFloat(s_moveSpeed, Mathf.Abs(PlayerSpeed));
        var isMoving = Mathf.Abs(PlayerSpeed) >= 0.1f;
        _animator.SetBool(s_isMoving, isMoving);
        if (PlayerSpeed < 0)
        {
            _animator.SetFloat(s_moveDirection, -1);
        }
        else
        {
            _animator.SetFloat(s_moveDirection, 1);
        }
    }

    private void UpdateDirection()
    {
        var transform1 = transform;
        Vector3 scale = transform1.localScale;

        if (IsFacingLeft)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x);
        }

        transform1.localScale = scale;
    }

    private float CalculateAnimatorDirectionParameter(Vector2 direction, bool isFacingLeft)
    {
        //Calculate te coresponding angle of the direction vector
        //Depending on the angle:
        //If the player is facing left, angles between 90 and 270 are mapped to 1 to 0.
        //If the player is facing right, angles between -90(270) and 90 are mapped to 0 to 1.
        
        // Calculate the angle of the direction vector in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Normalize the angle to a range of [0, 360)
        angle = (angle + 360) % 360;

        if (isFacingLeft)
        {
            // Map angles between 90 and 270 to the range [1, 0]
            if (angle >= 90 && angle <= 270)
            {
                return Mathf.InverseLerp(270, 90, angle);
            }
        }
        else
        {
            // Map angles between -90 (or 270) and 90 to the range [0, 1]
            if (angle <= 90 || angle >= 270)
            {
                if (angle >= 270)
                {
                    angle -= 360; // Map 270-360 range to -90-0
                }
                return Mathf.InverseLerp(-90, 90, angle);
            }
        }

        // Default to 0 for angles outside the mapped range
        return 0f;
    }
}