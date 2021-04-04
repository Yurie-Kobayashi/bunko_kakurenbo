using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    #endregion

    #region 第一画面
    public Sprite tiruno;
    public Sprite mail;
    public AudioSource source;
    public AudioClip hihhi;
    public AudioClip ohoihoi;
    public AudioClip zurakare;
    bool ariceVoiceFlg = true;
    bool isReadAriceMessage = false;
    bool isGetRadio = false;
    #endregion

    #region
    public Button kimePC;
    public Image kimeBikkuri;
    public Image kimeKyoton;
    public Image kimeTere;
    public Image kimeOko;
    public Button radio;
    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        nowWall = WALLFLONT;
        List<string> columns = new List<string>() { "0","1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
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
        List<string> posterMessages = new List<string>() { "つまり私は、劇団員を探せばいいのかな？","一体どこにいるんだろう…"};
        AddOneMessage("ポスター１", posterMessages);

        //アリスのメッセージ
        List<string> mailMessage = new List<string>() { "何か書いてあるな、ええと…？","「只今演技の練習中。満足するまで不在にします。」","「目標：拍手喝采して頂ける演技」" ,"「名悪役 アリス」","まいったな、出てくるのを待つしかないんだろうか？"};
        AddOneMessage("メール１", mailMessage);

        //金庫(アリスのメッセージ後
        List<string> kinkoMessage = new List<string>() {""};
        AddOneMessage("金庫", kinkoMessage);

    }

    private void AddOneMessage(string messageName,List<string> messages) 
    {
        DataRow dr = messageMaster.NewRow();
        dr[0] = messageName;
        int i = 1;
        foreach(string message in messages) 
        {
            dr[i] = message;
            i++;
            if (i == 11) 
            {
                break;
            }
        }
        messageMaster.Rows.Add(dr);
    }
    #endregion

    #region ゲームメソッド（共通）
    public void PushLeftDoor() 
    {
        if (messagePageCounter == 0&&!selectPanel.activeSelf)
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
        if (messagePageCounter == 0&&!selectPanel.activeSelf)
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
                ShowOrCloseMessage("あ、あの、ちょっと出てきて休憩とか…", "金庫");
                Invoke("PlayZurakareVoice", 2);
                break;
        }
    }

    public void PushSelectSecond()
    {
        selectionFirst.gameObject.SetActive(false);
        selectionSecond.gameObject.SetActive(false);
        selectPanel.SetActive(false);
        source.PlayOneShot(select);
        switch (selectFirstTriger)
        {
            case "金庫正解":

                break;
        }
    }


    private void PlayZurakareVoice() 
    {
        source.PlayOneShot(zurakare);
        Invoke("KinkoNgMessage", 2);
    }

    private void KinkoNgMessage() 
    {
        source.PlayOneShot(Crick);
        ShowOrCloseMessage("…うーん、出てきてくれないな", "金庫");
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

    public void ShowOrCloseMessage(string message="",string showingMessage="")
    {
        //メッセージを閉じる
        if (string.IsNullOrEmpty(message))
        {
            B_Message.gameObject.SetActive(false);
            showingMessageName = "";
            messagePageCounter = 0;
            bigImage.gameObject.SetActive(false);
            return;
        }

        //メッセージを出す
        if (!B_Message.IsActive())
        {
            B_Message.gameObject.SetActive(true);
            showingMessageName = showingMessage;
        }
        var messageButtonText = B_Message.GetComponentInChildren<Text>().text = message;
        messagePageCounter++;
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
            ShowOrCloseMessage("あれ、紙が落ちてる…", "メール１");
            bigImage.sprite = mail;
            bigImage.gameObject.SetActive(true);
            isReadAriceMessage= true;
        }
    }

    public void KinkoClick() 
    {
        if (messagePageCounter == 0 && !selectPanel.activeSelf)
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
    }

    private void KinkoMessage() 
    {
        source.PlayOneShot(Crick);
        ShowOrCloseMessage("ここにいるよな…どうにか出てきてくれないかな", "金庫");
        Invoke("ShowKinkoButton", 1);
    }

    private void ShowKinkoButton() 
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
        selectSecondTriger= "金庫正解";
    }

}
#endregion
