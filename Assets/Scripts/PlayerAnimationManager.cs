using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    //[SerializeField] private SpriteLibrary _spriteLibrary;

    public float PlayerSpeed { get; set; }
    public Vector2 FacingDirection { get; set; }
    public bool IsFacingLeft { get; set; } 
    private static readonly int s_isMoving = Animator.StringToHash("IsMoving");
    private static readonly int s_facingDirection = Animator.StringToHash("FacingDirection");
    private static readonly int s_moveSpeed = Animator.StringToHash("MoveSpeed");
    private static readonly int s_shoot = Animator.StringToHash("Shoot");
    private static readonly int s_moveDirection = Animator.StringToHash("MoveDirection");

    private float m_lastValidFacingDirectionParameterValue = 0.5f;

    public void TriggerShootAnimation()
    {
        _animator.SetTrigger(s_shoot);
    }

    private void Start()
    {
        /*//Find all sprite skin components in the children
        var spriteSkins = GetComponentsInChildren<SpriteSkin>();
        foreach (var spriteSkin in spriteSkins)
        {
            //user reflection to set "autoRebind" property on spriteSkin to false
            var spriteSkinType = spriteSkin.GetType();
            var autoRebindProperty = spriteSkinType.GetProperty("autoRebind");
            if (autoRebindProperty != null)
            {
                autoRebindProperty.SetValue(spriteSkin, false);
            }
        }

        yield return new WaitForSeconds(0.2f);
        
        foreach (var spriteSkin in spriteSkins)
        {
            //user reflection to set "autoRebind" property on spriteSkin to false
            var spriteSkinType = spriteSkin.GetType();
            var autoRebindProperty = spriteSkinType.GetProperty("autoRebind");
            if (autoRebindProperty != null)
            {
                autoRebindProperty.SetValue(spriteSkin, true);
            }
        }*/

        /*var currentSpriteLibrary = _spriteLibrary.spriteLibraryAsset;
        _spriteLibrary.spriteLibraryAsset = null;
        yield return new WaitForSeconds(0.2f);;
        
        _spriteLibrary.spriteLibraryAsset = currentSpriteLibrary;*/
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateDirection();
        var directionValue = CalculateAnimatorDirectionParameter(FacingDirection, IsFacingLeft);
        
        _animator.SetFloat(s_facingDirection, directionValue);
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

        float output = -1;
        
        // Calculate the angle of the direction vector in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        

        // Normalize the angle to a range of [0, 360)
        angle = (angle + 360) % 360;
        if (Math.Abs(angle - 90) < 1)
        {
            output = 1;
        }
        else if (isFacingLeft)
        {
            // Map angles between 90 and 270 to the range [1, 0]
            if (angle >= 90 && angle <= 270)
            {
                output = 1f - (angle - 90f) / 180f; // Linearly interpolate from 1 (at 90) to 0 (at 270)
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
                output = (angle + 90f) / 180f; // Linearly interpolate from 0 (at -90) to 1 (at 90)
            }
        }
        
        if(output == -1)
            output = m_lastValidFacingDirectionParameterValue;
        else
            m_lastValidFacingDirectionParameterValue = output;
        
        return output;
    }
}