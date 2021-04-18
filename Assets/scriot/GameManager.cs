using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    //壁用定数
    public const int WALLFLONT = 1;
    public const int WALLRIGHT = 2;
    public const int WALLBACK = 3;
    public const int WALLLEFT = 4;
    public int nowWall;

    //メッセージマスタ
    DataTable messageMaster = new DataTable();

    #region ゲームオブジェクト(共通)
    public GameObject PanelWalls;
    public Button B_Message;
    public Image bigImage;
    public int messagePageCounter = 0;
    public string showingMessageName = "";
    public GameObject selectPanel;
    public Button selectionFirst;
    public Button selectionSecond;
    string selectFirstTriger = "";
    string selectSecondTriger = "";
    public AudioClip Crick;
    public AudioClip select;
    public AudioClip finish;
    public int yukkuriCount = 0;
    public AudioClip getYukkuri;
    #endregion

    #region 第一画面
    public Sprite tiruno;
    public Sprite mail;
    public Sprite alice;
    public AudioSource source;
    public AudioClip hihhi;
    public AudioClip ohoihoi;
    public AudioClip zurakare;
    public AudioClip keyOpen;
    bool ariceVoiceFlg = true;
    bool isReadAriceMessage = false;
    bool isGetRadio = false;
    bool aliceGetFlg = false;
    #endregion

    #region 第二画面
    public Button PC;
    public Button kimePC;
    public Image kimeBikkuri;
    public Image kimeKyoton;
    public Image kimeTere;
    public Image kimeOko;
    public AudioClip turnOn;
    public AudioClip claping;
    public Button radio;
    public Sprite radioPicture;
    public AudioClip switchOff;
    public Sprite kimeBigPic;
    bool isGetKime = false;
    #endregion

    #region 第三画面
    public Image spotLite;
    public AudioClip switchSE;
    bool denkyuFlg = false;
    public AudioClip gasagoso;
    public Sprite memo;
    bool memoFlg = false;
   
    #endregion

    #region 第四画面
    private bool isGetIceCream = false;
    public Sprite ice;
    public bool isLookedReizou = false;
    public bool isGetRimokon=false;
    public Button rimokonButton;
    public Sprite rimokon;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        nowWall = WALLFLONT;
        List<string> columns = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10","11","12","13"};
        columns.ForEach(x => messageMaster.Columns.Add(x));
        SetAllMessages();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
                if (!string.IsNullOrEmpty(showingMessageName))
                {

                    DataRow targetRow = messageMaster.NewRow();
                    foreach (DataRow dr in messageMaster.Rows)
                    {
                        if (dr[0].ToString() == showingMessageName)
                        {
                            targetRow = dr;
                            break;
                        }
                    }
                    if (targetRow != null)
                    {
                        ShowOrCloseMessage(targetRow[messagePageCounter].ToString());
                        source.PlayOneShot(Crick);
                    }
                }
        }
    }

    #region メッセージマスタ確保

    private void SetAllMessages()
    {
        //ポスター
        List<string> posterMessages = new List<string>() { "つまり私は、劇団員を探せばいいのかな？", "一体どこにいるんだろう…" };
        AddOneMessage("ポスター１", posterMessages);

        //アリスのメッセージ
        List<string> mailMessage = new List<string>() { "何か書いてあるな、ええと…？", "「只今演技の練習中。満足するまで不在にします。」", "「目標：拍手喝采して頂ける演技」", "「名悪役 アリス」", "まいったな、出てくるのを待つしかないんだろうか？" };
        AddOneMessage("メール１", mailMessage);

        //追加メッセージなし
        List<string> kinkoMessage = new List<string>() { "" };
        AddOneMessage("なし", kinkoMessage);

        //きめぇまる登場
        List<string> kimeTojo = new List<string> {"きめ「ふっふっふ。」", "きめ「流石のあなたでも、PCの中には手を出せない」", "きめ「図星でしょう！」", "きめ「これで私の勝ちですね！」", "困ったぞ、なんとか出てきてくれないかな" };
        AddOneMessage("きめぇ", kimeTojo);

        //ラジオゲット
        List<string> getRadio = new List<string>() {"何か役に立つかもしれない、持っていこう" };
        AddOneMessage("ラジオ", getRadio);

        //アリスゲット１
        List<string> getAlice = new List<string>() {"アリス「メッセージを見て、考えてくれたんでしょう？」","アリス「ラジカセから拍手を頂いたんだもの、練習はおしまい」" ,"アリス「あと%s人、頑張って見つけてね」"};
        AddOneMessage("アリス", getAlice);

        List<string> denkyuMesseage = new List<string>() {"…あ、そうか。この部屋、他の部屋の照明とは別に","電球が設置してあるんだね。","………。","え、何のために？？？"};
        AddOneMessage("電球", denkyuMesseage);

        List<string> getMemo = new List<string>() { "字が書いてあるけど、汚くて読みにくいぞ…", "うぅーーーん…", "『アタイを呼びたければ、サイキョーと叫べ！』", "って、書いてあるのかな、これは", "一応、覚えておくか" };
        AddOneMessage("メモ", getMemo);

        List<string> gomibako = new List<string>() { "『アタイを呼びたければ、サイキョーと叫べ！』", "って、書いてあったんだよな" };
        AddOneMessage("ゴミ箱", gomibako);

        List<string> getIce = new List<string>() {"アイスクリームだ。良く冷えてる","そういえば、お腹すいたな…","……","どこかに座って、食べようかな…","すみません、後で買って返しますので…",};
        AddOneMessage("アイス", getIce);

        List<string> getKime = new List<string>() { "きめぇ「あ……。」" };
        AddOneMessage("getきめぇ", getKime);

        List<string> getKime2 = new List<string>() { "きめぇ「出てくるしかありませんねえ」","きめぇ「決して、アイスに釣られてはいませんよ」","きめぇ「あと%s人ですか、頑張ってください」"};
        AddOneMessage("getきめぇ２",getKime2);

        List<string> reizou = new List<string>() { "む、よく見ると、奥の方に何か見える", "水色の羽…あそこにちるのがいる？", "どうにか出てきてもらえないかな"};
        AddOneMessage("冷蔵室", reizou);

        List<string> osezi = new List<string>() { "きめ「えっ、そ、そうですか？」","うん、キャラクターの魅せ方が上手いというか…","きめ「ふ、ふふ、当然ですとも！」","きめ「私こそ、文庫劇団の二枚目ですから！」","そんなきめぇ丸のサイン、欲しいなあ"
            　　　　　　　　　　　　　　　　　　　,"出てきてくれないかな…","きめ「そういうことなら…」","きめ「って、その手には乗りませんよ！」","きめ「おだてたって、ここからは出てあげません！」","失敗か…うーん","あ、でもちょっと、嬉しそうな顔してる"};
        AddOneMessage("お世辞", osezi);

        List<string> madaGoal = new List<string>() { "これはさいきょーのドア","4人のゆっくりを集めた者は","この扉を開けることができる！","なるほど、ゆっくりを4人探せばいいのか" };
        AddOneMessage("閉じたゴール", madaGoal);

        List<string> goal = new List<string>() { "ガチャッ！", "鍵の音が鳴った…", "僕はそっと、さいきょーのドアを開いた","その先には…" };
        AddOneMessage("開いたゴール", goal);

        List<string> rimokon = new List<string>() { "これって、何かのリモコン？", "…あ、よく見ると、照明用って書いてある","部屋の照明のリモコンなのか。どの部屋のだろう？" };
        AddOneMessage("リモコン", rimokon);
    }

    private void AddOneMessage(string messageName, List<string> messages)
    {

        DataRow dr = messageMaster.NewRow();
        dr[0] = messageName;
        int i = 1;
        foreach (string message in messages)
        {
            dr[i] = message;
            i++;
            if (i == 15)
            {
                break;
            }
        }
        messageMaster.Rows.Add(dr);
    }
    #endregion

    #region ゲームメソッド（共通）

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    public void PushLeftDoor()
    {
        if (messagePageCounter == 0 && !selectPanel.activeSelf)
        {
            nowWall--;
            if (nowWall == 0)
            {
                nowWall = WALLLEFT;
            }
            DisplayWall();
        }
    }

    public void PushRightDoor()
    {
        if (messagePageCounter == 0 && !selectPanel.activeSelf)
        {
            nowWall++;
            if (nowWall == 5)
            {
                nowWall = WALLFLONT;
            }
            DisplayWall();
        }
    }

    public void PushSelectFirst()
    {
        selectionFirst.gameObject.SetActive(false);
        selectionSecond.gameObject.SetActive(false);
        selectPanel.SetActive(false);
        source.PlayOneShot(select);
        switch (selectFirstTriger)
        {
            case "金庫不正解":
                ShowOrCloseMessage("あ、あの、ちょっと出てきて休憩とか…", "なし");
                Invoke("PlayZurakareVoice", 2);
                break;

            case "きめぇ正解":
                ShowOrCloseMessage("あの、これ", "getきめぇ");
                yukkuriCount++;
                isGetKime = true;
                break;

            case "冷蔵室":
                if (!isLookedReizou)
                {
                    ShowOrCloseMessage("なんか、妙に冷えてないか…？", "冷蔵室");
                    isLookedReizou = true;
                }
                else 
                {
                    ShowOrCloseMessage("どうにか出てきてもらえないかな…", "なし");
                    selectionFirst.gameObject.SetActive(true);
                    selectionFirst.GetComponentInChildren<Text>().text = "名前を呼ぶ";
                    if (memoFlg)
                    {
                        selectionSecond.gameObject.SetActive(true);
                        selectionSecond.GetComponentInChildren<Text>().text = "サイキョ―と叫ぶ";
                    }
                    selectPanel.SetActive(true);
                    selectFirstTriger = "チルノ不正解";
                    selectSecondTriger = "チルノ正解";
                }
                break;

            case "チルノ不正解":
                break;
        }
    }

    public void PushSelectSecond()
    {
        selectionFirst.gameObject.SetActive(false);
        selectionSecond.gameObject.SetActive(false);
        selectPanel.SetActive(false);
        source.PlayOneShot(select);
        switch (selectSecondTriger)
        {
            case "金庫正解":
                yukkuriCount++;
                ShowOrCloseMessage("ここで、拍手の音を鳴らせば…", "なし");
                Invoke("GetAlice1", 1);
                aliceGetFlg = true;
                break;

            case "きめぇ不正解":
                ShowOrCloseMessage("あ、あの、きめぇ丸ってさ、いつも良い演技だよね", "お世辞");
                break;

            case "冷凍室":
                if (!isGetIceCream)
                {
                    messagePageCounter = 0;
                    B_Message.gameObject.SetActive(false);
                    ShowOrCloseMessage("ん、これって…", "アイス");
                    bigImage.sprite = ice;
                    bigImage.gameObject.SetActive(true);
                    isGetIceCream = true;
                }
                else 
                {
                    ShowOrCloseMessage("皆を見つけ終わったら、早くアイスを買い戻そう…", "なし");
                }
                break;

            case "チルノ正解":
                break;
        }
    }

    private void GetAlice1() 
    {
        source.PlayOneShot(claping);
        Invoke("GetAlice2", 3);
    }

    private void GetAlice2() 
    {
        source.PlayOneShot(keyOpen);
        Invoke("GetAlice3", 2);
    }

    private void GetAlice3() 
    {
        B_Message.gameObject.SetActive(false);
        bigImage.sprite = alice;
        bigImage.gameObject.SetActive(true);
        source.PlayOneShot(getYukkuri);
        ShowOrCloseMessage("アリス「うふふ、そんなに良い演技だった？」", "アリス");
    }

    private void PlayZurakareVoice()
    {
        source.PlayOneShot(zurakare);
        Invoke("KinkoNgMessage", 2);
    }

    private void KinkoNgMessage()
    {
        source.PlayOneShot(Crick);
        ShowOrCloseMessage("…うーん、出てきてくれないな", "なし");
    }


    public void DisplayWall()
    {
        switch (nowWall)
        {
            case WALLFLONT:
                PanelWalls.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                break;

            case WALLRIGHT:
                PanelWalls.transform.localPosition = new Vector3(-1000.0f, 0.0f, 0.0f);
                break;

            case WALLBACK:
                PanelWalls.transform.localPosition = new Vector3(-2000.0f, 0.0f, 0.0f);
                break;

            case WALLLEFT:
                PanelWalls.transform.localPosition = new Vector3(-3000.0f, 0.0f, 0.0f);
                break;
        }
    }

    public void ShowOrCloseMessage(string message = "", string showingMessage = "")
    {
        //メッセージを閉じる
        if (string.IsNullOrEmpty(message))
        { 
            B_Message.gameObject.SetActive(false);
            showingMessageName = "";
            messagePageCounter = 0;
            bigImage.gameObject.SetActive(false);
            string nowMessage = B_Message.GetComponentInChildren<Text>().text;
            switch (nowMessage) 
            {
                case "え、何のために？？？":
                    source.PlayOneShot(switchSE);
                    spotLite.gameObject.SetActive(false);
                    break;

                case "あ、でもちょっと、嬉しそうな顔してる":
                    kimeTere.gameObject.SetActive(false);
                    break;    

                case "きめぇ「あ……。」":
                    source.PlayOneShot(switchOff);
                    kimePC.gameObject.SetActive(false);
                    PC.gameObject.SetActive(true);

                    StartCoroutine(DelayMethod(1.5f, () =>
                    {
                        source.PlayOneShot(getYukkuri);
                        bigImage.sprite = kimeBigPic;
                        bigImage.gameObject.SetActive(true);
                        ShowOrCloseMessage("きめぇ「やれやれ、そこまで言われちゃ、」", "getきめぇ２");
                    }));
                    
                    break;

                default:
                    break;
            }
            return;
        }

        //メッセージを出す
        if (!B_Message.IsActive())
        {
            B_Message.gameObject.SetActive(true);
            showingMessageName = showingMessage;
            }
        int yukkuriNokoriCount = 4 - yukkuriCount;
        B_Message.GetComponentInChildren<Text>().text =Regex.Replace(message,"%s",yukkuriNokoriCount.ToString());
        messagePageCounter++;
        string nowMessage2 = B_Message.GetComponentInChildren<Text>().text;
        switch (nowMessage2)
        {
            case "きめ「えっ、そ、そうですか？」":
                kimeBikkuri.gameObject.SetActive(true);
                break;

            case "きめ「ふ、ふふ、当然ですとも！」":
                kimeBikkuri.gameObject.SetActive(false);
                break;

            case "きめ「って、その手には乗りませんよ！」":
                kimeOko.gameObject.SetActive(true);
                break;

            case "失敗か…うーん":
                kimeOko.gameObject.SetActive(false);
                kimeTere.gameObject.SetActive(true);
                break;
        }
    }
    #endregion

    #region 第一画面
    public void PosterClick()
    {
        if (messagePageCounter == 0 && !selectPanel.activeSelf)
        {
            source.PlayOneShot(Crick);
            ShowOrCloseMessage("文庫劇団員、かくれんぼ中？", "ポスター１");
            bigImage.sprite = tiruno;
            bigImage.gameObject.SetActive(true);
        }
    }

    public void MailClick()
    {
        if (messagePageCounter == 0 && !selectPanel.activeSelf)
        {
            source.PlayOneShot(Crick);
            if (!aliceGetFlg)
            {
                ShowOrCloseMessage("あれ、紙が落ちてる…", "メール１");
                bigImage.sprite = mail;
                bigImage.gameObject.SetActive(true);
                isReadAriceMessage = true;
            }
            else 
            {
                ShowOrCloseMessage("この挿絵、自分で描いたのかな。上手いな…", "なし");
                bigImage.sprite = mail;
                bigImage.gameObject.SetActive(true);
            }
        }
    }

    public void KinkoClick()
    {
        if (messagePageCounter == 0 && !selectPanel.activeSelf&&!aliceGetFlg)
        {
            if (ariceVoiceFlg)
            {
                source.PlayOneShot(hihhi);
                ariceVoiceFlg = false;
            }
            else
            {
                source.PlayOneShot(ohoihoi);
                ariceVoiceFlg = true;
            }

            if (isReadAriceMessage)
            {
                Invoke("KinkoMessage", 1);
            }
        }
        else 
        {
            source.PlayOneShot(Crick);
            ShowOrCloseMessage("こんな金庫で、缶詰演技練習か…ストイックだなあ", "なし");
        }
    }

    private void KinkoMessage()
    {
        source.PlayOneShot(Crick);
        ShowOrCloseMessage("ここにいるよな…どうにか出てきてくれないかな", "なし");
        Invoke("ShowKinkoSelect", 1);
    }

    private void ShowKinkoSelect()
    {
        selectionFirst.gameObject.SetActive(true);
        selectionFirst.GetComponentInChildren<Text>().text = "外に出るよう誘ってみる";
        if (isGetRadio)
        {
            selectionSecond.gameObject.SetActive(true);
            selectionSecond.GetComponentInChildren<Text>().text = "ラジカセを再生してみる";
        }
        else
        {
            selectionSecond.gameObject.SetActive(false);
        }
        selectPanel.SetActive(true);
        selectFirstTriger = "金庫不正解";
        selectSecondTriger = "金庫正解";
    }

    #endregion

    #region 第二画面

    public void ClickPC()
    {
        if (messagePageCounter == 0 && !selectPanel.activeSelf)
        {
            if (!isGetKime)
            {
                source.PlayOneShot(Crick);
                ShowOrCloseMessage("ノートパソコンだ", "なし");
                Invoke("OnKimePC", 1);
            }
            else
            {
                ShowOrCloseMessage("一体どうやって、ここに入ってたんだ？", "なし");
            }
        }
    }

    private void OnKimePC()
    {

            B_Message.gameObject.SetActive(false);
            PC.gameObject.SetActive(false);
            kimePC.gameObject.SetActive(true);
            source.PlayOneShot(turnOn);
            ShowOrCloseMessage("うわ！", "きめぇ");
    }

    public void ClickKimePC()
    {
        if (messagePageCounter == 0 && !selectPanel.activeSelf)
        {
            if (isGetIceCream)
            {
                source.PlayOneShot(Crick);
                ShowOrCloseMessage("どうやってPCから出てきてもらおう？", "なし");
                Invoke("ShowKimeSelect", 1);
            }
            else
            {
                source.PlayOneShot(Crick);
                ShowOrCloseMessage("きめ「なんだか少し、お腹が減りましたね」", "なし");
            }
        }
    }

    private void ShowKimeSelect()
    {
        selectionFirst.gameObject.SetActive(true);
        selectionFirst.GetComponentInChildren<Text>().text = "アイスを見せる";
        selectionSecond.gameObject.SetActive(true);
        selectionSecond.GetComponentInChildren<Text>().text = "お世辞を言ってみる";
        selectPanel.SetActive(true);
        selectFirstTriger = "きめぇ正解";
        selectSecondTriger = "きめぇ不正解";
    }

    public void ClickRadio() 
    {
        if (messagePageCounter == 0 && !selectPanel.activeSelf)
        {
            bigImage.sprite = radioPicture;
            bigImage.gameObject.SetActive(true);
            source.PlayOneShot(Crick);
            ShowOrCloseMessage("これって、ラジカセ？何が入ってるんだろ", "なし");
            radio.gameObject.SetActive(false);
            Invoke("PlayRadioSound", 1);
        }
    }

    private void PlayRadioSound() 
    {
        source.PlayOneShot(claping);
        Invoke("GetRadio", 2);
    }

    private void GetRadio() 
    {
        B_Message.gameObject.SetActive(false);
        messagePageCounter = 0;
        ShowOrCloseMessage("拍手の効果音か…。", "ラジオ");
        source.PlayOneShot(Crick);
        isGetRadio = true;
    }

    #endregion

    #region 第三画面
    public void PushLightSwitch() 
    {
        if (messagePageCounter == 0 && !selectPanel.activeSelf)
        {
            if (isGetRimokon)
            {
                source.PlayOneShot(Crick);
                ShowOrCloseMessage("これって、電気のスイッチかな。どうしようか？", "なし");
                StartCoroutine(DelayMethod(1.0f, () =>
                {
                    selectionFirst.gameObject.SetActive(true);
                    selectionFirst.GetComponentInChildren<Text>().text = "壁のスイッチを押してみる";
                    selectionSecond.gameObject.SetActive(true);
                    selectionSecond.GetComponentInChildren<Text>().text = "リモコンのスイッチを押してみる";
                    selectPanel.SetActive(true);
                    selectFirstTriger = "壁スイッチ";
                    selectSecondTriger = "リモコン";
                }));

            }
            else
            {
                source.PlayOneShot(Crick);
                ShowOrCloseMessage("これって、電気のスイッチかな。押してみよう", "なし");
                StartCoroutine(DelayMethod(1.5f, () =>
                {
                    source.PlayOneShot(switchSE);
                    spotLite.gameObject.SetActive(true);
                    Invoke("DenkyuOn", 1);
                    return;
                }));
            }
        }
    }

    private void DenkyuOn() 
    {
        messagePageCounter = 0;
        B_Message.gameObject.SetActive(false);
        source.PlayOneShot(Crick);
        ShowOrCloseMessage("んん…？これ、どうなってるんだ？","電球");
    }

    public void PushGomibako() 
    {
        if (messagePageCounter == 0 && !selectPanel.activeSelf)
        {
            if (!memoFlg)
            {
                source.PlayOneShot(Crick);
                ShowOrCloseMessage("…ん？これって…", "なし");
                source.PlayOneShot(gasagoso);
                StartCoroutine(DelayMethod(1.5f, () =>
                {
                    messagePageCounter = 0;
                    B_Message.gameObject.SetActive(false);
                    bigImage.sprite = memo;
                    bigImage.gameObject.SetActive(true);
                    source.PlayOneShot(Crick);
                    ShowOrCloseMessage("何だろう、落書きかな？", "メモ");
                    memoFlg = true;
                }));
            }
            else
            {
                source.PlayOneShot(Crick);
                ShowOrCloseMessage("ゴミ箱だ。ここで拾った紙には、確か", "ゴミ箱");
            }
        }
    }

    #endregion

    #region 第四画面
    public void PushReizouko() 
    {
        if (messagePageCounter == 0 && !selectPanel.activeSelf)
        {
            source.PlayOneShot(Crick);
            ShowOrCloseMessage("冷蔵庫か…。用があるのは…？", "なし");
            StartCoroutine(DelayMethod(1.0f, () =>
            {
                selectionFirst.gameObject.SetActive(true);
                selectionFirst.GetComponentInChildren<Text>().text = "冷蔵室";
                selectionSecond.gameObject.SetActive(true);
                selectionSecond.GetComponentInChildren<Text>().text = "冷凍室";
                selectPanel.SetActive(true);
                selectFirstTriger = "冷蔵室";
                selectSecondTriger = "冷凍室";
            }));
        }
    }

    public void PushLastDoor()
    {
        if (messagePageCounter == 0 && !selectPanel.activeSelf)
        {
            source.PlayOneShot(Crick);
            if (yukkuriCount < 4)
            {
                ShowOrCloseMessage("このドア、貼り紙がしてあるな", "閉じたゴール");
                return;
            }

            ShowOrCloseMessage("よし、ゆっくりを4人集めたぞ", "開いたゴール");
        }
    }

    public void GetRimokon() 
    {
        bigImage.sprite = rimokon;
        bigImage.gameObject.SetActive(true);
        source.PlayOneShot(Crick);
        ShowOrCloseMessage("ん、何か落ちてる", "リモコン");
        isGetRimokon = true;
        rimokonButton.gameObject.SetActive(false);
    }

    #endregion
}