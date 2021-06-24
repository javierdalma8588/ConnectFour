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

    [Header("Player UI")]
    public GameObject player1UI;
    public GameObject player2UI;

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
        Debug.Log("GameManager Colum " + column);
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
            }
            else
            {
                currentPiece = Instantiate(player2, spawnPoints[column]);
            }

            player1Turns = !player1Turns;
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
                    Debug.Log("Piece being spawned at (" + column + " , " + row + ")");
                    return true;
                }
            }
        }
        Debug.LogWarning("The column " + column + " is full");
        return false;
    }
}
