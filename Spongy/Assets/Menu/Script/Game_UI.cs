// ステージでプレイしているときにメニュー画面を開いたり閉じたりする

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_UI : MonoBehaviour
{
    [SerializeField] Button Openbtn;
    [SerializeField] Button Closebtn;
    bool menuActive = false;            // メニューが表示されているとtrue


    void Update() {
        // Esc押した時かつタイトルでもクリア画面でもないとき
        if(Input.GetKeyDown(KeyCode.Escape) && GameRoot.State == 1) {
            if(menuActive) {                // メニューを閉じる
                Closebtn.onClick.Invoke();
                menuActive = false;
            } else {                        // メニューを開く
                Openbtn.onClick.Invoke();
                menuActive = true;
            }
        }
    }
}
