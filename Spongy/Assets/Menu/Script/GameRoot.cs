// ゲームの状態を数字で保持しておく

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRoot : MonoBehaviour {
    // 0 = title , 1 = game, 2 = clear
    static public int State = 0;
    [SerializeField] Button btn;
    
    void Start() {
        if (State != 0)
        btn.onClick.Invoke();
    }
    

}
