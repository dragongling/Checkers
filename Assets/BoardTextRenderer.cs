using UnityEngine;
using TMPro;

public class BoardTextRenderer : MonoBehaviour
{
    public Board board;
    public GameObject textPrefab;
    public GameObject textParent;
    public Vector3 xCoordOffset;
    public Vector3 yCoordOffset;

    // Start is called before the first frame update
    void Start()
    {
        CreateBoardText();
    }

    private void CreateBoardText()
    {
        for (int y = 0; y < board.height; ++y)
        {
            var textPosision = transform.position + new Vector3(0, y, 0) + yCoordOffset;
            var text = $"{y + 1}";
            CreateText(text, textPosision);
        }
        for (int x = 0; x < board.width; ++x)
        {
            var textPosision = transform.position + new Vector3(x, 0, 0) + xCoordOffset;
            var text = $"{(char)('a' + x)}";
            CreateText(text, textPosision);
        }
    }

    private void CreateText(string text, Vector3 textPosision)
    {
        var currentText = Instantiate(textPrefab, textPosision, Quaternion.identity, textParent.transform);
        currentText.GetComponent<TextMeshPro>().text = text;
        currentText.name = text;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
