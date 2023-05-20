using System.Collections;
using UnityEngine;

public enum CubeState { DEFAULT, EFF_ON,READY4MERGE, MERGED}
public class GhostBallController : MonoBehaviour
{
    public CubeState cubeState;

    [SerializeField] private GameObject myPopUpImg;
    [SerializeField] private GameObject myEff;
    [SerializeField] private MergeManager mergeManager;

    private Quaternion originalRotation;
    private Vector3 previousTouchPosition;
    private float speed = 100f;
    private float duration = 1f;
    private float lastTapTime;
    private float tapDelay = 0.5f;
    private bool isCanTouch = false;

    private void OnEnable()
    {
        if (mergeManager.IsReady4Merge == true)
        {
            cubeState = CubeState.READY4MERGE;
            return;
        }
        myEff.SetActive(false);
        myPopUpImg.SetActive(true);
        originalRotation = Quaternion.identity;
        transform.rotation = originalRotation;
    }

    private void OnDisable()
    {
        cubeState = CubeState.DEFAULT;
        myPopUpImg.SetActive(false);
    }

    private void Update()
    {
        if(cubeState == CubeState.MERGED)
        {
            myEff.SetActive(false);
            return;
        }
        if (cubeState == CubeState.READY4MERGE) return;
        if(cubeState == CubeState.EFF_ON)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (Time.time - lastTapTime < tapDelay)
                    {
                        cubeState = CubeState.READY4MERGE;
                    }
                    lastTapTime = Time.time;
                }
            }
        }
        isCanTouch = !myPopUpImg.activeSelf;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                previousTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // 이전 터치 위치와 현재 터치 위치의 차이를 구함
                Vector3 deltaPosition = Camera.main.ScreenToViewportPoint(new Vector3(touch.position.x, touch.position.y, 0) - previousTouchPosition);

                // x 방향 회전
                transform.Rotate(0f, -deltaPosition.x * speed, 0f, Space.World);

                // y 방향 회전
                transform.Rotate(-deltaPosition.y * speed, 0f, 0f, Space.Self);

                // z 방향 회전
                float swipeAngle = Mathf.Atan2(deltaPosition.y, deltaPosition.x) * Mathf.Rad2Deg;
                transform.rotation = originalRotation * Quaternion.Euler(0f, 0f, swipeAngle);

                // 현재 터치 위치를 이전 터치 위치로 업데이트
                previousTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if(isCanTouch == true && cubeState != CubeState.EFF_ON)
                {
                    StopCoroutine(RotateBackToOriginalRotation());
                    transform.rotation = Quaternion.identity;
                    // 마우스 포인터 위치로 Ray를 발사
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    // Raycast를 실행하여 충돌한 오브젝트 검사
                    if (Physics.Raycast(ray, out hit))
                    {
                        StartCoroutine(Rotate4Merge(new Vector3(90f, 0f, 0f)));
                    }
                    return;
                }

                StartCoroutine(RotateBackToOriginalRotation());
            }
        }
    }

    private IEnumerator RotateBackToOriginalRotation()
    {
        Quaternion targetRotation = originalRotation;
        float lerpTime = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < lerpTime)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, elapsedTime / lerpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    private IEnumerator Rotate4Merge(Vector3 rotateAmount)
    {
        // 회전 종료 시의 각도
        Quaternion endRotation = Quaternion.Euler(transform.rotation.eulerAngles + rotateAmount);

        // 회전에 걸리는 시간 동안 각도를 보간하여 회전
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, endRotation, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
        originalRotation = transform.rotation;
        cubeState = CubeState.EFF_ON;
        myEff.SetActive(true);
        isCanTouch = false;
    }
}
