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
        // ���� ����
        Vector3 currentDirection = transform.forward;

        // Ÿ�� ����
        Vector3 targetDirection = (target.position - transform.position).normalized;

        // ȸ�� ���� ���
        float angle = Vector3.SignedAngle(currentDirection, targetDirection, Vector3.up);

        // �ִ� ȸ�� �ӵ� ���� (���ϴ� ������ ����)
        float maxRotationSpeed = 90f;

        // ������ ȸ���� ���� ���
        float rotationStep = maxRotationSpeed * Time.deltaTime;
        float rotationToApply = Mathf.Clamp(angle, -rotationStep, rotationStep);

        // ȸ�� ����
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationToApply);

        // �̵�
        rb.velocity = transform.forward * moveSpeed;
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