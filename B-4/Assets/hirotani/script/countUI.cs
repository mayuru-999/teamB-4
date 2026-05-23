
using UnityEngine;
using UnityEngine.UI;

public class CountUI : MonoBehaviour
{
    public Text text;

    void Update()
    {
        text.text = DropItem.destroyedCount.ToString();
    }
}

