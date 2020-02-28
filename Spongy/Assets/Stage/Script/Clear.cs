using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    [SerializeField] GameObject ui;

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            ui.SetActive(true);
        }
    }

}
