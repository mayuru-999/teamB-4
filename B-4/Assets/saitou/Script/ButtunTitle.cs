using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class scene3 : MonoBehaviour
{

    float count = 0.0f;
    public float delay = 10.0f;
    bool log = false;

    private void Update()
    {
        count += 0.1f;

        if (count >= delay)
        {
            Debug.Log("count >= delay");
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