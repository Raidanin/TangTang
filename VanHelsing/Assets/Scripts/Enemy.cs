using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public Transform target;
    private Rigidbody rb;
    public float moveSpeed;
    public MonsterType monsterType; // 몬스터의 종류

    void Start()
    {
        // 플레이어를 찾아서 타겟으로 설정
        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 몬스터의 종류에 따라 다른 이동 및 회전 로직 호출
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

            // 다른 몬스터 종류에 대한 처리 추가 가능

            default:
                break;
        }
    }

    void YokaiMove()
    {
        // Yokai 몬스터의 이동 로직 구현
        // 예: 특정한 패턴으로 이동
        // 현재 방향
        Vector3 currentDirection = transform.forward;

        // 타겟 방향
        Vector3 targetDirection = (target.position - transform.position).normalized;

        // 회전 각도 계산
        float angle = Vector3.SignedAngle(currentDirection, targetDirection, Vector3.up);

        // 최대 회전 속도 설정 (원하는 값으로 조절)
        float maxRotationSpeed = 90f;

        // 실제로 회전할 각도 계산
        float rotationStep = maxRotationSpeed * Time.deltaTime;
        float rotationToApply = Mathf.Clamp(angle, -rotationStep, rotationStep);

        // 회전 적용
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationToApply);

        // 이동
        rb.velocity = transform.forward * moveSpeed;
    }

    void ButcherMove()
    {
        // Butcher 몬스터의 이동 로직 구현
        // 예: 다른 패턴으로 이동
        Vector3 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed * 0.5f; // Type2는 느리게 이동
    }

    void RotateTowardsPlayer()
    {
        if (target != null)
        {
            // 플레이어를 바라보도록 회전
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
        // 추가적인 몬스터 종류 정의 가능
    }
}