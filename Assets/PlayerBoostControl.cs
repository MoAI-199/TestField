using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerBoostControl : MonoBehaviour
{
    [SerializeField] private float boostDelayFrames = 10f; //Spaceキー入力猶予フレーム数
    [SerializeField] private float boostLength = 20f; //移動距離
    [SerializeField] private float boostTime = 2.0f; //ブースト速度

    private bool isBoosting = false; //ブースト中かどうかのフラグ
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
            //1秒間重力の影響を無視する
            rb.useGravity = false;
            Invoke("DisableBoost", 1f);
        }
    }

    private void DisableBoost()
    {
        //ブースト無効化
        isBoosting = false;
        rb.useGravity = true;
    }

    /// <summary>指定秒後に処理を呼ぶ</summary>
    /// <param name="seconds">遅延させるフレーム数</param>
    /// <param name="action">実行したい処理</param>
    private IEnumerator DelayCoroutine(float waitSeconds)
    {
        //待機フレーム
        for (int i = 0; i < waitSeconds; i++)
        {
            yield return null;
        }
        //ブースト有効化
        isBoosting = true;
        //ブースト後の移動位置の算出
        Vector3 nextPos = this.transform.position + this.transform.forward * boostLength;
        
        while (true)
        {
            Vector3 distance = nextPos - this.transform.position;
            //移動量の算出
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
    /// ブーストの実行処理
    /// </summary>
    private void doBoost()
    {
        StartCoroutine(DelayCoroutine(boostDelayFrames));
    }

}

