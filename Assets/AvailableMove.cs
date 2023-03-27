using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableMove : MonoBehaviour
{
    public Checker owner;
    public Checker beatenChecker;
    public bool isPromotingMove;
    private PlayerManager playerManager;
    private Game game;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindObjectOfType<Game>();
        playerManager = game.playerManager;
    }

    public void MakeMove()
    {
        owner.transform.position = transform.position;
        if (isPromotingMove)
        {
            owner.MakeKing();
        }
        Game.ClearAvailableMoves();
        if (beatenChecker != null)
        {
            beatenChecker.Destroy();
            var comboAttackMoves = owner.isKing ? owner.GetKingAttackMoves() : owner.GetAttackMoves();
            if (comboAttackMoves.Count == 0)
            {
                game.NextTurn();
            }
            else
            {
                game.isComboMode = true;
                foreach (var move in comboAttackMoves)
                {
                    owner.MakeAvailableMove(move);
                }
            }
        }
        else
        {
            game.NextTurn();
        }
    }

    // Update is called once per frame 
    void Update()
    {

    }
}
