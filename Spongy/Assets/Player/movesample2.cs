using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movesample2 : MonoBehaviour
{
    [SerializeField,Header("楽するため")] Rigidbody2D rb;
    [SerializeField, Header("移動量(乗算)")] float Speed;
    [SerializeField,Header("水流時移動抵抗量")]float Current;
    [SerializeField, Header("含水量")] int Hydrated;
    [SerializeField, Header("向き管理")] bool Left, Right, Under;
    [SerializeField, Header("ジャンプ管理")] bool Jump;
    [SerializeField, Header("ジャンプ高さ調整(減算)")] float Jump_Height;
    [SerializeField, Header("吸水管理")] bool Soak_On, Water;
    [HideInInspector] float xSpeed;

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
            rb.AddForce(Vector3.up * (Speed * 3-Jump_Height), ForceMode2D.Impulse);
        }
        //--------------------//

        


        //-----------水中にいるとき段々吸水---------------//
        if (Input.GetKeyDown(KeyCode.P) && Water&&Soak_On)
        {
            StartCoroutine(Soak());
            Soak_On = false;
        }
        if (Input.GetKeyUp(KeyCode.P)) Soak_On = true;
        //------------------------------------------------//


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

        //----------------水中かどうか--------------//
        if (col.gameObject.CompareTag("Water"))
            Water = true;
        //-----------------------------------------//
    }
    void OnCollisionEnter2D(Collision2D col)
    {



        //--------------接地しているか-------------//
        if (col.gameObject.CompareTag("Ground"))
            Jump = true;
        //-----------------------------------------//




    }




    void OnTriggerExit2D(Collider2D col)
    {




        //--------水流から離れたら移動速度が戻る------//
        if (col.gameObject.CompareTag("Left_Current") || col.gameObject.CompareTag("Right_Current"))
            Current = 0;
        //--------------------------------------------//






        //-----------水中かどうか-------------//
        if (col.gameObject.CompareTag("Water"))
            Water = false;
        //------------------------------------//
    }


    void OnTriggerStay2D(Collider2D col)
    {

        //--------------------水流に流されるやつ-------------------//
        if (col.gameObject.CompareTag("Left_Current"))//左
            Current = 2;
        if (col.gameObject.CompareTag("Right_Current"))//右
            Current = -2;
        //----------------------------------------------------------//
    }








    //--------水に入っている間水を吸う処理----------//
    IEnumerator Soak()
    {
        while (Hydrated < 100)
        {
            Hydrated += 5;//だんだん吸水
            if (Hydrated % 4 == 0) Speed -= 0.5f;//だんだん減速
            yield return new WaitForSeconds(0.5f);
            if (Soak_On) break;
        }

        yield break;
    }
    //----------------------------------------------//
}
