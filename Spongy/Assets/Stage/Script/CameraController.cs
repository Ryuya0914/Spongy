using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    float speed = 0.8f;                     // カメラが動く最大速度
    float H = 6.0f;                         // カメラの縦の長さ
    float W = 10.0f;                         // カメラの横の長さ
    Vector3 vec = new Vector3(0, 0, 0);

    void Update()
    {
        // カメラの中心にプレイヤーが来るように移動する
        vec = player.transform.position - transform.position;                   // 位置の差分
        vec.x = (vec.x > W) ? vec.x - W : ((vec.x < -W) ? vec.x + W : ((vec.x > speed) ? speed : ((vec.x < -speed) ? -speed : vec.x)));
        vec.y = (vec.y > H) ? vec.y - H : ((vec.y < -H) ? vec.y + H : ((vec.y > speed) ? speed : ((vec.y < -speed) ? -speed : vec.y)));
        //vec.x = ((vec.x > speed) ? speed : ((vec.x < -speed) ? -speed : vec.x));  // xの値がspeedを超えないようにする
        //vec.y = (vec.y > speed) ? speed : ((vec.y < -speed) ? -speed : vec.y);  // yの値がspeedを超えないようにする
        vec.z = 0f;




        transform.position += vec;
    }
}
