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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectColumn(int column)
    {

    }

    public void TakeTurn(int column)
    {
        Instantiate(player1, spawnPoints[column]);
    }
}
