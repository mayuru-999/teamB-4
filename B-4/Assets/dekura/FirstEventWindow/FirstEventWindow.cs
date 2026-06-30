using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstEventWindow : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject window;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Image mouseImage;
    [SerializeField] private Image shadow;

    //条件定義List
    private List<NotifyCondition> conditions = new List<NotifyCondition>();
    private class NotifyCondition
    {
        public string scene;
        public string flagKey;
        public string message;
        public Func<bool> condition;
    }

    //同時にトリガーした時のためのキュー処理
    private Queue<string> queue = new Queue<string>();
    private bool showing = false;

    //条件取得のための変数たち
    private BigBang bigbang;

    private string thisScene;
    private int m_gameLv;
    private int m_bigbang;
    private int m_skillpoint;

    private bool canBigbang = false;
    private bool isClicked = false;

    //Uiの設定
    private Color defaltInfoColor;

    void Start()
    {
        canvas.SetActive(false);

        thisScene = SceneManager.GetActiveScene().name;
        bigbang = FindAnyObjectByType<BigBang>();
        m_gameLv = SkillManage.Instance.gameLv;
        m_bigbang = SkillManage.Instance.bigbang;
        m_skillpoint = SkillPointManager.Instance.skillPoint;

        defaltInfoColor = infoText.color;
        infoText.color = Color.clear;

        SetUp();
        CheckConditions();
    }
    void Update()
    {
        if (thisScene == "souma.sence")
        {
            if (bigbang == null || canBigbang) return;
            canBigbang = bigbang.canUseBigBang;
            CheckConditions();
        }
    }

    private void SetUp()
    {
        //メインシーンでのアクション
        conditions.Add(new NotifyCondition
        {
            scene = "souma.sence",
            flagKey = "gameStart",
            condition = () => true,

            message = "マウスでレティクルを操作！\n" +
                      "惑星を破壊し\n" +
                      "鉱石を集めよう！",
        });
        conditions.Add(new NotifyCondition
        {
            scene = "souma.sence",
            flagKey = "firstBigbang",
            condition = () => canBigbang,

            message = "中央の惑星が点滅すると\n" +
                      "左クリック長押しで\n" +
                      "ビッグバンができる合図！\n" +
                      "スキルをリセットする代わりに\n" +
                      "ボーナスをゲットできるぞ！",
        });
        conditions.Add(new NotifyCondition
        {
            scene = "souma.sence",
            flagKey = "firstAfterBigbang",
            condition = () => m_bigbang >= 1,

            message = "ビッグバン後は星のかけらの\n" +
                      "ドロップ率UP！\n" +
                      "どんどんビッグバンを起こし、\n" +
                      "星のかけらを集めよう！",
        });
        conditions.Add(new NotifyCondition
        {
            scene = "souma.sence",
            flagKey= "firstLvUp",
            condition = () => m_gameLv >= 2,

            message = "ビッグバンを繰り返すと\n" +
                      "惑星LVがアップ！\n" +
                      "LVが高い惑星を壊して\n" +
                      "鉱石やかけらをたくさん集めよう！",
        });

        //スキルツリーでのアクション
        conditions.Add(new NotifyCondition
        {
            scene = "SkillTree",
            flagKey= "firstVisitTree",
            condition = () => m_bigbang >= 0,

            message = "鉱石でスキルを開放！\n" +
                    　"右のノードをクリック！\n" +
                      "どんどん強化していこう！",
        });
        conditions.Add(new NotifyCondition
        {
            scene = "SkillTree",
            flagKey= "firstCreateUnlocked",
            condition = () => m_skillpoint >= 20,

            message = "惑星作成がアンロック！\n" +
                      "地球のボタンを押してみよう！",
        });
        conditions.Add(new NotifyCondition
        {
            scene = "SkillTree",
            flagKey= "firstResetSkill",
            condition = () => m_bigbang >= 1,

            message = "ビッグバン後は\n" +
                      "スキルツリーがリセット！\n" +
                      "繰り返し強化して\n" +
                      "スキルツリーを完成させよう！",
        });

        //惑星シーンでのアクション
        conditions.Add(new NotifyCondition
        {
            scene = "CreatePlanet",
            flagKey= "firstVisitCreate",
            condition = () => true,

            message = "星のかけらで惑星を創造！\n" +
                    　"惑星を作ってボーナスを獲得し、\n" +
                      "すべての惑星を創造しよう！",
        });
    }

    private void CheckConditions()
    {
        //条件全て確認
        foreach (var c in conditions)
        {
            bool flags = SkillManage.Instance.GetFlags(c.flagKey);

            //未実行かつ、条件、シーンが合致していれば
            if (!flags && c.condition() && c.scene == thisScene)
            {
                //キューに登録
                queue.Enqueue(c.message);
                SkillManage.Instance.SetFlags(c.flagKey, true);
            }
        }

        if (queue.Count > 0 && !showing)
        {
            StartCoroutine(ShowNext());
        }
    }

    //yield::IEnumeratorでつかえる、関数を任意のタイミングで中断・再開できるモノらしい
    private IEnumerator ShowNext()
    {
        //キューがないなら終わり
        if(queue.Count == 0)
        {
            showing = false;
            yield break;
        }

        if (thisScene == "souma.sence") BigBang.Instance.theWorld(true);

        showing = true;
        string message = queue.Dequeue();　//dequeue::取り出してから削除

        canvas.SetActive(true);
        infoText.color = Color.clear;
        window.transform.localScale = Vector3.zero;
        window.transform.DOScale(1f, 1f).SetEase(Ease.OutBack);
        shadow.DOFade(0.8f, 1f);
        descText.text = message;

        yield return new WaitForSeconds(1.5f);

        infoText.color = defaltInfoColor;

        isClicked = false;
        yield return new WaitUntil(() => isClicked);

        yield return window.transform
            .DOScale(0f, 0.5f)
            .SetEase(Ease.InBack)
            .WaitForCompletion();

        yield return shadow.DOFade(0f, 0.5f);

        canvas.SetActive(false);

        if (thisScene == "souma.sence") BigBang.Instance.theWorld(false);

        //もう一度最初から
        yield return ShowNext();
    }

    public void OnClick() => isClicked = true;
}
