// minimapを拡大縮小する

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    // 拡大縮小するオブジェクト(UI)
    [SerializeField] Image mapBack;
    [SerializeField] RawImage map;

    // コンポーネント
    RectTransform RTrans_back, RTrans_map;          // サイズや位置変えるやつ

    // 位置やサイズの情報
    Vector2 backpos_before,  backpos_after  = new Vector2(155, -155);        // 初期位置, 拡大後の位置
    Vector2 mappos_before,   mappos_after   = new Vector2(0, 0);        // 初期位置, 拡大後の位置
    Vector2 backsize_before, backsize_after = new Vector2(430,  430);        // 初期サイズ, 拡大後のサイズ
    Vector2 mapsize_before,  mapsize_after  = new Vector2(345, 345);        // 初期サイズ, 拡大後のサイズ


    void Start()
    {
        // コンポーネント取得
        RTrans_back = mapBack.GetComponent<RectTransform>();
        RTrans_map = map.GetComponent<RectTransform>();

        // 初期位置、初期サイズ取得
        backpos_before = RTrans_back.anchoredPosition;
        backsize_before = RTrans_back.sizeDelta;

        mappos_before = RTrans_map.anchoredPosition;
        mapsize_before = RTrans_map.sizeDelta;
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)) {       // Kを押したとき
            ChangeMapSize(RTrans_back, backpos_after, backsize_after);
            ChangeMapSize(RTrans_map, mappos_after, mapsize_after);
        } else if(Input.GetKeyUp(KeyCode.K)) {  // Kを離したとき
            ChangeMapSize(RTrans_back, backpos_before, backsize_before);
            ChangeMapSize(RTrans_map, mappos_before, mapsize_before);
        }
    }
    
    void ChangeMapSize(RectTransform trans, Vector3 pos, Vector2 size) {
        trans.anchoredPosition = pos;
        trans.sizeDelta = size;
    }

}
