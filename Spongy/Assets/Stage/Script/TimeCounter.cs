// スコアと時間を管理する
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    bool CountFlag = false;         // 時間を数えるフラグ(メニューとかで時間が止まるように)
    float timer = 0;                // 現在の経過時間
    [SerializeField] Text text_time;



    void Update() {
        if(CountFlag) {             // 時間カウントしていい？
            timer += Time.deltaTime;                    // 時間をタイマーに加算する
            text_time.text = timer.ToString("0000.0");   // 画面に時間を表示
        }
    }

    // 時間のゲッタ―
    public float GetTime {
        get { return timer; }
    }

    // カウントフラグのセッター
    public bool SetCountFlag {
        set { CountFlag = value; }
    }

}
