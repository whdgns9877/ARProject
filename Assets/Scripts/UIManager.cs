using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ghostRedStateText;
    [SerializeField] private TextMeshProUGUI ghostBlueStateText;

    [SerializeField] private GhostBallController ghostRedController;
    [SerializeField] private GhostBallController ghostBlueController;

    // Update is called once per frame
    void Update()
    {
        ghostRedStateText.text = "���� ��Ʈ�� ���� ���� : " + (ghostRedController.gameObject.activeInHierarchy == true ? " O " : " X ") + "\n"
             + "���� ��Ʈ �� ���� : " + ghostRedController.cubeState + "\n";

        ghostBlueStateText.text = "��� ��Ʈ�� ���� ���� : " + (ghostBlueController.gameObject.activeInHierarchy == true ? " O " : " X ") + "\n"
     + "��� ��Ʈ �� ���� : " + ghostBlueController.cubeState + "\n";
    }
}
