using System.Collections;
using System.Collections.Generic;
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

    #region ゲームオブジェクト(共通)
    public GameObject PanelWalls;
    public Button B_Message;
    public int messagePageCounter = 0;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        nowWall = WALLFLONT;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region ゲームメソッド（共通）
    public void PushLeftDoor() 
    {
        nowWall--;
        if (nowWall == 0) 
        {
            nowWall = WALLLEFT;
        }
        DisplayWall();
    }

    public void PushRightDoor() 
    {
        nowWall++;
        if (nowWall == 5)
        {
            nowWall = WALLFLONT;
        }
        DisplayWall();
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

    public void ChangeOrCloseMessage(string message="")
    {
        //メッセージを閉じる
        if (string.IsNullOrEmpty(message))
        {
            B_Message.gameObject.SetActive(false);
            return;
        }

        //メッセージを出す
        if (!B_Message.IsActive())
        {
            B_Message.gameObject.SetActive(true);
        }
        var messageButtonText = B_Message.GetComponentInChildren<Text>().text = message;
    }
    #endregion

    public void posterClick()
    {
        ChangeOrCloseMessage("文庫劇団員、かくれんぼ中？");
    }
}
