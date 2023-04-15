using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerBoostControl : MonoBehaviour
{
    [SerializeField] private float boostDelayFrames = 10; //Space�L�[���͗P�\�t���[����
    [SerializeField] private float boostSpeed = 20; //�u�[�X�g���x

    private bool isBoosting = false; //�u�[�X�g�����ǂ����̃t���O
    private int boostDelayCount = 0; //Space�L�[���͗P�\�p�̃J�E���^
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            doBoost();
        }

        if (isBoosting)
        {
            //1�b�ԏd�͂̉e���𖳎�����
            rb.useGravity = false;
            Invoke("DisableBoost", 1f);
        }
    }

    private void DisableBoost()
    {
        //�u�[�X�g������
        isBoosting = false;
        rb.useGravity = true;

        Debug.Log("Boost disabled");
    }

    /// <summary>�w��b��ɏ������Ă�</summary>
    /// <param name="seconds">�x��������t���[����</param>
    /// <param name="action">���s����������</param>
    private IEnumerator DelayCoroutine(float seconds, Action action)
    {
        for (int i = 0; i < seconds; i++)
        {
            yield return null;
        }
        action();
    }

    /// <summary>
    /// �u�[�X�g�̎��s����
    /// </summary>
    private void doBoost()
    {
        //�x���t���[�������P�t���[��������̕b��
        StartCoroutine(DelayCoroutine(boostDelayCount, () =>
        {
            //�u�[�X�g�L����
            isBoosting = true;
            boostDelayCount = 0;
            //�u�[�X�g���x�̃x�N�g�����Z�o
            Vector3 boostVelocity = transform.forward * boostSpeed;
            boostVelocity.y = rb.velocity.y;
            //�u�[�X�g���x�̃x�N�g����K�p
            rb.velocity = boostVelocity;
            Debug.Log("Boost enabled");
        }));
    }

}

