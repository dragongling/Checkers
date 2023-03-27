using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class Game : MonoBehaviour
{
    public PlayerManager playerManager;
    public Transform checkersCollection;
    public GameObject checkerPrefab;
    public Board board;
    public GameObject movesParent;
    public bool isComboMode;
    public bool isOver;
    public TextMeshProUGUI statusMessage;


    // Start is called before the first frame update
    void Start()
    {
        GameStart();
    }

    void GameStart()
    {
        NextTurn();
        foreach (var player in playerManager.players)
        {
            SetCheckersForPlayer(player);
        }
    }

    private void SetCheckersForPlayer(Player player)
    {
        for (int y = (int)player.checkerSpawnArea.yMin; y < (int)player.checkerSpawnArea.yMax; ++y)
        {
            for (int x = (int)player.checkerSpawnArea.xMin; x < (int)player.checkerSpawnArea.xMax; ++x)
            {
                if ((x + y) % 2 == 1)
                {
                    var position = checkersCollection.position + new Vector3(x, y, 0);
                    var checkerObject = Instantiate(checkerPrefab, position, Quaternion.identity, checkersCollection);
                    checkerObject.name = $"{player.name} Checker {x} {y}";
                    var checker = checkerObject.GetComponent<Checker>();
                    checker.ownerPlayer = player;
                    checker.ownerPlayer.checkerCount++;
                }
            }
        }
    }

    public void NextTurn()
    {
        var nextPlayer = playerManager.GetNextPlayer();
        if (playerManager.currentPlayer == nextPlayer)
        {
            WonBy(playerManager.currentPlayer);
        }
        else
        {
            playerManager.currentPlayer = playerManager.GetNextPlayer();
            isComboMode = false;
            statusMessage.text = $"{playerManager.currentPlayer.name} turn";
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void ClearAvailableMoves()
    {
        var previousMoves = GameObject.FindGameObjectsWithTag("AvailableMove");
        foreach (var move in previousMoves)
        {
            Destroy(move);
        }
    }

    public static Game GetInstance()
    {
        return FindObjectOfType<Game>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void WonBy(Player player)
    {
        statusMessage.text = $"{player.name} won!";
    }
}
