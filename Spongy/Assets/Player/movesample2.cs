using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movesample2 : MonoBehaviour {
    [SerializeField, Header("楽するため")] Rigidbody2D rb;
    [SerializeField, Header("移動量(乗算)")] float Speed;
    [SerializeField, Header("ジャンプ高さ調整(加算)")] float Jump_Height;
    [SerializeField,Header("ブースト加速量")] float Boost_Rise, Boost_Influence, Boost_Limit;
    [SerializeField, Header("水流時移動抵抗量")] float Current, UaD_Current;
    [SerializeField, Header("吸水の増減量")] float Water_Fluctuation;
    [SerializeField, Header("現在の向き保存")] int Quantity;
    [SerializeField, Header("浮力にしたかった")] float buoyancy;
    [SerializeField, Header("含水量")] float Hydrated;
    [SerializeField, Header("ブーストの実行間隔")] float Boost_Wait;
    [SerializeField, Header("向き管理")] bool Left, Right, Under;
    [SerializeField, Header("ジャンプ管理")] bool Jump;
    [SerializeField, Header("吸水管理")] bool Soak_On, Soak_Cancel, Water;
    [SerializeField, Header("UI")] Image Meter;
    [HideInInspector] float xSpeed, Speed_Influence, Speed_Rise, BR, BI;
    [SerializeField,Header("速度増減量(除算)")] float[] z;

    void Update() {

        //------------移動処理----------//
        if(Input.GetKey(KeyCode.A) && !Right) {
            if(!Under) {
                Quantity = 180;
                transform.rotation = Quaternion.AngleAxis(Quantity, new Vector3(0, 1, 0));
            }
            xSpeed = -Speed;
            Right = false;
            Left = true;
        } else if(Input.GetKey(KeyCode.D) && !Left) {
            if(!Under) {
                Quantity = 0;
                transform.rotation = Quaternion.AngleAxis(Quantity, new Vector3(0, 1, 0));
            }
            xSpeed = Speed;
            Left = false;
            Right = true;
        } else {
            Left = false;
            Right = false;
            xSpeed = 0;
        }


        Speed_Influence = xSpeed + BI - Current;
        Speed_Rise = rb.velocity.y + BR - UaD_Current;
        rb.velocity = new Vector2(Speed_Influence, Speed_Rise);
        //------------------------------//







        //---------------向き切り替え-------------------//
        if(Input.GetKeyDown(KeyCode.W))//上向く
        {
            Under = true;
            transform.rotation = Quaternion.AngleAxis(90, new Vector3(0, 0, 1));
        }
        if(Input.GetKeyUp(KeyCode.W))//元に戻る
        {
            Under = false;
            transform.rotation = Quaternion.AngleAxis(Quantity, new Vector3(0, 0, 1));
        }
        //-----------------------------------------------//




        //---------------ブースター--------------//
        if(Input.GetKeyDown(KeyCode.O) && 0 <= Hydrated) {
            //Boost();//ブースト実行
            StopAllCoroutines();
            StartCoroutine(Boost());
        }

        if(Input.GetKeyUp(KeyCode.O)) {
            StopAllCoroutines();
            StartCoroutine(Boost_Deseletion());
            StartCoroutine(Boost_Deseletion2());
            StartCoroutine(Boost_Deseletion3());
        }
        //---------------------------------------//

        //-------ジャンプ--------//
        if(Input.GetKeyDown(KeyCode.Space) && Jump) {
            Jump = false;
            rb.AddForce(Vector3.up * (Speed *2 + Jump_Height), ForceMode2D.Impulse);
        }
        //--------------------//

        //-----------水中にいるとき段々吸水---------------//
        if(Input.GetKeyDown(KeyCode.P) && Water && Soak_On) {
            StartCoroutine(Soak());
            Soak_On = false;
        }
        if(Input.GetKeyUp(KeyCode.P))
            Soak_On = true;
        //------------------------------------------------//

        //-----------含水量のUIを更新---------------------//
        Meter.fillAmount = Hydrated / 100f;
        //------------------------------------------------//

    }

    //-------------------加速システム処理部-----------------------//
    IEnumerator Boost() {
        //while(Speed < 5) {
        while(Hydrated > 0) {
            Speed += z[0] / z[1];//だんだん加速

            Hydrated -= Water_Fluctuation;//吸った水を吐き出す

            if(Right && !Under)//右
                BI += Boost_Influence;
            ////rb.AddForce(Vector3.right * 20, ForceMode2D.Impulse);
            if(Left && !Under)//左
                BI -= Boost_Influence;
            //rb.AddForce(-Vector3.right * 20, ForceMode2D.Impulse);
            if(Under&&BR<Boost_Limit)//下
            {
                //Under = false;
                BR += Boost_Rise;
            }
            yield return new WaitForSeconds(Boost_Wait);
        }
        StartCoroutine(Boost_Deseletion());
        StartCoroutine(Boost_Deseletion2());
        StartCoroutine(Boost_Deseletion3());
        StopCoroutine(Boost());

    }
    //-------------------------------------------------------------//


    IEnumerator Boost_Deseletion() {
        while(0 < BR) {
            BR -= Boost_Rise * 2;
            if(BR < 0)
                yield break;
            yield return null;
        }
    }
    IEnumerator Boost_Deseletion2() {
        while(0 < BI) {
            BI -= Boost_Influence;
            if(BI < 0)
                yield break;
            yield return null;
        }
    }
    IEnumerator Boost_Deseletion3() {
        while(0 > BI) {
            BI += Boost_Influence;
            if(BI > 0)
                yield break;
            yield return null;
        }
    }





    //--------水に入っている間水を吸う処理----------//
    IEnumerator Soak() {
        while(Hydrated < 100) {
            Hydrated += Water_Fluctuation;//だんだん吸水

            Speed -= z[0] / z[1];//だんだん減速
            yield return new WaitForSeconds(0.1f);
            if(Soak_On || Soak_Cancel) {
                Soak_Cancel = false;
                break;
            }
        }

        yield break;
    }
    //----------------------------------------------//





    void OnTriggerEnter2D(Collider2D col) {
        //----------------水中かどうか--------------//
        if(col.gameObject.CompareTag("Water") || col.gameObject.CompareTag("Left_Current") || col.gameObject.CompareTag("Right_Current") || col.gameObject.CompareTag("Fast_Current") || col.gameObject.CompareTag("Low_Current"))
            Soak_Cancel = false;
        //-----------------------------------------//
    }



    void OnTriggerExit2D(Collider2D col) {
        //--------水流から離れたら移動速度が戻る------//
        if(col.gameObject.CompareTag("Left_Current") || col.gameObject.CompareTag("Right_Current") || col.gameObject.CompareTag("Fast_Current") || col.gameObject.CompareTag("Low_Current")) {
            Current = 0;
            UaD_Current = 0;
        }
        //--------------------------------------------//

        //-----------水中かどうか-------------//
        if(col.gameObject.CompareTag("Water") || col.gameObject.CompareTag("Left_Current") || col.gameObject.CompareTag("Right_Current") || col.gameObject.CompareTag("Fast_Current") || col.gameObject.CompareTag("Low_Current")) {
            Water = false;
            Soak_Cancel = true;
        }
        //------------------------------------//
    }


    void OnTriggerStay2D(Collider2D col) {
        //----------------水中かどうか--------------//
        if(col.gameObject.CompareTag("Water") || col.gameObject.CompareTag("Left_Current") || col.gameObject.CompareTag("Right_Current") || col.gameObject.CompareTag("Fast_Current") || col.gameObject.CompareTag("Low_Current"))
            Water = true;
        //-----------------------------------------//

        //--------------------水流に流されるやつ-------------------//
        if(col.gameObject.CompareTag("Left_Current"))//左
            Current = 2;
        if(col.gameObject.CompareTag("Right_Current"))//右
            Current = -2;
        if(col.gameObject.CompareTag("Fast_Current"))
            UaD_Current = 5;
        if(col.gameObject.CompareTag("Low_Current"))
            UaD_Current = 2;
        //----------------------------------------------------------//

        //--------------接地しているか-------------//
        if(col.gameObject.CompareTag("TopGround")) {
            Jump = true;
        }
        //-----------------------------------------//
    }
}
