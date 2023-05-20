using UnityEngine;

public class ObjectConnector : MonoBehaviour
{
    [SerializeField] private GameObject object1;
    [SerializeField] private GameObject object2;

    private bool isConnecting = false;
    private Vector3 connectionPoint;

    public void StartConnection()
    {
        // �� ������Ʈ ���� �߽����� ���մϴ�.
        Vector3 centerPoint = (object1.transform.position + object2.transform.position) / 2f;

        // �� ������Ʈ�� �߽������� �̵���ŵ�ϴ�.
        object1.transform.position = centerPoint + new Vector3(-0.5f, 0f, 0f);
        object2.transform.position = centerPoint + new Vector3(0.5f, 0f, 0f);

        // ���� ������ �����մϴ�.
        connectionPoint = centerPoint;
        isConnecting = true;
    }

    public void StopConnection()
    {
        isConnecting = false;
    }

    private void Update()
    {
        // ���� ���� ��, �� ������Ʈ�� ���� ������ ���� �̵���ŵ�ϴ�.
        if (isConnecting)
        {
            Vector3 offset1 = object1.transform.position - connectionPoint;
            Vector3 offset2 = object2.transform.position - connectionPoint;
            object1.transform.position = connectionPoint + offset1;
            object2.transform.position = connectionPoint + offset2;
        }
    }
}
