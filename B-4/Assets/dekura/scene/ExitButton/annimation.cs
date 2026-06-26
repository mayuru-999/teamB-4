using UnityEngine;
using UnityEngine.UI;

public class asd : MonoBehaviour
{
    private Image image;

    void Start()
    {
        image = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        image.color = Color.HSVToRGB(Time.time % 1, 1, 1);
    }
}
