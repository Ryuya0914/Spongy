using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movesample2 : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField,Header("移動量(乗算)")] float Speed;
    float Current;
    [SerializeField, Header("含水量")] int Hydrated;



    [SerializeField,Header("管理フラグ")] bool Left, Right, Under,Jump;


    float xSpeed;

    void Update()
    {

        //------------移動処理----------//
        if (Input.GetKey(KeyCode.A) && !Right)
        {
            transform.rotation = Quaternion.AngleAxis(180, new Vector3(0, 1, 0));
            xSpeed = -Speed;
            Right = false;
            Left = true;
        }
        else if (Input.GetKey(KeyCode.D) && !Left)
        {
            transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 1, 0));
            xSpeed = Speed;
            Left = false;
            Right = true;
        }
        else
        {
            Left = false;
            Right = false;
            xSpeed = 0;
        }


        rb.velocity = new Vector2(xSpeed - Current, rb.velocity.y);
        //------------------------------//


        //-----------左右反転----------//
        //if (Input.GetKeyDown(KeyCode.A) && !Right)//左
        //    transform.rotation = Quaternion.AngleAxis(180, new Vector3(0, 1, 0));
        //if (Input.GetKeyDown(KeyCode.D) && !Left)//右
        //    transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 1, 0));
        //-----------------------------//





        //---------------向き切り替え-------------------//
        if (Input.GetKeyDown(KeyCode.K))//上向く
        {
            Under = true;
            transform.rotation = Quaternion.AngleAxis(90, new Vector3(0, 0, 1));
        }
        if (Input.GetKeyUp(KeyCode.K))//元に戻る
        {
            Under = false;
            transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
        }
        //-----------------------------------------------//




        //---------------ブースター--------------//
        if (Input.GetKey(KeyCode.B) && 5 <= Hydrated)
        {
            Hydrated -= 5;//吸った水を吐き出す
            Boost();//ブースト実行
        }
        //---------------------------------------//



        //-------ジャンプ--------//
        if (Input.GetKeyDown(KeyCode.Space) && Jump)
        {
            Jump = false;
            rb.AddForce(Vector3.up * Speed * 3 / 2, ForceMode2D.Impulse);
        }
        //--------------------//






    }

    //-------------------加速システム処理部-----------------------//
    void Boost()
    {


        if (Hydrated % 4 == 0 && Speed < 5) Speed += 0.5f;//移動量を戻す



        if (Right)//右
            rb.AddForce(Vector3.right * 20, ForceMode2D.Impulse);
        if (Left)//左
            rb.AddForce(-Vector3.right * 20, ForceMode2D.Impulse);
        if (Under)//下
        {
            Under = false;
            rb.AddForce(Vector3.up * 10, ForceMode2D.Impulse);
        }
    }
    //-------------------------------------------------------------//



    void OnTriggerEnter2D(Collider2D col)
    {
        //-----------水を含む処理の開始地点--------------//
        if (col.gameObject.CompareTag("Water"))
            StartCoroutine(Soak());
        //-----------------------------------------------//


    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            Jump = true;
    }




    void OnTriggerExit2D(Collider2D col)
    {
        //-----------水を含む処理の終了地点--------------//
        if (col.gameObject.CompareTag("Water"))
            StopCoroutine(Soak());
        //-----------------------------------------------//
        if (col.gameObject.CompareTag("Left_Current")|| col.gameObject.CompareTag("Right_Current"))
        {
            Current = 0;
        }

    }


    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Left_Current"))
        {
            Current = 2;
        }
        if (col.gameObject.CompareTag("Right_Current"))
        {
            Current = -2;
        }

    }








    //--------水に入っている間水を吸う処理----------//
    IEnumerator Soak()
    {
        while (Hydrated < 100)
        {
            Hydrated += 5;//だんだん吸水
            if (Hydrated % 4 == 0) Speed -= 0.5f;//だんだん減速
            yield return new WaitForSeconds(0.1f);
        }
        StopCoroutine(Soak());
    }
    //----------------------------------------------//
}
