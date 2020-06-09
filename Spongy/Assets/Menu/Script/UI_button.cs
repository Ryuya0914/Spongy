// メニュー画面のそれぞれのボタンの処理

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Canvas_selectから呼び出す関数たち

public class UI_button : MonoBehaviour
{

    [SerializeField] string NextSceneName;  // 次のシーンの名前
    [SerializeField] GameObject MenuObj;    // 閉じたいメニューを格納
    [SerializeField] int nextState;         // ボタンを押した後のゲームの状態
    [SerializeField] TimeCounter _tCounter; // 時間を数えるかどうかを切り替えるスクリプト


    // シーン読み込み
    public void SceneChange() {
        Invoke("SceneLoad", 0.5f);  // 時間を遅延させてメソッド呼び出し
    }
    
    // 実際にシーンを切り替えるメソッド
    void SceneLoad() {
        GameRoot.State = nextState;
        SceneManager.LoadScene(NextSceneName);
    }


    // ポーズメニューを閉じる
    public void ExitMenu() {
        _tCounter.SetCountFlag = true;
        GameRoot.State = nextState;
        MenuObj.SetActive(false);
    }

    // メニューを開く
    public void OpenMenu() {
        _tCounter.SetCountFlag = false;
        MenuObj.SetActive(true);
    }


    // ゲーム終了 (titleから)
    public void EndGame() {

    }
    

}
