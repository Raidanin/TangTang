using System.Collections;
using System.Collections.Generic;
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


    void Start()
    {
        // �÷��̾ ã�Ƽ� Ÿ������ ����
        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();
        enemyRenderer = GetComponentInChildren<Renderer>();
        originalColor = enemyRenderer.material.color;
    }

    void FixedUpdate()
    {
        // ������ ������ ���� �ٸ� �̵� �� ȸ�� ���� ȣ��
        MoveBasedOnMonsterType();
        RotateTowardsDirection();

        if(enemyHp <=0)
        {
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

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 circularDirection = Vector3.Cross(directionToTarget, Vector3.up).normalized; // XZ ��鿡���� �������� ����

        Vector3 combinedDirection = (directionToTarget + circularDirection * circleRadius).normalized;
        rb.velocity = combinedDirection * moveSpeed;
    }

    void GimpMove()
    {
        // Butcher ������ �̵� ���� ����
        // ��: �ٸ� �������� �̵�
        Vector3 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
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