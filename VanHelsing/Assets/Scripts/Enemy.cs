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
        RotateTowardsPlayer();
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
        Vector3 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    void ButcherMove()
    {
        // Butcher ������ �̵� ���� ����
        // ��: �ٸ� �������� �̵�
        Vector3 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed * 0.5f; // Type2�� ������ �̵�
    }

    void RotateTowardsPlayer()
    {
        if (target != null)
        {
            // �÷��̾ �ٶ󺸵��� ȸ��
            transform.LookAt(target);
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