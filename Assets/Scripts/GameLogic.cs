using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public GameObject chesspiece;

    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];
    //player stuff
    private string currentPlayer = "white";
    public int turnsWhite;
    public int turnsBlack;
    public int turns;
    //Turn Indicator
    public GameObject BlackTurn;
    public GameObject WhiteTurn;

    public bool gameOver = false;

    //Audio
    public AudioClip movepiece;

    public void PlayMovePiece()
    {
        this.GetComponent<AudioSource>().PlayOneShot(movepiece, 1);
    }
    void Start()
    {
        playerWhite = new GameObject[]
        {
          Create("wR", 0, 0), Create("wN", 1, 0), Create("wB", 2, 0),
          Create("wQ", 3, 0), Create("wK", 4, 0),
          Create("wR", 7, 0), Create("wN", 6, 0), Create("wB", 5, 0),
          Create("wP", 0, 1), Create("wP", 1, 1), Create("wP", 2, 1), Create("wP", 3, 1), Create("wP", 4, 1), Create("wP", 7, 1), Create("wP", 6, 1), Create("wP", 5, 1)
        };
        playerBlack = new GameObject[]
        {
            Create("bR", 0, 7), Create("bN", 1, 7), Create("bB", 2, 7),
            Create("bQ", 3, 7), Create("bK", 4, 7),
            Create("bB", 5, 7), Create("bN", 6, 7), Create("bR", 7, 7),
            Create("bP", 7, 6), Create("bP", 6, 6), Create("bP", 5, 6), Create("bP", 4, 6), Create("bP", 3, 6), Create("bP", 2, 6), Create("bP", 1, 6), Create("bP", 0, 6)
        };
        //Set Positions on the board
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
        //FInd Turn Text

    }
    public GameObject Create(string Piece, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(x, y, -1), Quaternion.identity);
        CHessmenScript cm = obj.GetComponent<CHessmenScript>();
        cm.name = Piece;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();
        return obj;
    }
    public void SetPosition(GameObject obj)
    {
        CHessmenScript cm = obj.GetComponent<CHessmenScript>();
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }
    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }
    public GameObject getPosition(int x, int y)
    {
        return positions[x, y];
    }
    public bool positionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }
    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }
    public bool GameOver()
    {
        return gameOver;
    }
    public void NextTurn()
    {
        if(currentPlayer == "white")
        {
            currentPlayer = "black";
            turnsWhite += 1;
            turns += 1;
            BlackTurn.GetComponent<Text>().enabled = true;
            WhiteTurn.GetComponent<Text>().enabled = false;
        }
        else
        {
            currentPlayer = "white";
            turnsBlack += 1;
            turns += 1;
            BlackTurn.GetComponent<Text>().enabled = false;
            WhiteTurn.GetComponent<Text>().enabled = true;
        }
        Debug.Log(turns);
    }
    public void Update()
    {
        if(gameOver && Input.GetMouseButtonUp(0))
        {
            gameOver = false;

            SceneManager.LoadScene("Game");
        }
    }
    public void WinnerText(string Winner)
    {
        gameOver = true;

        GameObject.FindGameObjectWithTag("WinText").GetComponent<Text>().text = Winner + " Wins!";
        GameObject.FindGameObjectWithTag("WinText").GetComponent<Text>().enabled = true;

        GameObject.FindGameObjectWithTag("LoseText").GetComponent<Text>().enabled = true;
    }
    public void SetGameOver()
    {
        gameOver = true;
    }
    public void loadmainmenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
