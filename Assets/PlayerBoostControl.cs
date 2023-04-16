using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerBoostControl : MonoBehaviour
{
    [SerializeField] private float boostDelayFrames = 10f; //Space�L�[���͗P�\�t���[����
    [SerializeField] private float boostLength = 20f; //�ړ�����
    [SerializeField] private float boostTime = 2.0f; //�u�[�X�g���x

    private bool isBoosting = false; //�u�[�X�g�����ǂ����̃t���O
    private Rigidbody rb;
    private float nowTime = 0f;

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
    }

    /// <summary>�w��b��ɏ������Ă�</summary>
    /// <param name="seconds">�x��������t���[����</param>
    /// <param name="action">���s����������</param>
    private IEnumerator DelayCoroutine(float waitSeconds)
    {
        //�ҋ@�t���[��
        for (int i = 0; i < waitSeconds; i++)
        {
            yield return null;
        }
        //�u�[�X�g�L����
        isBoosting = true;
        //�u�[�X�g��̈ړ��ʒu�̎Z�o
        Vector3 nextPos = this.transform.position + this.transform.forward * boostLength;
        
        while (true)
        {
            Vector3 distance = nextPos - this.transform.position;
            //�ړ��ʂ̎Z�o
            Vector3 vec = distance / ( boostTime * (1f / Time.deltaTime) );
            this.transform.position += vec;
            if(Vector3.Distance(nextPos, this.transform.position) < 5f)
            {
                break;
            }
            yield return this.transform.position += vec;
        }
        Debug.Log("boostEnd");
    }

    /// <summary>
    /// �u�[�X�g�̎��s����
    /// </summary>
    private void doBoost()
    {
        StartCoroutine(DelayCoroutine(boostDelayFrames));
    }

}

