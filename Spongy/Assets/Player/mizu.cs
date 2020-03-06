using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mizu : MonoBehaviour
{
    [SerializeField]movesample2 mizu_check;
    BuoyancyEffector2D huryoku;
    int mizu_ryou;
    // Start is called before the first frame update
    void Start()
    {
        huryoku = GetComponent<BuoyancyEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mizu_ryou = mizu_check.Hydrated_check();
        if (mizu_ryou < 50)
        {
            huryoku.density = 2;
        }
        else
        {
            huryoku.density = 0;
        }
    }
}
