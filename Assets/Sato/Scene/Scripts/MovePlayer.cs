using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private Vector3 moveDirection;
    private Vector3 addForceDownPower = Vector3.zero;
    private CharacterController controller;

    private float speed;
    private float rotateSpeed;
    private float gravity;
    private float jumpPower;

    // Start is called before the first frame update
    void Start()
    {
        this.rotateSpeed = 60.0f;
        this.gravity = 10.0f;
        this.jumpPower = 6;
        this.speed = 6.0f;

        this.controller = this.gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        this.addForceDownPower = Vector3.zero;
        if(controller.isGrounded)
        {
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0);
            this.moveDirection = transform.forward * speed * Input.GetAxis("Vertical");
            if(Input.GetButtonDown("Jump"))
            {
                this.addForceDownPower = Vector3.zero;
                moveDirection.y = this.jumpPower;
            }
        }
        else
        {
            this.moveDirection.y -= this.gravity * Time.deltaTime;
        }
        controller.Move(this.moveDirection * Time.deltaTime + this.addForceDownPower);
    }
}
