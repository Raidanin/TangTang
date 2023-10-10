using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject weaponprefab;
    public WeaponType weaponType;
    public float attackSpeed = 1f;
    public float damage;
    public GameObject[] bulletPrefab;
    private bool attackInProgress = false;
    public GameObject player;
    private Scanner scanner;
    public Transform firepos;

    // Start is called before the first frame update
    void Start()
    {
        scanner = player.GetComponent<Scanner>();
    }

    // Update is called once per frame
    void Update()
    {
        AttackBaseOnWeaponType();
        LookEnemy();
    }

    private void LookEnemy()
    {
        if (scanner.closestEnemy != null)
        {
            Vector3 targetDirection = scanner.closestEnemy.transform.position - transform.position;
            targetDirection.y = 0f; // Y 축 회전 무시
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = targetRotation;
        }
    }

    void AttackBaseOnWeaponType()
    {
        switch (weaponType)
        {
            case WeaponType.crossbow:
                CrossbowAttack();
                break;



        }
    }

    void CrossbowAttack()
    {
        if (!attackInProgress)
        {
            attackInProgress = true;
            StartCoroutine(StartCrossbowAttack());
        }
    }
    IEnumerator StartCrossbowAttack()
    {
        while (true)
        {
            Instantiate(bulletPrefab[0], firepos.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(attackSpeed);
        }
    }

    public enum WeaponType
    {
        crossbow,
        sword,

    }
}
