using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GridType
{
	ThreeByThree,
	FourByFour
}
public class GridManager : MonoBehaviour {
    public Transform tileParent;
    public GameObject gridTile;
    public List<Tile> tileList;

    public int numOfRows;
    public int numOfCols;
    private float tileWidth;
    private float tileHeight;
    private int row_offSet = -2;
    private int col_offSet = -3;
    public Player playerScript;
    // Use this for initialization
    void Start() {
        GetHeightAndWidthOfTile();
        CreateGrid();
    }


    void GetHeightAndWidthOfTile()
    {
        tileWidth = gridTile.transform.localScale.x;
        tileHeight = gridTile.transform.localScale.z;
    }

    public void SetTileStatus()
    {
        for (int i = 0; i < tileList.Count; i++)
        {
            if (playerScript.tankPositionList.Contains(tileList[i].tileNumber))
            {
                tileList[i].isFilled = true;
            }
            else {
                tileList[i].isFilled = false;
            }
        }
    }

	void CreateGrid()
	{
		for (int row = 0; row < numOfRows; row++) {
			for (int col = 0; col < numOfCols; col++) {
				GameObject tile = Instantiate (gridTile, tileParent);
				tile.GetComponent<Tile> ().tileNumber = row*numOfRows + col;
                tile.GetComponent<Tile>().gridManager = this;
                tileList.Add (tile.GetComponent<Tile> ());
				tile.transform.localPosition = new Vector3 (row * tileWidth + row_offSet, 0, col * tileHeight + col_offSet);
                tile.name = "Tile " + (row * numOfRows + col).ToString();

            }
		}
	}


	// Update is called once per frame
	void Update () {
		
	}
}
