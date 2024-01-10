using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class charactorMovement : MonoBehaviour
{
    Animator animator;
    CharacterController characterController;
    public float speed = 6.0f;
    public float rotationSpeed = 25;
    public float jumpSpeed = 7.5f;
    public float gravicty =20.0f;
    Vector3 inputVec;
    Vector3 targetDirection;
    private Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
        Time.timeScale = 1;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = -Input.GetAxisRaw("Vertical");
        float z = Input.GetAxisRaw("Horizontal");
        inputVec = new Vector3(x,0,z);
        
        animator.SetFloat("Input X",z);
        animator.SetFloat("Input Z",-(x));

        if (x!=0 || z!=0)
        {
            animator.SetBool("Moving",true);
            animator.SetBool("Running",true);
        }else
        {
            animator.SetBool("Moving",false);
            animator.SetBool("Running",false);
        }
        //jumping
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"),0.0f,Input.GetAxis("Vertical"));
            moveDirection *= speed;  
        }        
        characterController.Move(moveDirection * Time.deltaTime);
        UpdateMovement();
    }
    
    private void UpdateMovement() {
        Vector3 motion = inputVec;
        motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1)?.7f:1;
        RotateTWMovemeDR();
        getCameraReative();
    }

    void RotateTWMovemeDR(){
        if (inputVec != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(targetDirection),Time.deltaTime *rotationSpeed);
        }
    }
    void getCameraReative(){
        Transform cameraTranform = Camera.main.transform;
        Vector3 forward = cameraTranform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z,0,-forward.x);
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        targetDirection = (h * right) + (v * forward);
    }
}