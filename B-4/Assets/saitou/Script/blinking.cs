using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextColor : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI colorText;

    void Start()
    {
        colorText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        StartCoroutine("Flashing");
    }

    IEnumerator Flashing()
    {
        while (true)
        {
            for (int i = 0; i < 20; i++)
            {
                colorText.color = colorText.color - new Color32(0, 0, 0, 10);
                yield return new WaitForSeconds(0.01f);
            }

            for (int k = 0; k < 20; k++)
            {
                colorText.color = colorText.color + new Color32(0, 0, 0, 10);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}