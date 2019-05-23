using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TankType
{
    TankP,
    TankQ
}


public class Player : MonoBehaviour {
    public List<int> tankPositionList;
    public Transform tankListParent;
    public GameObject tankItem;
    public List<int> tankList = new List<int>{ 0, 0, 1 };
    public Sprite[] tankImages;
    public GameObject tankSelection;
    public int tankSelected;
    public bool isTankSetupDone;

    public InputField rowInputField;
    public InputField columnInputField;
    private int rowNumberToAttack;
    private int colNumberToAttack;
    public GridManager gridManager;
    public GameObject playerAttackInput;
    public GameObject shell;
    public int tileNumberToAttack;
    public bool hasBulletReached;
    // Use this for initialization
    void Start() {
        PopulateTankSelectionList();
        rowInputField.onEndEdit.AddListener(GetRowNumber);
        columnInputField.onEndEdit.AddListener(GetColumnNumber);
    }

    void GetRowNumber(string userInput)
    {
        rowNumberToAttack = int.Parse(userInput);
    }

    void GetColumnNumber(string userInput)
    {
        colNumberToAttack = int.Parse(userInput);
    }

    void PopulateTankSelectionList()
    {
        for (int i = 0; i < tankList.Count; i++)
        {
            GameObject tank = Instantiate(tankItem, tankListParent);
            tank.GetComponent<TankItem>().player = this;
            if (tankList[i] == (int)TankType.TankP)
            {
                tank.GetComponent<TankItem>().tankImage.sprite = tankImages[(int)TankType.TankP];
                tank.GetComponent<TankItem>().tankDesc.text = "Tank P";
                tank.GetComponent<TankItem>().tankType = TankType.TankP;
            }
            else {
                tank.GetComponent<TankItem>().tankImage.sprite = tankImages[(int)TankType.TankQ];
                tank.GetComponent<TankItem>().tankDesc.text = "Tank Q";
                tank.GetComponent<TankItem>().tankType = TankType.TankQ;

            }
        }
    }

    public void FireShell()
    {
        hasBulletReached = false;
        tileNumberToAttack = rowNumberToAttack + colNumberToAttack * gridManager.numOfRows;
        playerAttackInput.SetActive(false);
        GameManager.Instance.camera1.SetActive(false);
        GameManager.Instance.camera2.SetActive(false);
        GameManager.Instance.bulletCamera.SetActive(true);
        if (this.tag == "Player1")
        {
            Vector3 startPos = new Vector3(GameManager.Instance.gridManager1.transform.position.x,3, GameManager.Instance.gridManager1.transform.position.z);
            GameObject shellInstance =
                   Instantiate(shell, startPos, Quaternion.identity) as GameObject;
            shellInstance.transform.eulerAngles = new Vector3(0, 90, 0);
            shellInstance.transform.DOMove(GameManager.Instance.gridManager2.GetComponent<GridManager>().tileList[tileNumberToAttack].transform.position, 5).OnComplete(BulletReached);
            GameManager.Instance.bulletCamera.transform.DOMove(GameManager.Instance.camera2.transform.position, 5);
        }
        else {
            Vector3 startPos = new Vector3(GameManager.Instance.gridManager2.transform.position.x, 3, GameManager.Instance.gridManager2.transform.position.z);

            GameObject shellInstance =
                   Instantiate(shell, startPos, Quaternion.identity) as GameObject;
            shellInstance.transform.eulerAngles = new Vector3(0, -90, 0);
            shellInstance.transform.DOMove(GameManager.Instance.gridManager1.GetComponent<GridManager>().tileList[tileNumberToAttack].transform.position, 5).OnComplete(BulletReached);            
            GameManager.Instance.bulletCamera.transform.DOMove(GameManager.Instance.camera1.transform.position, 5);
        }
       
    }

    void BulletReached()
    {
        hasBulletReached = true;
        if (this.tag == "Player1")
        {
            GameManager.Instance.Player2Attack();
        }
        else {
            GameManager.Instance.Player1Attack();
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
