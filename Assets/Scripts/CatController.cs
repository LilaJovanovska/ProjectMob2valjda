using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CatController : MonoBehaviour
{
    [Tooltip("How far to the left or right will cat move?")]
    [SerializeField] private float horizontalMovementDelta;
    [Tooltip("How fast does the character move horizontally")]
    [SerializeField] private float movementInterval;
    [SerializeField] private Ease movementEase;
    [SerializeField] private Animator anim;


    private Vector3 centerPos;

    private PositionState characterPosition;
    private Vector3 targetPos;
    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        // initialize start position
        centerPos = transform.position;

        characterPosition = PositionState.Center;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        JumpAndSlide();

    }

    private void Movement()
    {
        // moving left
        if (Input.GetKeyDown(KeyCode.LeftArrow) && characterPosition != PositionState.Left && isMoving == false)
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
        }

        // moving right
        if (Input.GetKeyDown(KeyCode.RightArrow) && characterPosition != PositionState.Right && isMoving == false)
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
        }
    }

    private void JumpAndSlide()
    {
        // moving left
        if (Input.GetKeyDown(KeyCode.UpArrow) && isMoving == false)
        {
            isMoving = true;

            // trigger jump animation
            anim.ResetTrigger("Jump");
            anim.SetTrigger("Jump");

            isMoving = false;
        }

        // moving right
        if (Input.GetKeyDown(KeyCode.DownArrow) && isMoving == false)
        {
            isMoving = true;

            //trigger slide anim
            anim.ResetTrigger("Slide");
            anim.SetTrigger("Slide");

            isMoving = false;
        }
    }

    private enum PositionState
    {
        Left,
        Right,
        Center
    }
}
