using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Board : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject cellParent;
    public Color whiteCellColor;
    public Color blackCellColor;
    public int width = 8;
    public int height = 8;
    public float cellDistance = 1;
    // Start is called before the first frame update
    void Start()
    {
        CreateCells();
    }

    private void CreateCells()
    {
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                var position = transform.position + new Vector3(x * cellDistance, y * cellDistance, 0);
                var currentCell = Instantiate(cellPrefab, position, Quaternion.identity, cellParent.transform);
                currentCell.GetComponent<SpriteRenderer>().color = (x + y) % 2 == 0 ? whiteCellColor : blackCellColor;
                currentCell.name = $"{(char)('a' + x)}{y + 1}";
                Player baseOwner = null;
                if (y == 0)
                {
                    baseOwner = Game.GetInstance().playerManager.players[0];
                }
                if (y == height - 1)
                {
                    baseOwner = Game.GetInstance().playerManager.players[1];
                }
                if (baseOwner != null)
                {
                    var ownable = currentCell.GetComponent<Ownable>();
                    ownable.owner = baseOwner;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
