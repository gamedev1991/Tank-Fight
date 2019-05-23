using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankItem : MonoBehaviour {
  
    public Image tankImage;
    public Text tankDesc;
    public Player player;
    public TankType tankType;

    public void OnClickTank()
    {
        int index = (int)tankType;
        player.tankSelected = index;
        if (player.tankList.Contains(index))
        {
            player.tankList.Remove(index);        
        }
        player.tankSelection.SetActive(false);
        this.gameObject.SetActive(false);
        GameManager.Instance.gameText.text = "Place Tank";
    }
}
