using System;
using System.Collections;
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
    public bool isLevelUping = false;
    public GameObject levelUpParticle;
    private Coroutine levelUpEffectCoroutine;
    public event Action LevelUpEvent;


    void Start()
    {
        currentExp = 0;
        level = 1;
        currentHp = maxHp;
        anim = GetComponent<Animator>(); // �ִϸ����� ������Ʈ�� �����ɴϴ�.
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
            // ���⿡ ��Ʈ �ִϸ��̼� ��� ���� �߰�
        }
    }

    public void LevelUp()
    {
        if (currentExp >= maxExp)
        {
            isLevelUping = true;
            currentExp -= maxExp;
            level++;
            LevelUpEvent?.Invoke();
        }
    }


    public void PlayLevelUpEffect()
    {
        if (levelUpEffectCoroutine != null)
        {
            StopCoroutine(levelUpEffectCoroutine); // �̹� ���� ���� �ڷ�ƾ�� �ִٸ� �ߴ�
        }
        levelUpEffectCoroutine = StartCoroutine(ActivateLevelUpEffect());
    }

    private IEnumerator ActivateLevelUpEffect()
    {
        levelUpParticle.SetActive(true); // ����Ʈ Ȱ��ȭ
        yield return new WaitForSeconds(1); // 1�� ��ٸ�
        levelUpParticle.SetActive(false); // ����Ʈ ��Ȱ��ȭ
    }


    void Dead()
    {
        if (currentHp <= 0)
        {
            // ��� �ִϸ��̼� ��� ���� �߰�
            anim.SetTrigger("Die"); // ����: ��� �ִϸ��̼� ���
            gameObject.SetActive(false);
            // Time.timeScale = 0.01f; // ���� ���� ���� ��� ��� UI ǥ�� ���� �߰�
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magnet"))
        {
            // 'Exp' �±װ� ���� ��� ���� ������Ʈ�� ã�� ����Ʈ�� �߰��մϴ�.
            expObjects.AddRange(GameObject.FindGameObjectsWithTag("Exp"));
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Exp"))
        {
            expObjects.Remove(other.gameObject);

            currentExp += other.GetComponent<Exp>().expValue;
            other.gameObject.SetActive(false);
        }
    }

    private void PullExpObjects()
    {
        foreach (GameObject expObject in expObjects)
        {
            // �÷��̾��� ��ġ�� ������Ʈ�� �ε巴�� �̵���ŵ�ϴ�.
            expObject.transform.position = Vector3.MoveTowards(expObject.transform.position, transform.position, pullForce * Time.deltaTime);
        }
    }
}