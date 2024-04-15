using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    [Tooltip("how far to the left or right will the cat move")]
    [SerializeField] private float horizontalMovementDelta;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        //initialize start position
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            transform.position = startPos - (Vector3.right * horizontalMovementDelta);
        }
    }
}
