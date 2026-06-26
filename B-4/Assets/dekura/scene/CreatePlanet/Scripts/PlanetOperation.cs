using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlanetOperation : MonoBehaviour
{
    [Header("惑星")]
    [SerializeField] private GameObject[] planets;
    [SerializeField] public Sprite hidenPlane;
    [SerializeField] public Sprite canUnlockPlane;
    [SerializeField] public Sprite errorPlane;

    [Header("サイズ設定")]
    [SerializeField] private Transform orbitCenter;
    [SerializeField] private float orbitRadius = 3f;
    [SerializeField] private float defaultSize = 1.0f;
    [SerializeField] private float expandedSize = 1.5f;
    [SerializeField] private float expandDuration = 0.4f;

    [Header("回転設定")]
    [SerializeField] private float rotationDuration = 0.4f;
    [SerializeField] private float currentAngleOffset = 0f;
    [SerializeField] private Ease easeType = Ease.InOutQuad;

    private float stepAngle;
    private bool isRotating = false;
    private int currentFocusedIndex = 6;

    private PlanetUiManager planetUiManager;

    void Awake()
    {
        planetUiManager = FindAnyObjectByType<PlanetUiManager>();

        // 中心の円を透明にする
        orbitCenter.GetComponent<Image>().color = Color.clear;
        //360度/惑星の数、で回転のステップ角度を計算
        stepAngle = 360f / planets.Length;

        PlaceCirclesOnOrbit(currentAngleOffset);
        ExpandPlanet();
    }

    //移動処理
    void PlaceCirclesOnOrbit(float angleOffset)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            float angleDeg = angleOffset + i * stepAngle;
            float angleRad = angleDeg * Mathf.Deg2Rad;

            //単位円
            planets[i].transform.position = orbitCenter.position + new Vector3(
                Mathf.Cos(angleRad) * orbitRadius,
                Mathf.Sin(angleRad) * orbitRadius,
                0f
            );
        }
    }

    //ボタン操作
    public void RotateRight()
    {
        TryRotate(+stepAngle);
        SoundsManager.Instance.PlaySound("select");
    } 
    public void RotateLeft() 
    {
        TryRotate(-stepAngle);
        SoundsManager.Instance.PlaySound("select");
    }
    public void GetPlanet() 
    {
        planets[GetObjectIndex()].GetComponent<PlaneSkill>().OnClick();
        planetUiManager.UpdateUI();
    }

    public GameObject GetFocusobject() => planets[GetObjectIndex()];


    //回転処理
    void TryRotate(float deltaAngle)
    {
        if (isRotating) return;
        isRotating = true;

        //始点、終点
        float startAngle = currentAngleOffset;
        float endAngle = currentAngleOffset + deltaAngle;

        // DOTween で float をアニメーション
        DOTween.To(
            () => startAngle,
            angle => PlaceCirclesOnOrbit(angle),
            endAngle,
            rotationDuration
        )
        .SetEase(easeType)
        .OnComplete(() =>
        {
            PlaceCirclesOnOrbit(endAngle); // スナップ
            currentAngleOffset = endAngle;
            isRotating = false;

            ExpandPlanet(); // 回転後に拡大処理を呼び出す
            planetUiManager.UpdateUI();
        });
    }

    //拡大処理
    public void ExpandPlanet()
    {
        // 惑星を見つける
        int tgIndex = GetObjectIndex();
        if (tgIndex == currentFocusedIndex) return; 

        // 拡大していたオブジェクトを戻す
        if (currentFocusedIndex >= 0)
        {
            planets[currentFocusedIndex].transform
                .DOScale(defaultSize, expandDuration)
                .SetEase(Ease.OutQuad);
        }

        // オブジェクトを拡大
        planets[tgIndex].transform
            .DOScale(expandedSize, expandDuration)
            .SetEase(Ease.OutBack); // 少しバウンドするかんじ

        currentFocusedIndex = tgIndex;
    }

    // 真下に来るオブジェクトを取得
    public int GetObjectIndex()
    {
        int tgIndex = 0;

        for (int i = 0; i < planets.Length; i++)
        {
            // 現在の惑星の角度を計算
            float angleDeg = (currentAngleOffset + i * stepAngle) % 360f;
            if (angleDeg < 0) angleDeg += 360f; // マイナス角度を正規化

            //真下(270度)
            //if (angleDeg == 270f) tgIndex = i;
            if (angleDeg == 90f) tgIndex = i;
        }

        return tgIndex;
    }
}
