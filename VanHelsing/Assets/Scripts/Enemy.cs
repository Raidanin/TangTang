using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public Transform target;
    private Rigidbody rb;
    public float moveSpeed;
    public MonsterType monsterType; // ������ ����

    void Start()
    {
        // �÷��̾ ã�Ƽ� Ÿ������ ����
        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // ������ ������ ���� �ٸ� �̵� �� ȸ�� ���� ȣ��
        MoveBasedOnMonsterType();
        RotateTowardsDirection();
    }

    void MoveBasedOnMonsterType()
    {
        switch (monsterType)
        {
            case MonsterType.Yokai:
                YokaiMove();
                break;

            case MonsterType.Butcher:
                ButcherMove();
                break;

            // �ٸ� ���� ������ ���� ó�� �߰� ����

            default:
                break;
        }
    }

    void YokaiMove()
    {
        // Yokai ������ �̵� ���� ����
        // ��: Ư���� �������� �̵�
        float circleRadius = 2f; // Adjust this value to control the size of the circle

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 circularDirection = Vector3.Cross(directionToTarget, Vector3.up).normalized; // Assuming movement in the XZ plane

        Vector3 combinedDirection = (directionToTarget + circularDirection * circleRadius).normalized;
        rb.velocity = combinedDirection * moveSpeed;
    }

    void ButcherMove()
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
        Butcher,
        Type3,
        Type4,
        Type5,
        // �߰����� ���� ���� ���� ����
    }
}