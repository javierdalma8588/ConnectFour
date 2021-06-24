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

    [Header("Dot Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Turns")]
    bool player1Turns = true;

    [Header("Board")]
    int height = 6;
    int lenght = 7;
    public int[,] board; //0 is empty, 1 player1 and 2 player2

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
    }

    public void SelectColumn(int column)
    {
        Debug.Log("GameManager Colum " + column);
        TakeTurn(column);
    }

    public void TakeTurn(int column)
    {
        if(UpdateBoardState(column))
        {
            if (player1Turns)
            {
                Instantiate(player1, spawnPoints[column]);
            }
            else
            {
                Instantiate(player2, spawnPoints[column]);
            }

            player1Turns = !player1Turns;
        }
    }

    bool UpdateBoardState(int column)
    {
        for(int row = 0; row < height ; row++)
        {
            if(board[column, row] == 0)// the spot is empty
            {
                if (player1Turns)
                {
                    board[column, row] = 1;
                }
                else
                {
                    board[column, row] = 2;
                }
                Debug.Log("Piece being spawned at ("+ column +" , " + row +")");
                return true;
            }
        }
        Debug.LogWarning("The column " + column + " is full");
        return false;
    }
}
