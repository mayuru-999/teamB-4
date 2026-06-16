using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class SenceChang : MonoBehaviour
{
    public float changeTime = 30f;
    public TMP_Text timeText;
    public Image targetImage; // �F�ύX�p

    public Transform scaleImage;
    private Vector3 initialScale;

    // �ǉ��F���̉摜4��
    public Image[] borderImages;

    private float remainingTime;
    private bool isFinished = false;

    // �_�ŗp

    private float blinkInterval = 0.2f; // �_�ŊԊu
    private float blinkCounter = 0f;
    private bool isBlinkVisible = true;

 

    void Start()
    {
        remainingTime = changeTime;
        initialScale = scaleImage.localScale;

        // �ŏ��͔�\��
        foreach (Image img in borderImages)
        {
            img.enabled = false;
        }

    }

    void Update()
    {
        if (isFinished) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            isFinished = true;
            SceneManager.LoadScene("SkillTree_debug");
        }

        // �\��
        timeText.text = remainingTime.ToString("F1") + "s";

        // �c��2�b�Ő�
        if (remainingTime <= 2f)
        {
            timeText.color = Color.red;
            targetImage.color = Color.red;

            // �_�ŏ���
            BlinkBorders();
        }
        else
        {
            timeText.color = Color.white;
            targetImage.color = Color.white;

            // ��ɔ�\���ɂ���
            foreach (Image img in borderImages)
            {
                img.enabled = false;
            }
        }

        // �X�P�[���k��
        float rate = remainingTime / changeTime;
        Vector3 newScale = initialScale;
        newScale.x = initialScale.x * rate;
        scaleImage.localScale = newScale;
    }


    void BlinkBorders()
    {
        blinkCounter += Time.deltaTime;

        if (blinkCounter >= blinkInterval)
        {
            blinkCounter = 0f;
            isBlinkVisible = !isBlinkVisible; // ON/OFF�؂�ւ�
        }

        foreach (Image img in borderImages)
        {
            img.enabled = isBlinkVisible;
        }

    }
}