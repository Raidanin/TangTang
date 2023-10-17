using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject weaponprefab;
    public WeaponType weaponType;
    public float attackSpeed = 1f;
    public float damage;
    public GameObject[] bulletPrefab;
    public GameObject player;
    private Scanner scanner;
    public Transform firepos;
    public int weaponLevel = 1;
    private bool isCrossbowCoroutineRunning = false;  // �÷��� �߰�

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scanner = player.GetComponent<Scanner>();
    }

    void Update()
    {
        AttackBaseOnWeaponType();
    }

    private void LookEnemy()
    {
        if (scanner.closestEnemy != null)
        {
            Vector3 targetDirection = scanner.closestEnemy.transform.position - transform.position;
            targetDirection.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = targetRotation * Quaternion.Euler(-90f, 180f, 0);
        }
    }

    void AttackBaseOnWeaponType()
    {
        switch (weaponType)
        {
            case WeaponType.crossbow:
                CrossbowAttack();
                LookEnemy();
                break;
            case WeaponType.sword:
                SwordAttack();
                break;
        }
    }

    void CrossbowAttack()
    {
        if (!isCrossbowCoroutineRunning)  // �÷��� üũ
        {
            StartCoroutine(StartCrossbowAttack());
        }
    }

    void SwordAttack()
    {
        int swordCount = 3;
    }

    IEnumerator StartCrossbowAttack()
    {
        isCrossbowCoroutineRunning = true;  // �÷��� ����

        while (true)
        {
            Instantiate(bulletPrefab[0], firepos.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1);
        }

        // �ڷ�ƾ�� ��� ������ ����Ǹ� �÷��׸� false�� ����
        // isCrossbowCoroutineRunning = false;  // �� �ڵ�� ���� ���� ���� ������ ������ �� �����ϴ�.
    }

    public enum WeaponType
    {
        crossbow,
        sword,
    }
}