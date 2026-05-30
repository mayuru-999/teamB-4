using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    public RectTransform arrow;
    private int counter = 0;
    private float move = 0.04f;
    private bool stop = false;

    void Update()
    {
        if (!stop)
        {
            arrow.position += new Vector3(0, move, 0);
            counter++;
            if (counter == 500)
            {
                counter = 0;
                move *= -1;
                stop = true;
            }
        }
        if (stop)
        {
            counter++;
            if(counter == 45)
            {
                counter = 0;
                stop = false;
            }
        }
      
    }
}