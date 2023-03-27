using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

public class PlayerManager : MonoBehaviour
{
    public Player currentPlayer;
    public List<Player> players => transform.Cast<Transform>().Select(playerObject => playerObject.GetComponent<Player>()).ToList();
    // Start is called before the first frame update
    void Start()
    {
    }

    public Player GetNextPlayer()
    {
        if (currentPlayer == null)
        {
            return GetFirstPlayer();
        }
        var childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Player>() == currentPlayer)
            {
                if (i == childCount - 1)
                {
                    return GetFirstPlayer();
                }
                else
                {
                    return transform.GetChild(i + 1).GetComponent<Player>();
                }
            }
        }
        return GetFirstPlayer();
    }

    public Player GetFirstPlayer()
    {
        return transform.GetChild(0).GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void CheckRemainingPlayers()
    {
        if (transform.childCount == 1)
        {
            Game.GetInstance().WonBy(transform.GetChild(0).GetComponent<Player>());
        }
    }

    internal void RemovePlayer(Player ownerPlayer)
    {
        ownerPlayer.transform.parent = null;
        Destroy(ownerPlayer.gameObject);
    }
}
