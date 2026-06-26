using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class scene3 : MonoBehaviour
{
    

    private void Start()
    {
       
    }
    

    float count = 0.0f;
    public float delay = 10.0f;
    public bool AutoChange = false;
    public float AutoChangeCount = 1000.0f;

    private void Update()
    {
        count += Time.deltaTime;

        if (count >= delay)
        {
            Debug.Log("count >= delay");
        }

        if (count >= AutoChangeCount)
        {
            Debug.Log("count >= AutoChangeCount");
            if (AutoChange)
            {
                SceneManager.LoadScene("TitleScene");
            }
            
        }

    }


    public void change_button()
    {
        if (count >= delay)
        {
            SceneManager.LoadScene("TitleScene");
        }
        
    }
}