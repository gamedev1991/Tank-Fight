using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text gameText;
    public Player player1;
    public Player player2;

    public GameObject gridManager1;
    public GameObject gridManager2;

    public GameObject player1_canvas;
    public GameObject player2_canvas;

    public GameObject camera1;
    public GameObject camera2;
    public GameObject bulletCamera;
    public GameObject startButton;

    public Text player1ScoreText;
    public Text player2ScoreText;

    public GameObject restartButton;

    public static int player1Score = 0;
    public static int player2Score = 0;

    private bool isGameOver;
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {

    }

    public void SetScore()
    {
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }

    public void CheckWinner()
    {
        if (player1Score >= 3)
        {
            gameText.text = "Player 1 wins";
            isGameOver = true;
        }
        else if (player2Score >= 3)
        {
            gameText.text = "Player 2 wins";
            isGameOver = true;
        }

        if (isGameOver)
        {
            camera1.SetActive(true);
            player1_canvas.SetActive(false);
            player2_canvas.SetActive(false);
            gridManager2.SetActive(false);
            gridManager1.SetActive(false);
            camera2.SetActive(false);
            player1.playerAttackInput.SetActive(false);
            player2.playerAttackInput.SetActive(false);
            restartButton.SetActive(true);

        }
    }

    public void OnClickRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnClickStart()
    {
        SetScore();
        startButton.SetActive(false);
        player1_canvas.SetActive(true);
        gameText.text = "Player 1 Select Tank";
        gridManager1.SetActive(true);
        StartCoroutine(Player2Setup());
    }

    IEnumerator Player2Setup()
    {
        yield return new WaitUntil(() => player1.isTankSetupDone == true);
        camera1.SetActive(false);
        player1_canvas.SetActive(false);
        player2_canvas.SetActive(true);
        gridManager2.SetActive(true);
        camera2.SetActive(true);
        gameText.text = "Player 2 Select Tank";
        StartCoroutine(Player1AttackCoroutine());
    }

    IEnumerator Player1AttackCoroutine()
    {
        yield return new WaitUntil(() => player2.isTankSetupDone == true);
        Player1Attack();

    }

    public void Player1Attack()
    {
        player1.playerAttackInput.SetActive(true);
        camera1.SetActive(true);
        camera2.SetActive(false);
        gameText.text = "Player 1 Attack";
    }

    public void Player2Attack()
    {
        player2.playerAttackInput.SetActive(true);
        camera1.SetActive(false);
        camera2.SetActive(true);
        gameText.text = "Player 2 Attack";

    }

}