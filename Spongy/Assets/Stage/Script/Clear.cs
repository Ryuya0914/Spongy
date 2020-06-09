using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clear : MonoBehaviour
{
    [SerializeField] GameObject ui;
    [SerializeField] Text _text;
    [SerializeField] TimeCounter timecounter;

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            ui.SetActive(true);
            _text.text = timecounter.GetTime.ToString("000.00");   // 画面に時間を表示
        }
    }

}
