using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movesample2 : MonoBehaviour {
    [SerializeField, Header("楽するため")] Rigidbody2D rb;
    [SerializeField, Header("移動量(乗算)")] float Speed;
    [SerializeField, Header("ジャンプ高さ調整(減算)")] float Jump_Height;
    [SerializeField, Header("水流時移動抵抗量")] float Current, UaD_Current;
    [SerializeField, Header("現在の向き保存")] int Quantity;
    [SerializeField, Header("浮力にしたかった")] float buoyancy;
    [SerializeField, Header("含水量")] int Hydrated;
    [SerializeField, Header("向き管理")] bool Left, Right, Under;
    [SerializeField, Header("ジャンプ管理")] bool Jump;
    [SerializeField, Header("吸水管理")] bool Soak_On, Soak_Cancel, Water;
    [SerializeField, Header("UI")] Image Meter;
    [HideInInspector] float xSpeed;

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


        rb.velocity = new Vector2(xSpeed - Current, rb.velocity.y - UaD_Current);
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
        if(Input.GetKey(KeyCode.O) && 5 <= Hydrated) {
            Hydrated -= 5;//吸った水を吐き出す
            Boost();//ブースト実行
        }
        //---------------------------------------//



        //-------ジャンプ--------//
        if(Input.GetKeyDown(KeyCode.Space) && Jump) {
            Jump = false;
            rb.AddForce(Vector3.up * (Speed * 3 - Jump_Height), ForceMode2D.Impulse);
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
    void Boost() {



        Speed += 2.5f / 20f;//だんだん加速


        if(Right && !Under)//右
            rb.AddForce(Vector3.right * 20, ForceMode2D.Impulse);
        if(Left && !Under)//左
            rb.AddForce(-Vector3.right * 20, ForceMode2D.Impulse);
        if(Under)//下
        {
            Under = false;
            rb.AddForce(Vector3.up * 15, ForceMode2D.Impulse);
        }
    }
    //-------------------------------------------------------------//



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
        if(col.gameObject.CompareTag("TopGround"))
            Jump = true;
        //-----------------------------------------//



    }








    //--------水に入っている間水を吸う処理----------//
    IEnumerator Soak() {
        while(Hydrated < 100) {
            Hydrated += 5;//だんだん吸水

            Speed -= 2.5f / 20f;//だんだん減速
            yield return new WaitForSeconds(0.05f);
            if(Soak_On || Soak_Cancel) {
                Soak_Cancel = false;
                break;
            }
        }

        yield break;
    }
    //----------------------------------------------//
}
