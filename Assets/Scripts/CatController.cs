using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SimpleInputNamespace;

public class CatController : MonoBehaviour
{
    [Tooltip("How far to the left or right will cat move?")]
    [SerializeField] private float horizontalMovementDelta;
    [Tooltip("How fast does the character move horizontally")]
    [SerializeField] private float movementInterval;
    [SerializeField] private Ease movementEase;
    [SerializeField] private Ease rotationEase;
    [SerializeField] private Animator anim;

    [SerializeField] private Transform catCollider;

    [SerializeField] private float colliderJumpPos;
    [SerializeField] private float jumpInterval;
    [SerializeField] private Ease jumpUpEase;
    [SerializeField] private Ease jumpDownEase;

    [SerializeField] private float colliderSlidePos;
    [SerializeField] private float slideInterval;
    [SerializeField] private Ease slideDownEase;
    [SerializeField] private Ease slideUpEase;

    [SerializeField] private float swipeThresholdDelta;


    private Vector3 centerPos;

    private PositionState characterPosition;
    private Vector3 targetPos;
    private bool isMoving;
    private Vector3 originalColPos;
    private Vector2 touchStartPos;

    // Start is called before the first frame update
    void Start()
    {
        // initialize start position
        centerPos = transform.position;

        characterPosition = PositionState.Center;
        originalColPos = catCollider.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMovement();
        JumpAndSlide();
    }

    private void HorizontalMovement()
    {
        // moving left
        if (GetMoveLeftInput() && characterPosition != PositionState.Left && isMoving == false)
        {
            isMoving = true;

            if (characterPosition == PositionState.Center)
            {
                characterPosition = PositionState.Left;
                targetPos = centerPos - (Vector3.right * horizontalMovementDelta);
            }
            else if (characterPosition == PositionState.Right)
            {
                characterPosition = PositionState.Center;
                targetPos = centerPos;
            }

            // teleportation code
            // transform.position = targetPos;
            // animation code
            transform.DOMove(targetPos, movementInterval).SetEase(movementEase).OnComplete(() =>
            {
                Debug.Log("Cat reached its destination");
                isMoving = false;
            });

            // you can rotate to ()
            // or you can rotate for (additive)

            transform.DOLocalRotate(Vector3.up * -90, movementInterval / 2).SetEase(rotationEase).OnComplete(
                () =>
                {
                    transform.DOLocalRotate(Vector3.zero, movementInterval / 2f).SetEase(rotationEase);
                }
            );
        }

        // moving right
        if (GetMoveRightInput() && characterPosition != PositionState.Right && isMoving == false)
        {
            isMoving = true;

            if (characterPosition == PositionState.Center)
            {
                characterPosition = PositionState.Right;
                targetPos = centerPos + (Vector3.right * horizontalMovementDelta);
            }
            else if (characterPosition == PositionState.Left)
            {
                characterPosition = PositionState.Center;
                targetPos = centerPos;
            }

            // teleportation code 
            // transform.position = targetPos;
            // animation code
            transform.DOMove(targetPos, movementInterval).SetEase(movementEase).OnComplete(() =>
            {
                Debug.Log("Cat reached its destination");
                isMoving = false;
            });

            transform.DOLocalRotate(Vector3.up * 90, movementInterval / 2f).SetEase(rotationEase).OnComplete(
                () =>
                {
                    transform.DOLocalRotate(Vector3.zero, movementInterval / 2f).SetEase(rotationEase);
                }
            );
        }
    }

    private void JumpAndSlide()
    {

        // jumping
        if (Input.GetKeyDown(KeyCode.UpArrow) && isMoving == false)
        {
            isMoving = true;

            // trigger jump animation
            anim.ResetTrigger("Jump");
            anim.SetTrigger("Jump");

            // animating the position of the collider
            catCollider.DOLocalMove(Vector3.up * colliderJumpPos, jumpInterval / 2).SetEase(jumpUpEase).OnComplete(
                () =>
                {
                    catCollider.DOLocalMove(originalColPos, jumpInterval / 2).SetEase(jumpDownEase).OnComplete(
                        () =>
                        {
                            isMoving = false;
                        }
                    );
                }
            );
        }

        // slide
        if (Input.GetKeyDown(KeyCode.DownArrow) && isMoving == false)
        {
            isMoving = true;

            // trigger slide animation
            anim.ResetTrigger("Slide");
            anim.SetTrigger("Slide");


            // animating the position of the collider (0.104)
            catCollider.DOLocalMove(Vector3.up * colliderSlidePos, slideInterval / 2).SetEase(slideDownEase).OnComplete(
                () =>
                {
                    catCollider.DOLocalMove(originalColPos, slideInterval / 2).SetEase(slideUpEase).OnComplete(
                        () =>
                        {
                            isMoving = false;
                        }
                    );
                }
            );

        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool GetMoveLeftInput()
    {
        if (SimpleInput.GetAxis("Horizontal") < 0)
        {
            return true;
        }

        return false;

/*#if UNITY_EDITOR || UNITY_STANDALONE
        // Debug.Log("Code for Windows");
        return Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
#elif UNITY_ANDROID
 
        // get input for mobile
        if (Input.touchCount > 0)
        {
            Touch firstTouch = Input.GetTouch(0);
 
            if (firstTouch.phase == TouchPhase.Began)
            {
                touchStartPos = firstTouch.position;
            }
            else if (firstTouch.phase == TouchPhase.Moved)
            {
                // check if player swiped to the left
                Vector2 currentTouchPos = firstTouch.position;
                float horizontalDelta = currentTouchPos.x - touchStartPos.x;
                if (horizontalDelta < 0 && Mathf.Abs(horizontalDelta) > swipeThresholdDelta) 
                {
                    // player swiped to the left
                    return true;
                }
            }
 
        }
 
        return false;
        Debug.Log("Code for android");
#endif*/
    }

    private bool GetMoveRightInput()
    {
        if (SimpleInput.GetAxis("Horizontal") > 0)
        {
            return true;
        }

        return false;
    }

    private enum PositionState
    {
        Left,
        Right,
        Center
    }
}
