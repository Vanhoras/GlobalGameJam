using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private static readonly int s_isMoving = Animator.StringToHash("IsMoving");

    public float PlayerSpeed { get; set; }
    public Vector2 FacingDirection { get; set; }
    public bool IsFacingLeft { get; set; }

    private bool m_isFacingLeft = false;
    private static readonly int s_facingDirection = Animator.StringToHash("FacingDirection");
    private static readonly int s_moveSpeed = Animator.StringToHash("MoveSpeed");

    public void TriggerShootAnimation()
    {
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Mathf.Abs(PlayerSpeed) < 0.1f)
        {
            _animator.SetBool(s_isMoving, false);
        }
        else
        {
            _animator.SetBool(s_isMoving, true);
        }

        if (IsFacingLeft != m_isFacingLeft)
        {
            FlipVisuals();
        }

        m_isFacingLeft = IsFacingLeft;
        _animator.SetFloat(s_facingDirection, CalculateAnimatorDirectionParameter(FacingDirection, IsFacingLeft));
        _animator.SetFloat(s_moveSpeed, PlayerSpeed);
    }

    private void FlipVisuals()
    {
        var transform1 = transform;
        Vector3 scale = transform1.localScale;
        scale.x *= -1;
        transform1.localScale = scale;
    }

    private float CalculateAnimatorDirectionParameter(Vector2 direction, bool isFacingLeft)
    {
        //Calculate te coresponding angle of the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        //If the player is facing left, angles between 90 and 270 are mapped to 1 to 0.
        //If the player is facing right, angles between -90 and 90 are mapped to 1 to 0.
        
        if (isFacingLeft)
        {
            if (angle < 0)
            {
                angle += 360;
            }

            if (angle > 90 && angle < 270)
            {
                return 1 - (angle - 90) / 180;
            }
            else
            {
                return 1 + angle / 180;
            }
        }
        else
        {
            if (angle < 0)
            {
                angle += 360;
            }

            if (angle > 90 && angle < 270)
            {
                return 1 + (angle - 90) / 180;
            }
            else
            {
                return 1 - angle / 180;
            }
        }
    }
}