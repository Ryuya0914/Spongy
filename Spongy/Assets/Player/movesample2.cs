using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movesample2 : MonoBehaviour
{
    [SerializeField, Header("楽するため")] Rigidbody2D rb;
    [SerializeField, Header("移動量(乗算)")] float Speed;
    [SerializeField, Header("ジャンプ高さ調整(加算)")] float Jump_Height;
    [SerializeField, Header("ブースト加速量(左右)")] float Boost_Influence;
    [SerializeField, Header("ブースト加速量(上)")] float Boost_Rise;
    [SerializeField, Header("ブースト速度制限")] float Boost_Limit;
    [SerializeField, Header("吸水の増減量")] float Water_Fluctuation;
    [SerializeField, Header("含水量")] float Hydrated;
    [SerializeField, Header("ブーストの実行間隔")] float Boost_Wait;
    [SerializeField, Header("向き管理")] bool Left, Right, Under;
    [SerializeField, Header("ジャンプ管理")] bool Jump;
    [SerializeField, Header("ジャンプ力")] float JumpForce;
    [SerializeField, Header("ジャンプ力差分(乗算)")] float JumpDifference;
    [SerializeField, Header("ジャンプ時滞空時間？落下開始までの時間？")] float JumpFlight;
    [SerializeField, Header("吸水管理")] bool Soak_On, Soak_Cancel, Water;
    [SerializeField, Header("UI")] Image Meter;
    [HideInInspector] float xSpeed, Speed_Influence, Speed_Rise, BR, BI, JumpForce_Execution;
    [SerializeField, Header("吸水時移動速度増減量(除算)")] float[] Increase;

    [Header("水流時移動抵抗量")] float Current, UaD_Current;
    [Header("現在の向き保存")] int Quantity;

    void Update()
    {

        //------------移動処理----------//
        if (Input.GetKey(KeyCode.A) && !Right)
        {
            if (!Under) Rotate(180);
            Move(-1);
            Left = true;
            Right = false;
        }
        else if (Input.GetKey(KeyCode.D) && !Left)
        {
            if (!Under) Rotate(0);
            Move(1);
            Left = false;
            Right = true;
        }
        else
        {
            Left = false;
            Right = false;
            Move(0);

        }
        //------------------------------//



        //---------------向き切り替え-------------------//
        if (Input.GetKeyDown(KeyCode.W))//上向く
        {
            Under = true;
            transform.rotation = Quaternion.AngleAxis(90, new Vector3(0, 0, 1));
        }
        if (Input.GetKeyUp(KeyCode.W))//元に戻る
        {
            Under = false;
            transform.rotation = Quaternion.AngleAxis(Quantity, new Vector3(0, 0, 1));
        }
        //-----------------------------------------------//


        //-----------------吸水＆放水--------------//
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Water && Soak_On)
            {
                StartCoroutine(Soak());
                Soak_On = false;
                if (Under || Right || Left)
                {
                    StopAllCoroutines();
                    StartCoroutine(Boost());
                }
            }
            else if (0 <= Hydrated && Jump)
            {
                if (Under || Right || Left)
                {
                    StopAllCoroutines();
                    StartCoroutine(Boost());
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            StopAllCoroutines();
            Boost_Deseletion();
            Soak_On = true;
        }
        //-----------------------------------------//



        //-------ジャンプ--------//
        if (Input.GetKeyDown(KeyCode.Space) && Jump)
        {
            Jump = false;
            StartCoroutine(Jump2());
        }
        //--------------------//


        ////---------------ブースター--------------//
        //    if (Input.GetKeyDown(KeyCode.O) && 0 <= Hydrated && Jump)
        //{
        //    StopAllCoroutines();
        //    StartCoroutine(Boost());
        //}
        //if (Input.GetKeyUp(KeyCode.O))
        //{
        //    Boost_Deseletion();
        //}
        //---------------------------------------//

        //-----------水中にいるとき段々吸水---------------//
        //if (Input.GetKeyDown(KeyCode.P) && Water && Soak_On)
        //{
        //    StartCoroutine(Soak());
        //    Soak_On = false;
        //}
        //if (Input.GetKeyUp(KeyCode.P))
        //    Soak_On = true;
        //------------------------------------------------//

        //-----------含水量のUIを更新---------------------//
        Meter.fillAmount = Hydrated / 100f;
        //------------------------------------------------//

    }

    //-----------左右反転-------------//
    void Rotate(int Quantit)
    {
        Quantity = Quantit;
        transform.rotation = Quaternion.AngleAxis(Quantity, new Vector3(0, 1, 0));
    }
    //-------------------------------//

    //--------------------移動処理---------------------//
    void Move(int move_speed)
    {
        xSpeed = Speed * move_speed;
        Speed_Influence = xSpeed + BI - Current;
        Speed_Rise = rb.velocity.y + BR - UaD_Current + JumpForce_Execution;
        if (rb.velocity.y < 10)
            rb.velocity = new Vector2(Speed_Influence, Speed_Rise);
    }
    //--------------------------------------------------------//

    //---------------ジャンプ処理部---------------//
    IEnumerator Jump2()
    {
        rb.AddForce(Vector3.up * (Jump_Height - (4 * Hydrated / 100)), ForceMode2D.Impulse);
        yield return new WaitForSeconds(JumpFlight);
        JumpForce_Execution = JumpForce * (1 + Hydrated / 100);
        yield return new WaitForSeconds(JumpFlight / (1 + Hydrated / 100));
        JumpForce_Execution = 0;
        yield break;
    }
    //-------------------------------------------//


    //------------加速システム処理部----------------//
    IEnumerator Boost()
    {
        while (Hydrated > 0)
        {
            Speed += Increase[0] / Increase[1];//だんだん加速

            Hydrated -= Water_Fluctuation;//吸った水を吐き出す

            if (Right && !Under)//右
                BI += Boost_Influence;
            if (Left && !Under)//左
                BI -= Boost_Influence;
            if (Under && BR < Boost_Limit)//下
                BR = Boost_Rise + 0.2f;
            yield return new WaitForSeconds(Boost_Wait);
        }
        Boost_Deseletion();
        yield break;
    }
    //-------------------------------------------------------------//

    //----------ブーストキャンセル処理?----------//
    void Boost_Deseletion()
    {
        BR = 0;
        BI = 0;
    }
    //------------------------------------------------//

    //--------水に入っている間水を吸う処理----------//
    IEnumerator Soak()
    {
        while (Hydrated < 100)
        {
            Hydrated += Water_Fluctuation;//だんだん吸水

            Speed -= Increase[0] / Increase[1];//だんだん減速
            yield return new WaitForSeconds(0.1f);
            if (Soak_On || Soak_Cancel)
            {
                Soak_Cancel = false;
                break;
            }
        }

        yield break;
    }
    //----------------------------------------------//


   public int Hydrated_check()
    {
        return (int)Hydrated;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //----------------水中かどうか--------------//
        if (col.gameObject.CompareTag("Water") || col.gameObject.CompareTag("Left_Current") || col.gameObject.CompareTag("Right_Current") || col.gameObject.CompareTag("Fast_Current") || col.gameObject.CompareTag("Low_Current"))
            Soak_Cancel = false;
        //-----------------------------------------//
    }



    void OnTriggerExit2D(Collider2D col)
    {
        //--------水流から離れたら移動速度が戻る------//
        if (col.gameObject.CompareTag("Left_Current") || col.gameObject.CompareTag("Right_Current") || col.gameObject.CompareTag("Fast_Current") || col.gameObject.CompareTag("Low_Current"))
        {
            Current = 0;
            UaD_Current = 0;
        }
        //--------------------------------------------//

        //-----------水中かどうか-------------//
        if (col.gameObject.CompareTag("Water") || col.gameObject.CompareTag("Left_Current") || col.gameObject.CompareTag("Right_Current") || col.gameObject.CompareTag("Fast_Current") || col.gameObject.CompareTag("Low_Current"))
        {
            Water = false;
            Soak_Cancel = true;
        }
        //------------------------------------//

        //--------------接地しているか-------------//
        if(col.gameObject.CompareTag("TopGround")) {
            Jump = false;
        }
        //-----------------------------------------//
    }


    void OnTriggerStay2D(Collider2D col)
    {
        //----------------水中かどうか--------------//
        if (col.gameObject.CompareTag("Water") || col.gameObject.CompareTag("Left_Current") || col.gameObject.CompareTag("Right_Current") || col.gameObject.CompareTag("Fast_Current") || col.gameObject.CompareTag("Low_Current"))
            Water = true;
        //-----------------------------------------//

        //--------------------水流に流されるやつ-------------------//
        if (col.gameObject.CompareTag("Left_Current"))//左
            Current = 2;
        if (col.gameObject.CompareTag("Right_Current"))//右
            Current = -2;
        if (col.gameObject.CompareTag("Fast_Current"))
            UaD_Current = 5;
        if (col.gameObject.CompareTag("Low_Current"))
            UaD_Current = 2;
        //----------------------------------------------------------//

        //--------------接地しているか-------------//
        if (col.gameObject.CompareTag("TopGround"))
        {
            Jump = true;
        }
        //-----------------------------------------//
    }
}
