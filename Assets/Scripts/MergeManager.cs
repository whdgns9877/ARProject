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
        GameObject parentObject = GameObject.Find("GhostBalls"); // 고스트볼이 있는 부모 오브젝트의 이름을 지정합니다.
        GhostBallController[] ghostBallsArr = parentObject.GetComponentsInChildren<GhostBallController>(); // 부모 오브젝트 하위에 있는 모든 고스트볼을 찾습니다.
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
        // 합칠 준비가 된 GhostBallController를 찾음
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

        // 합칠 고스트볼이 2개 이상이면, 합칠 준비가 된 고스트볼끼리 거리를 비교하고, 일정 거리 이하에 있으면 합침
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
        trySummonCallCountText.text = "TrySummon함수 호출 횟수 : " + ++trySummonCallCount;
    }
}
