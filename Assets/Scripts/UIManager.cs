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
        ghostRedStateText.text = "레드 고스트볼 증강 상태 : " + (ghostRedController.gameObject.activeInHierarchy == true ? " O " : " X ") + "\n"
             + "레드 고스트 볼 상태 : " + ghostRedController.cubeState + "\n";

        ghostBlueStateText.text = "블루 고스트볼 증강 상태 : " + (ghostBlueController.gameObject.activeInHierarchy == true ? " O " : " X ") + "\n"
     + "블루 고스트 볼 상태 : " + ghostBlueController.cubeState + "\n";
    }
}
