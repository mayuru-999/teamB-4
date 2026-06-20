using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class move : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    void Start()
    {
        content.DOAnchorPosY(content.anchoredPosition.y + 50f, 2.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
