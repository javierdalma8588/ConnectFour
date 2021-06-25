using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Singleton")]
    public static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    [Header("Player dots")]
    public GameObject player1;
    public GameObject player2;

    [Header("Board UI")]
    public GameObject player1UI;
    public GameObject player2UI;
    public GameObject winUI;

    [Header("Dot Spawn Points")]
    public Transform[] spawnPoints;

    [Header("GameMode")]
    public bool AI = false;

    [Header("Turns")]
    bool player1Turns = true;
    bool gameOver = false;

    [Header("Board")]
    int height = 6;
    int lenght = 7;
    public int[,] board; //0 is empty, 1 player1 and 2 player2

    [Header("Current Piece")]
    GameObject currentPiece;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        board = new int[lenght, height];

        player1UI.SetActive(false);

        player2UI.SetActive(false);
    }

    public void PlayerUI(int column)
    {
        if (board[column, height - 1] == 0 && !gameOver)
        {
            if (player1Turns)
            {
                player1UI.SetActive(true);
                player1UI.transform.position = spawnPoints[column].transform.position;
            }
            else if(!AI)
            {
                player2UI.SetActive(true);
                player2UI.transform.position = spawnPoints[column].transform.position;
            }
        }
    }

    public void SelectColumn(int column)
    {
        //Debug.Log("GameManager Colum " + column);
        StartCoroutine(TakeTurn(column));
    }

    public IEnumerator TakeTurn(int column)
    {
        int randomColumn = Random.Range(0, lenght-1);
        if (UpdateBoardState(column) && (currentPiece == null || currentPiece.GetComponent<Rigidbody>().velocity == Vector3.zero) && !gameOver)
        {
            player1UI.SetActive(false);
            player2UI.SetActive(false);

            if (player1Turns)
            {
                Debug.LogError("Player 1");
                currentPiece = Instantiate(player1, spawnPoints[column]);
                player1Turns = !player1Turns;
                if (Win(1))
                {
                    gameOver = true;
                    UIManager._instance.EnableWinScreen();
                    UIManager._instance.winText.text = "Player 1 Won";
                    UIManager._instance.winText.color = Color.red;
                    //Debug.LogError("Player1 Won");
                }
                DebugBoard();

                yield return new WaitForSeconds(3);
                if (UpdateBoardState(randomColumn) && (currentPiece == null || currentPiece.GetComponent<Rigidbody>().velocity == Vector3.zero) && !gameOver && AI && !player1Turns)
                {
                    Debug.LogError("AI");
                    player1Turns = !player1Turns;
                    //SelectColumn(x);
                    currentPiece = Instantiate(player2, spawnPoints[randomColumn]);
                    if (Win(2))
                    {
                        gameOver = true;
                        UIManager._instance.EnableWinScreen();
                        UIManager._instance.winText.text = "Player 1 Won";
                        UIManager._instance.winText.color = Color.red;
                        //Debug.LogError("Player1 Won");
                    }
                    DebugBoard();
                }
            }
            else if(!AI)
            {
                //Debug.LogError("Player 2");
                currentPiece = Instantiate(player2, spawnPoints[column]);
                player1Turns = !player1Turns;
                if (Win(2))
                {
                    gameOver = true;
                    UIManager._instance.EnableWinScreen();
                    UIManager._instance.winText.text = "Player 2 Won";
                    UIManager._instance.winText.color = Color.yellow;
                    //Debug.LogError("Player2 Won");
                }
                DebugBoard();
            }
        }

        if(Draw())
        {
            gameOver = true;
            UIManager._instance.EnableWinScreen();
            UIManager._instance.winText.text = "Draw";
            //Debug.LogError("Draw");
        }
    }

    bool UpdateBoardState(int column)
    {
        if(currentPiece == null || currentPiece.GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            for (int row = 0; row < height; row++)
            {
                if (board[column, row] == 0)// the spot is empty
                {
                    if (player1Turns)
                    {
                        board[column, row] = 1;
                    }
                    else
                    {
                        board[column, row] = 2;
                    }
                    //Debug.Log("Piece being spawned at (" + column + " , " + row + ")");
                    return true;
                }
            }
        }
        //Debug.LogWarning("The column " + column + " is full");
        return false;
    }

    bool Win(int playerNumber)
    {
        //Check Horizontal Win
        for (int x = 0; x<lenght -3; x++)
        {
             for(int y = 0; y < height; y++)
            {
                if(board[x,y] == playerNumber && board[x + 1,y] == playerNumber && board[x + 2, y] == playerNumber && board[x + 3, y] == playerNumber)
                {
                    return true;
                }
            }
        }

        //Check Verticla Win
        for (int x = 0; x < lenght; x++)
        {
            for (int y = 0; y < height -3; y++)
            {
                if (board[x, y] == playerNumber && board[x, y + 1] == playerNumber && board[x, y + 2] == playerNumber && board[x, y + 3] == playerNumber)
                {
                    return true;
                }
            }
        }

        //Check Diagonal Win
        for (int x = 0; x < lenght -3; x++)
        {
            for (int y = 0; y < height - 3; y++)
            {
                if (board[x, y] == playerNumber && board[x + 1, y + 1] == playerNumber && board[x + 2, y + 2] == playerNumber && board[x + 3, y + 3] == playerNumber)
                {
                    return true;
                }
            }
        }

        for (int x = 0; x < lenght - 3; x++)
        {
            for (int y = 0; y < height - 3; y++)
            {
                if (board[x, y + 3] == playerNumber && board[x + 1, y + 2] == playerNumber && board[x + 2, y + 1] == playerNumber && board[x + 3, y] == playerNumber)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool Draw()
    {
        //Check the top row to make sure that nobody won in that case the match ends in a Draw
        for (int x =0; x <lenght; x++)
        {
            if(board[x, height-1] == 0)
            {
                return false;
            }
        }
        return true;
    }

    public void DebugBoard()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                sb.Append(board[i, j]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }
}
