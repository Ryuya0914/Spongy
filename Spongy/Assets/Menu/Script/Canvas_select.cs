// メニュー画面でどのボタンを押すか切り替える

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_select : MonoBehaviour
{
    [SerializeField] Button[] btns;         // アタッチしてあるメニュー画面内の全部のボタン
    [SerializeField] Animator[] animator;   // ボタンのアニメーター
    int count, NowNum = 0;                  // ボタンの個数, 現在選択しているボタン
    void Start()
    {
        count = btns.Length - 1;            // ボタンの個数を保存
    }

    // メニューがアクティブになった時に呼ばれるメソッド
    void OnEnable() {
        NowNum = 0;                                 // 現在選択しているボタンをリセット
        animator[0].CrossFade("popup", 0.4f);       // アニメーションスタート
    }

    // メニューが非アクティブになった時に呼ばれるメソッド
    void OnDisable() {
        animator[NowNum].CrossFade("nomal", 0.1f);  //  アニメーションを止める
    }

    void Update()
    {
        // 選択しているボタンを切り替える(アニメーションも)
        if(Input.GetKeyDown(KeyCode.W)) {               // 一つ上へ
            animator[NowNum].CrossFade("nomal", 0.1f);
            NowNum--;
            NowNum = (NowNum < 0) ? count : NowNum;
            animator[NowNum].CrossFade("popup", 0.4f);

        } else if(Input.GetKeyDown(KeyCode.S)) {        // 一つ下へ
            animator[NowNum].CrossFade("nomal", 0.1f);
            NowNum++;
            NowNum = (NowNum > count) ? 0 : NowNum;
            animator[NowNum].CrossFade("popup", 0.4f);

        } else if(Input.GetKeyDown(KeyCode.Return)) {    // spaceで決定
            btns[NowNum].onClick.Invoke();  // unityのinspectorで紐付けしたメソッドを呼び出す
        }
    }
}
