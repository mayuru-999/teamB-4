using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstEventWindow : MonoBehaviour
{
    [SerializeField] private GameObject thisObj;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private Image mouseImage;

    //条件定義List
    private List<NotifyCondition> conditions = new List<NotifyCondition>();
    private class NotifyCondition
    {
        public string message;
        public string scene;
        public Func<bool> condition;
        public Func<bool> flag;
        public Action<bool> flagAction;
    }

    //同時にトリガーした時のためのキュー処理
    private Queue<string> queue = new Queue<string>();
    private bool showing = false;

    //条件取得のための変数群
    private BigBang bigbang;

    private string thisScene;
    private int m_gameLv;
    private int m_bigbang;

    private bool canBigbang = false;
    private bool isClicked = false;

    void Start()
    {
        Debug.Log("起動はしてるよ");
        thisObj.SetActive(false);

        thisScene = SceneManager.GetActiveScene().name;
        bigbang = FindAnyObjectByType<BigBang>();
        m_gameLv = SkillManage.Instance.gameLv;
        m_bigbang = SkillManage.Instance.bigbang;

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
            message = "ビッグバンができるよ～～～～～～～",
            scene = "souma.sence",
            condition = () => canBigbang,
            flag = () => SkillManage.Instance.firstBigbang,
            flagAction = (value) => SkillManage.Instance.firstBigbang = value
        });
        conditions.Add(new NotifyCondition
        {
            message = "",
            scene= "souma.sence",
            condition = () => m_bigbang >= 1,
            flag = () => SkillManage.Instance.firstAfterBigbang,
            flagAction = (value) => SkillManage.Instance.firstAfterBigbang = value
        });
        conditions.Add(new NotifyCondition
        {
            message = "",
            scene = "souma.sence",
            condition = () => m_gameLv >= 2,
            flag = () => SkillManage.Instance.firstLvUp,
            flagAction = (value) => SkillManage.Instance.firstLvUp = value
        });

        //スキルツリーでのアクション
        conditions.Add(new NotifyCondition
        {
            message = "すきるつりーにようこそ～～～～～",
            scene = "SkillTree",
            condition = () => m_bigbang >= 0,
            flag = () => SkillManage.Instance.firstVisitTree,
            flagAction = (value) => SkillManage.Instance.firstVisitTree = value
        });
        //conditions.Add(new NotifyCondition
        //{
        //    message = "",
        //    scene = "SkillTree",
        //    condition = () => ,
        //    flag = () => SkillManage.Instance.firstCreateUnlocked,
        //    flagAction = (value) => SkillManage.Instance.firstCreateUnlocked = value
        //});
        conditions.Add(new NotifyCondition
        {
            message = "",
            scene = "SkillTree",
            condition = () => m_bigbang >= 1,
            flag = () => SkillManage.Instance.firstResetSkill,
            flagAction = (value) => SkillManage.Instance.firstResetSkill = value
        });

        //惑星シーンでのアクション
        conditions.Add(new NotifyCondition
        {
            message = "",
            scene = "CreatePlanet",
            condition = () => true,
            flag = () => SkillManage.Instance.firstVisitCreate,
            flagAction = (value) => SkillManage.Instance.firstVisitCreate = value
        });
    }

    private void CheckConditions()
    {
        //条件全て確認
        foreach (var c in conditions)
        {
            //未実行かつ、条件、シーンが合致していれば
            if (!c.flag() && c.condition() && c.scene == thisScene)
            {
                //キューに登録
                Debug.Log($"登録したよ{c.message}");
                queue.Enqueue(c.message);
                c.flagAction(true);
            }
        }

        if (queue.Count > 0 && !showing)
        {
            StartCoroutine(ShowNext());
        }
    }

    //yield::関数を任意のタイミングで中断・再開できるモノらしい
    private IEnumerator ShowNext()
    {
        //キューがないなら終わり
        if(queue.Count == 0)
        {
            Debug.Log("見せてないよ");
            showing = false;
            yield break;
        }

        Debug.Log("見せてるよ");
        showing = true;
        string message = queue.Dequeue();　//dequeue::取り出してから削除

        thisObj.SetActive(true);
        thisObj.transform.localScale = Vector3.zero;
        thisObj.transform.DOScale(1f, 1f).SetEase(Ease.OutBack);
        descText.text = message;

        //時間で消すならこう
        //yield return new WaitForSeconds(3f);

        isClicked = false;
        yield return new WaitUntil(() => isClicked);

        thisObj.SetActive(false);

        //もう一度最初から
        yield return ShowNext();
    }

    //public void OnClick() => isClicked = true;
    public void OnClick()
    {
        isClicked = true;
        Debug.Log("くりっくしたよ");
    }
}
