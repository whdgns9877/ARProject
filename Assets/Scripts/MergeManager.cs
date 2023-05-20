using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    //[SerializeField] private ObjectConnector objectConnector;
    [SerializeField] private List<GhostBallController> ghostBalls = new List<GhostBallController>();
    [SerializeField] private List<GhostBallController> readyToMerge = new List<GhostBallController>();
    [SerializeField] private GameObject LightningEff;
    [SerializeField] private TextMeshProUGUI trySummonCallCountText;

    [SerializeField] private Transform redMergePos;
    [SerializeField] private Transform blueMergePos;

    private float mergeDistance = 3f;
    private int trySummonCallCount = 0;
    private bool isTrySummonCall = false;
    private bool isReady4Merge = false;
    public bool IsReady4Merge { get { return isReady4Merge; } }

    private void OnEnable()
    {
        GameObject parentObject = GameObject.Find("GhostBalls"); // ��Ʈ���� �ִ� �θ� ������Ʈ�� �̸��� �����մϴ�.
        GhostBallController[] ghostBallsArr = parentObject.GetComponentsInChildren<GhostBallController>(); // �θ� ������Ʈ ������ �ִ� ��� ��Ʈ���� ã���ϴ�.
        foreach (GhostBallController ghostBall in ghostBallsArr)
        {
            ghostBalls.Add(ghostBall);
        }
    }

    private void OnDisable()
    {
        ghostBalls.Clear();
    }

    private void Update()
    {
        if(isReady4Merge == true)
        {
            foreach (GhostBallController ghostBall in ghostBalls)
            {
                if (!readyToMerge.Contains(ghostBall))
                {
                    readyToMerge.Add(ghostBall);
                }
            }
        }
        // ��ĥ �غ� �� GhostBallController�� ã��
        foreach (GhostBallController ghostBall in ghostBalls)
        {
            if (ghostBall.cubeState == CubeState.MERGED) return;
            if (ghostBall.cubeState == CubeState.READY4MERGE)
            {
                isReady4Merge = true;
            }
            else
            {
                if (readyToMerge.Contains(ghostBall))
                {
                    isReady4Merge = false;
                    readyToMerge.Remove(ghostBall);
                    LightningEff.SetActive(false);
                    CancelInvoke(nameof(TrySummon));
                }
            }
        }

        // ��ĥ ��Ʈ���� 2�� �̻��̸�, ��ĥ �غ� �� ��Ʈ������ �Ÿ��� ���ϰ�, ���� �Ÿ� ���Ͽ� ������ ��ħ
        if (readyToMerge.Count >= 2 && isTrySummonCall == false)
        {
            LightningEff.SetActive(true);
            if (Vector3.Distance(readyToMerge[0].gameObject.transform.position, readyToMerge[0].gameObject.transform.position) < mergeDistance)
            {
                LightningEff.SetActive(false);
                isTrySummonCall = true;
                readyToMerge[0].gameObject.transform.position = redMergePos.position;
                readyToMerge[0].cubeState = CubeState.MERGED;
                readyToMerge[1].gameObject.transform.position = blueMergePos.position;
                readyToMerge[1].cubeState = CubeState.MERGED;
                //objectConnector.StartConnection();
                InvokeRepeating(nameof(TrySummon), 0f,1f);
            }
        }
    }

    private void TrySummon()
    {
        trySummonCallCountText.text = "TrySummon�Լ� ȣ�� Ƚ�� : " + ++trySummonCallCount;
    }
}
