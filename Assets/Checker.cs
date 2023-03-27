using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Checker : MonoBehaviour
{
    public Player ownerPlayer;
    public GameObject checkerMovePrefab;
    public SpriteRenderer kingMark;
    public bool isKing;
    private SpriteRenderer spriteRenderer;
    public static readonly List<Vector3> DefaultMoveOffsets = new List<Vector3> {
        new Vector3(-1, 1, 0),
        new Vector3(1, 1, 0)
        };
    public static readonly List<Vector3> AttackMoveOffsets = new List<Vector3> {
        new Vector3(-1, 1, 0),
        new Vector3(1, 1, 0),
        new Vector3(1, -1, 0),
        new Vector3(-1, -1, 0)
        };
    public List<Vector3> moveOffsets;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (ownerPlayer != null)
        {
            spriteRenderer.color = ownerPlayer.color;
            kingMark.color = ColorHelper.Invert(ownerPlayer.color);
        }
    }

    public void HandleClick()
    {
        var game = FindObjectOfType<Game>();
        if (!game.isOver && !game.isComboMode && ownerPlayer == game.playerManager.currentPlayer)
        {
            CreateAvailableMoves();
        }
    }

    public void CreateAvailableMoves()
    {
        Game.ClearAvailableMoves();

        var availableMoves = new List<MoveData>();
        if (!isKing)
        {
            availableMoves.AddRange(GetPassiveMoves());
            availableMoves.AddRange(GetAttackMoves());
        }
        else
        {
            // king logic
            availableMoves.AddRange(GetKingMoves());
        }
        foreach (var move in availableMoves)
        {
            MakeAvailableMove(move);
        }
    }

    private IEnumerable<MoveData> GetKingMoves()
    {
        var availableMoves = new List<MoveData>();
        foreach (var moveOffset in AttackMoveOffsets)
        {
            var newPosition = transform.position;
            Checker beatenChecker = null;
            while (true)
            {
                newPosition += moveOffset;
                var positionData = GetPositionData(newPosition);
                if (!positionData.IsOnBoard)
                    break;
                else if (positionData.checker == null)
                {
                    availableMoves.Add(new MoveData
                    {
                        newPosition = newPosition,
                        beatenChecker = beatenChecker
                    });
                }
                else if (positionData.checker == ownerPlayer)
                {
                    break;
                }
                else if (beatenChecker == null)
                {
                    beatenChecker = positionData.checker;
                    continue;
                }
                else
                {
                    break;
                }
            }
        }
        return availableMoves;
    }

    public List<MoveData> GetKingAttackMoves()
    {
        var availableMoves = new List<MoveData>();
        foreach (var moveOffset in AttackMoveOffsets)
        {
            var newPosition = transform.position;
            Checker beatenChecker = null;
            while (true)
            {
                newPosition += moveOffset;
                var positionData = GetPositionData(newPosition);
                if (!positionData.IsOnBoard)
                    break;
                else if (positionData.checker == null)
                {
                    if (beatenChecker == null)
                        continue;
                    availableMoves.Add(new MoveData
                    {
                        newPosition = newPosition,
                        beatenChecker = beatenChecker
                    });
                }
                else if (positionData.checker == ownerPlayer)
                {
                    break;
                }
                else if (beatenChecker == null)
                {
                    beatenChecker = positionData.checker;
                    continue;
                }
                else
                {
                    break;
                }
            }
        }
        return availableMoves;
    }

    public List<MoveData> GetAttackMoves()
    {
        var availableMoves = new List<MoveData>();
        foreach (var moveOffset in AttackMoveOffsets)
        {
            var newPosition = transform.position + moveOffset;
            var positionData = GetPositionData(newPosition);
            if (positionData.IsOnBoard && positionData.checker != null && positionData.checker.ownerPlayer != ownerPlayer)
            {
                newPosition += moveOffset;
                var beatMoveData = GetPositionData(newPosition);
                if (beatMoveData.IsOnBoard && beatMoveData.checker == null)
                {
                    availableMoves.Add(new MoveData
                    {
                        newPosition = newPosition,
                        beatenChecker = positionData.checker,
                        isPromoteMove = positionData.owner != null && positionData.owner != ownerPlayer
                    });
                }
            }
        }
        return availableMoves;
    }

    public List<MoveData> GetPassiveMoves()
    {
        var availableMoves = new List<MoveData>();
        moveOffsets = DefaultMoveOffsets.Select(offset => ownerPlayer.attackDirection * offset).ToList();
        foreach (var moveOffset in moveOffsets)
        {
            var newPosition = transform.position + moveOffset;
            var positionData = GetPositionData(newPosition);
            if (positionData.IsOnBoard && positionData.checker == null)
            {
                availableMoves.Add(new MoveData
                {
                    newPosition = newPosition,
                    beatenChecker = null,
                    isPromoteMove = positionData.owner != null && positionData.owner != ownerPlayer
                });
            }
        }
        return availableMoves;
    }

    public void MakeAvailableMove(MoveData moveData)
    {
        var moveObject = Instantiate(
            checkerMovePrefab,
            moveData.newPosition,
            Quaternion.identity,
            Game.GetInstance().movesParent.transform);
        moveObject.name = $"Move to {moveData.newPosition}";
        var move = moveObject.GetComponent<AvailableMove>();
        move.owner = this;
        move.beatenChecker = moveData.beatenChecker;
        move.isPromotingMove = moveData.isPromoteMove;
    }

    PositionData GetPositionData(Vector3 position)
    {
        var positionData = new PositionData();
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(position);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("BoardCell"))
            {
                positionData.IsOnBoard = true;
                var ownable = hitCollider.GetComponent<Ownable>();
                positionData.owner = ownable.owner;
            }
            if (hitCollider.CompareTag("Checker"))
            {
                positionData.checker = hitCollider.GetComponent<Checker>();
            }
        }
        return positionData;
    }

    public void Destroy()
    {
        ownerPlayer.checkerCount--;
        if (ownerPlayer.checkerCount == 0)
        {
            Game.GetInstance().playerManager.RemovePlayer(ownerPlayer);
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void MakeKing()
    {
        isKing = true;
        kingMark.enabled = true;
    }
}
