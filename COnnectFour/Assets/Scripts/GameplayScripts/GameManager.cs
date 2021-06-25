using System.Collections;
using System.Collections.Generic;
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

    [Header("Turns")]
    bool player1Turns = true;

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
        if (board[column, height - 1] == 0)
        {
            if (player1Turns)
            {
                player1UI.SetActive(true);
                player1UI.transform.position = spawnPoints[column].transform.position;
            }
            else
            {
                player2UI.SetActive(true);
                player2UI.transform.position = spawnPoints[column].transform.position;
            }
        }
    }

    public void SelectColumn(int column)
    {
        //Debug.Log("GameManager Colum " + column);
        TakeTurn(column);
    }

    public void TakeTurn(int column)
    {
        if(UpdateBoardState(column) && (currentPiece == null || currentPiece.GetComponent<Rigidbody>().velocity == Vector3.zero))
        {
            player1UI.SetActive(false);
            player2UI.SetActive(false);
            if (player1Turns)
            {
                currentPiece = Instantiate(player1, spawnPoints[column]);
                if(Win(1))
                {
                    Debug.LogError("Player1 Won");
                }
            }
            else
            {
                currentPiece = Instantiate(player2, spawnPoints[column]);
                if (Win(2))
                {
                    Debug.LogError("Player2 Won");
                }
            }

            player1Turns = !player1Turns;
        }

        if(Draw())
        {
            Debug.LogError("Draw");
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
                    Instantiate(winUI, new Vector3(x, y, 0), Quaternion.identity);
                    Instantiate(winUI, new Vector3(x + 1, y, 0), Quaternion.identity);
                    Instantiate(winUI, new Vector3(x + 2, y, 0), Quaternion.identity);
                    Instantiate(winUI, new Vector3(x + 3, y, 0), Quaternion.identity);
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
                    Instantiate(winUI, new Vector3(x, y, 0), Quaternion.identity);
                    Instantiate(winUI, new Vector3(x, y + 1, 0), Quaternion.identity);
                    Instantiate(winUI, new Vector3(x, y + 2, 0), Quaternion.identity);
                    Instantiate(winUI, new Vector3(x, y + 3, 0), Quaternion.identity);
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
                    Instantiate(winUI, new Vector3(x, y, 0), Quaternion.identity);
                    Instantiate(winUI, new Vector3(x + 1, y + 1, 0), Quaternion.identity);
                    Instantiate(winUI, new Vector3(x + 2, y + 2, 0), Quaternion.identity);
                    Instantiate(winUI, new Vector3(x + 3, y + 3, 0), Quaternion.identity);
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
                    Instantiate(winUI, new Vector3(x, y + 3, 0), Quaternion.identity);
                    Instantiate(winUI, new Vector3(x + 1, y + 2, 0), Quaternion.identity);
                    Instantiate(winUI, new Vector3(x + 2, y + 1, 0), Quaternion.identity);
                    Instantiate(winUI, new Vector3(x + 3, y, 0), Quaternion.identity);
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
}
