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
    private bool isCrossbowCoroutineRunning = false;  // 플래그 추가

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
        if (!isCrossbowCoroutineRunning)  // 플래그 체크
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
        isCrossbowCoroutineRunning = true;  // 플래그 설정

        while (true)
        {
            Instantiate(bulletPrefab[0], firepos.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1);
        }

        // 코루틴이 어떠한 이유로 종료되면 플래그를 false로 설정
        // isCrossbowCoroutineRunning = false;  // 이 코드는 현재 무한 루프 때문에 도달할 수 없습니다.
    }

    public enum WeaponType
    {
        crossbow,
        sword,
    }
}