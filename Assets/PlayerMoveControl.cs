using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMoveControl : MonoBehaviour
{

    [SerializeField] private float maxMoveRunSpeed = 2.0f;    // 移動速度
    [SerializeField] private float maxMoveWalkSpeed = 1.0f;    // 移動速度
    [SerializeField] private float maxMoveBackSpeed = 0.5f;    // 移動速度
    [SerializeField] private float jumpForce = 500f; // ジャンプの強さ
    [SerializeField] private float maxAccelerateTime = 1.5f; //最大速度到達時間
    [SerializeField] private float maxSlidingTime = 3.0f; //スライディング時の最大時間


    private float nowAccelerateTime = 0.0f;
    private float slidingTime = 0.0f;

    Rigidbody rb;
    Chara ch;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ch = GetComponent<Chara>();
    }
    private void Update()
    {
        updateMove();
    }

    private void updateMove()
    {
        Transform trans = transform;
        transform.position = trans.position;

        //基本の動き
        {            
            Vector3 verticalVec = trans.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical");
            Vector3 horizontalVec = trans.TransformDirection(Vector3.right) * Input.GetAxis("Horizontal");
            float moveSpeed = 0.0f;
            float accelerate = 0.0f;
            if (Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.D))
            {
                nowAccelerateTime += Time.deltaTime;
                accelerate = nowAccelerateTime / maxAccelerateTime;
                accelerate = Math.Min(accelerate, 1);
                moveSpeed = Math.Abs(horizontalVec.x) > 0 ? maxMoveWalkSpeed * accelerate : maxMoveRunSpeed * accelerate;
                moveSpeed = verticalVec.z < 0 ? maxMoveBackSpeed * accelerate : moveSpeed;
            }
            trans.position += ((verticalVec + horizontalVec) * moveSpeed).normalized;

            Chara.BODY_STATUS bs = moveSpeed == maxMoveRunSpeed ? Chara.BODY_STATUS.RUN : Chara.BODY_STATUS.WALK;
            ch.setBodyStatus(bs);
        }

        //ジャンプ
        {
            if (Input.GetKeyDown(KeyCode.Space) && ch.isGround)
            {
                rb.AddForce(new Vector3(0, jumpForce, 0));
            }
        }
        //しゃがむ
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                ch.setBodyStatus(Chara.BODY_STATUS.SQUAT);
                if (ch.getBodyStatus() == Chara.BODY_STATUS.RUN)
                {
                    ch.setBodyStatus(Chara.BODY_STATUS.SLIDING);
                }
            }
            else
            {
                ch.setBodyStatus(Chara.BODY_STATUS.STAND);
            }
        }
        //スライディング
        {
            if (ch.getBodyStatus() == Chara.BODY_STATUS.SLIDING)
            {
                slidingTime += Time.deltaTime;
                if (slidingTime >= maxSlidingTime)
                {
                    ch.setBodyStatus(Chara.BODY_STATUS.STAND);
                    slidingTime = 0.0f;
                }
                else
                {

                }
                return;
            }

        }
    }
}
