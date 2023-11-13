using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float currentExp = 0;
    public float maxExp;
    public int level = 1;
    public float maxHp = 3;
    private float currentHp;
    private Animator anim;
    private float pullForce = 10f;
    private List<GameObject> expObjects = new List<GameObject>();

    void Start()
    {
        currentExp = 0;
        level = 1;
        currentHp = maxHp;
        anim = GetComponent<Animator>(); // 애니메이터 컴포넌트를 가져옵니다.
    }

    void Update()
    {
        Dead();
        LevelUp();
        PullExpObjects();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentHp > 0)
        {
            currentHp--;
            // 여기에 히트 애니메이션 재생 로직 추가
        }
    }

    public bool LevelUp()
    {
        if (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            level++;
            // 레벨업 이펙트 재생 로직 추가
            anim.SetTrigger("LevelUp"); // 예시: 레벨업 애니메이션 재생
            return true;
        }
        return false;
    }

    void Dead()
    {
        if (currentHp <= 0)
        {
            // 사망 애니메이션 재생 로직 추가
            anim.SetTrigger("Die"); // 예시: 사망 애니메이션 재생
            gameObject.SetActive(false);
            // Time.timeScale = 0.01f; // 게임 멈춤 로직 대신 사망 UI 표시 로직 추가
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magnet"))
        {
            // 'Exp' 태그가 붙은 모든 게임 오브젝트를 찾아 리스트에 추가합니다.
            expObjects.AddRange(GameObject.FindGameObjectsWithTag("Exp"));
            other.gameObject.SetActive(false);
        }
    }

    private void PullExpObjects()
    {
        foreach (GameObject expObject in expObjects)
        {
            // 플레이어의 위치로 오브젝트를 부드럽게 이동시킵니다.
            expObject.transform.position = Vector3.MoveTowards(expObject.transform.position, transform.position, pullForce * Time.deltaTime);
        }
    }
}