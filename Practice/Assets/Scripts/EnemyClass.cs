using System.Collections;
using UnityEngine;
public class EnemyClass : MonoBehaviour
{
    public Transform target;
    private Rigidbody rb;
    public float moveSpeed;
    public MonsterType monsterType; // ������ ����
    private Color originalColor;
    private Renderer enemyRenderer;
    private Coroutine revertColorCoroutine; // �ڷ�ƾ�� �����ϱ� ���� ����
    private float enemyHp = 3;
    public GameObject expPrefab;
    public GameObject hitEffectPrefab;
    public GameObject magnet;


    void Start()
    {
        // �÷��̾ ã�Ƽ� Ÿ������ ����

        GameObject playerObj = GameObject.Find("Player");

        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        else
        {
            Debug.Log("Player GameObject not found!");
        }
        rb = GetComponent<Rigidbody>();
        enemyRenderer = GetComponentInChildren<Renderer>();
        originalColor = enemyRenderer.material.color;
    }

    void FixedUpdate()
    {
        // ������ ������ ���� �ٸ� �̵� �� ȸ�� ���� ȣ��
        MoveBasedOnMonsterType();
        RotateTowardsDirection();

        if (enemyHp <= 0)
        {
            Vector3 expPos = new Vector3(transform.position.x, 0.5f, transform.position.z);
            Instantiate(expPrefab, expPos, Quaternion.identity);

            int chance = Random.Range(0, 100);
            if (chance < 3)
            {
                Instantiate(magnet, expPos, Quaternion.identity);
            }
            gameObject.SetActive(false);
        }
    }

    void MoveBasedOnMonsterType()
    {
        switch (monsterType)
        {
            case MonsterType.Yokai:
                YokaiMove();
                break;

            case MonsterType.Gimp:
                GimpMove();
                break;

            // �ٸ� ���� ������ ���� ó�� �߰� ����

            default:
                break;
        }
    }
    public void OnHit()
    {
        if (revertColorCoroutine != null)
        {
            StopCoroutine(revertColorCoroutine); // ���� ���� ���� �ڷ�ƾ�� ����
        }
        enemyRenderer.material.color = Color.red; // ������ ���������� ��������� ����
        revertColorCoroutine = StartCoroutine(RevertColor()); // �ڷ�ƾ�� �ٽ� ����

        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
        }

        enemyHp--;
    }
    // ������ ������� ������ �ڷ�ƾ �޼���
    IEnumerator RevertColor()
    {
        float elapsed = 0f;
        float duration = 0.2f;
        Color currentColor = Color.red; // ���� ������ ���������� ����

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            enemyRenderer.material.color = Color.Lerp(currentColor, originalColor, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        enemyRenderer.material.color = originalColor;
    }
    void YokaiMove()
    {
        // Yokai ������ �̵� ���� ����
        float circleRadius = 1.5f; // ���� ũ�⸦ �����Ͽ� ���� ����� ������ ����
        if (target != null)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            Vector3 circularDirection = Vector3.Cross(directionToTarget, Vector3.up).normalized; // XZ ��鿡���� �������� ����

            Vector3 combinedDirection = (directionToTarget + circularDirection * circleRadius).normalized;
            rb.velocity = combinedDirection * moveSpeed;
        }
        else
        {
            Debug.Log("Player GameObject not found!");
        }

    }

    void GimpMove()
    {
        // Butcher ������ �̵� ���� ����
        // ��: �ٸ� �������� �̵�
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            Debug.Log("Player GameObject not found!");
        }


    }

    void RotateTowardsDirection()
    {
        if (rb.velocity != Vector3.zero)
        {
            // Look at the direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(rb.velocity);
            transform.rotation = targetRotation;
        }
    }

    public enum MonsterType
    {
        Yokai,
        Gimp,
        Type3,
        Type4,
        Type5,
        // �߰����� ���� ���� ���� ����
    }
}