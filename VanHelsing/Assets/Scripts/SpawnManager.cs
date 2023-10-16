using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] monsterPrefabs; // ���� ������ �迭
    public float spawnInterval = 2f;    // ���� ���� ����
    public float spawnRadius = 10f;     // ���� ��ġ �ݰ�
    public Transform player;            // �÷��̾��� Transform ������Ʈ
    


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
        // �÷��̾ �ٶ󺸴� ���⿡ ������ ������ �߰� (��: -30������ +30�� ����)
        float randomAngleOffset = Random.Range(-60f, 60f);
        Vector3 playerForward = player.forward;
        Quaternion spawnRotation = Quaternion.Euler(0, randomAngleOffset, 0);
        Vector3 spawnDir = spawnRotation * playerForward;

        // �÷��̾��� ��ġ�� �������� ���� ��ġ ����
        float spawnDistance = spawnRadius;
        Vector3 spawnPosition = player.position + spawnDir * spawnDistance;

        return spawnPosition;
    }
}