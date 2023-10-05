using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] monsterPrefabs; // ���� ������ �迭
    public float spawnInterval = 2f;    // ���� ���� ����
    public float spawnRadius = 10f;     // ���� ��ġ �ݰ�
    public Transform player;            // �÷��̾��� Transform ������Ʈ
    public Enemy enemy;


    // Start is called before the first frame update
    void Start()
    {
        // ���� �� ���� �ڷ�ƾ ����
        StartCoroutine(SpawnMonster());
      
    }

    // �ڷ�ƾ���� ���� ����
    IEnumerator SpawnMonster()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // ������ ��ġ ��� (�÷��̾� �ֺ�����)
            Vector3 spawnPosition = GetRandomSpawnPosition();

            // ������ ���� ������ ����
            GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];

            // ���� ����
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity, transform);
        }
    }

    // ������ ���� ��ġ ��� (�÷��̾� �ֺ�����)
    Vector3 GetRandomSpawnPosition()
    {
        float spawnAngle = Random.Range(0f, 360f);
        Vector3 spawnDir = Quaternion.Euler(0, spawnAngle, 0) * Vector3.forward;

        // �÷��̾��� ��ġ�� �������� ���� ��ġ ����
        float spawnDistance = spawnRadius;
        Vector3 spawnPosition = player.position + spawnDir * spawnDistance;

        return spawnPosition;
    }
}