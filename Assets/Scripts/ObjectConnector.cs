using UnityEngine;

public class ObjectConnector : MonoBehaviour
{
    [SerializeField] private GameObject object1;
    [SerializeField] private GameObject object2;

    private bool isConnecting = false;
    private Vector3 connectionPoint;

    public void StartConnection()
    {
        // 두 오브젝트 간의 중심점을 구합니다.
        Vector3 centerPoint = (object1.transform.position + object2.transform.position) / 2f;

        // 두 오브젝트를 중심점으로 이동시킵니다.
        object1.transform.position = centerPoint + new Vector3(-0.5f, 0f, 0f);
        object2.transform.position = centerPoint + new Vector3(0.5f, 0f, 0f);

        // 연결 지점을 저장합니다.
        connectionPoint = centerPoint;
        isConnecting = true;
    }

    public void StopConnection()
    {
        isConnecting = false;
    }

    private void Update()
    {
        // 연결 중일 때, 두 오브젝트를 연결 지점에 맞춰 이동시킵니다.
        if (isConnecting)
        {
            Vector3 offset1 = object1.transform.position - connectionPoint;
            Vector3 offset2 = object2.transform.position - connectionPoint;
            object1.transform.position = connectionPoint + offset1;
            object2.transform.position = connectionPoint + offset2;
        }
    }
}
