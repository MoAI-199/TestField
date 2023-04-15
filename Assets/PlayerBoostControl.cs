using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerBoostControl : MonoBehaviour
{
    [SerializeField] private float boostDelayFrames = 10; //Spaceキー入力猶予フレーム数
    [SerializeField] private float boostSpeed = 20; //ブースト速度

    private bool isBoosting = false; //ブースト中かどうかのフラグ
    private int boostDelayCount = 0; //Spaceキー入力猶予用のカウンタ
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

        Debug.Log("Boost disabled");
    }

    /// <summary>指定秒後に処理を呼ぶ</summary>
    /// <param name="seconds">遅延させるフレーム数</param>
    /// <param name="action">実行したい処理</param>
    private IEnumerator DelayCoroutine(float seconds, Action action)
    {
        for (int i = 0; i < seconds; i++)
        {
            yield return null;
        }
        action();
    }

    /// <summary>
    /// ブーストの実行処理
    /// </summary>
    private void doBoost()
    {
        //遅延フレーム数＊１フレームあたりの秒数
        StartCoroutine(DelayCoroutine(boostDelayCount, () =>
        {
            //ブースト有効化
            isBoosting = true;
            boostDelayCount = 0;
            //ブースト速度のベクトルを算出
            Vector3 boostVelocity = transform.forward * boostSpeed;
            boostVelocity.y = rb.velocity.y;
            //ブースト速度のベクトルを適用
            rb.velocity = boostVelocity;
            Debug.Log("Boost enabled");
        }));
    }

}

