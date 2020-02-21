using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class movesample : MonoBehaviour
{
    [SerializeField, Header("楽するため")] Rigidbody2D rb;
    [SerializeField, Header("移動速度上昇（乗算）")] float Move_Rate;
    [Header("移動管理フラグ")]
    [SerializeField,] bool Left, Right, Jump, Under;
    void Update()
    {
        //----------移動フラグ---------//
        if (Input.GetKey(KeyCode.A))//左
            Left = true;
        else Left = false;
        if (Input.GetKey(KeyCode.D))//右
            Right = true;
        else Right = false;
        //-----------------------------//



        //-----------左右反転----------//
        if (Input.GetKeyDown(KeyCode.A))//左
            transform.rotation = Quaternion.AngleAxis(180, new Vector3(0, 1, 0));
        if (Input.GetKeyDown(KeyCode.D))//右
            transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 1, 0));
        //-----------------------------//

        //-----------角度切り替え（上向く）---------//
        if (Input.GetKey(KeyCode.K))//上向く
        {
            transform.rotation = Quaternion.AngleAxis(90, new Vector3(0, 0, 1));
            Under = true;
        }
        if (Input.GetKeyUp(KeyCode.K))//元に戻る
        {
            transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
            Under = false;
        } 
        //------------------------------------------//




        //------------加速システム---------//
        if (Input.GetKeyDown(KeyCode.B))
            Boost();//実行
        //--------------------------------//




        //--------ジャンプフラグ-------//
        if (Input.GetKeyDown(KeyCode.Space)) Jump = true;
        else Jump = false;
        //-----------------------------//
    }





    //-------------------移動処理部分--------------------//
    void FixedUpdate()
    {
        //右に動くよ
        if (Right)
            rb.MovePosition(transform.position + Move_Rate * transform.right * Time.deltaTime);
        //左に動くよ
        if (Left)
            rb.MovePosition(transform.position + Move_Rate * transform.right * Time.deltaTime);
        //ジャンプするよ
        if (Jump)
            rb.AddForce(Vector3.up * Move_Rate * 3,ForceMode2D.Impulse);
    }
    //----------------------------------------------------//






    //-------------------加速システム処理部-----------------------//
    void Boost()
    {
        if (Right)//右
            rb.AddForce(Vector3.right * Move_Rate * 3,ForceMode2D.Impulse);
        if (Left)//左
            rb.AddForce(-Vector3.right * Move_Rate * 3, ForceMode2D.Impulse);
        if (Under)//下
            rb.AddForce(Vector3.up * Move_Rate * 3, ForceMode2D.Impulse);
    }
    //-------------------------------------------------------------//

}
