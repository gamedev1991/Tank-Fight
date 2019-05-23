using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {
	
	public int tileNumber;
    public bool isFilled;
    public GridManager gridManager;
    public GameObject tank;
    public GameObject shellExplosion;
    public GameObject tankExplosion;
    Color invalidMouseOverColor = Color.red;
    Color validMouseOverColor = Color.green;
    private GameObject tankPlaced;
    private TankType tankType;
    //This stores the GameObject’s original color
    Color originalColor;
    MeshRenderer renderer;
    // Use this for initialization
    void Start () {
        renderer = GetComponent<MeshRenderer>();
        //Fetch the original color of the GameObject
        originalColor = renderer.material.color;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseOver()
    {
        if (gridManager.playerScript.isTankSetupDone)
        {
            return;
        }
        //If your mouse hovers over the GameObject with the script attached, output this message
        if (isFilled)
        {
            renderer.material.color = invalidMouseOverColor;
        }
        else
        {
            renderer.material.color = validMouseOverColor;
        }
    }

    private void OnMouseDown()
    {
        if (gridManager.playerScript.tankSelection.activeSelf || gridManager.playerScript.isTankSetupDone || isFilled)
        {
            return;
        }
        isFilled = true;
        gridManager.playerScript.tankPositionList.Add(tileNumber);
        if (gridManager.playerScript.tankList.Count > 0)
        {
            gridManager.playerScript.tankSelection.SetActive(true);
            GameManager.Instance.gameText.text = "Select Tank";
        }
        else {
            gridManager.playerScript.isTankSetupDone = true;
        }

        GameObject tankObj = Instantiate(tank, this.transform);
        tankPlaced = tankObj;
        tankType = (TankType)gridManager.playerScript.tankSelected;
        tankObj.transform.localScale = new Vector3(0.3f, 10f, 0.3f);
        // Get all of the renderers of the tank.
        MeshRenderer[] renderers = tankObj.GetComponentsInChildren<MeshRenderer>();

        // Go through all the renderers...
        for (int i = 0; i < renderers.Length; i++)
        {
            if (gridManager.playerScript.tankSelected.Equals((int)TankType.TankP))
            {
                // ... set their material color to the color specific to this tank.
                renderers[i].material.color = Color.green;
            }
            else {
                renderers[i].material.color = Color.red;
            }
        }

        gridManager.SetTileStatus();

    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        renderer.material.color = originalColor;
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (!GameManager.Instance.player1.tileNumberToAttack.Equals(tileNumber) && !GameManager.Instance.player2.tileNumberToAttack.Equals(tileNumber))
        {
            return;
        }

        if (other.tag.Equals("Bullet"))
        {
            StartCoroutine(CheckForCollision(other));
        }
    }

    IEnumerator CheckForCollision(Collider other)
    {
        yield return new WaitUntil(() => GameManager.Instance.player1.hasBulletReached || GameManager.Instance.player2.hasBulletReached);
        
        if (isFilled)
        {
            GameObject explosion = Instantiate(tankExplosion, this.transform) as GameObject;
            explosion.SetActive(true);
            if (tankType.Equals(TankType.TankQ))
            {
                tankType = TankType.TankP;
                MeshRenderer[] renderers = tankPlaced.GetComponentsInChildren<MeshRenderer>();

                // Go through all the renderers...
                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].material.color = Color.green;
                }
            }
            else {
                tankPlaced.SetActive(false);
                if (GameManager.Instance.player1.hasBulletReached)
                {
                    GameManager.player1Score++;
                }
                else {
                    GameManager.player2Score++;
                }
                GameManager.Instance.SetScore();
                GameManager.Instance.CheckWinner();
            }
            
        }
        else
        {
            GameObject explosion = Instantiate(shellExplosion, this.transform) as GameObject;
            explosion.SetActive(true);
        }
        other.gameObject.SetActive(false);
        GameManager.Instance.player1.hasBulletReached = false;
        GameManager.Instance.player2.hasBulletReached = false;
        GameManager.Instance.player1.tileNumberToAttack = -1;
        GameManager.Instance.player2.tileNumberToAttack = -1;

    }
}
