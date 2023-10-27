using System;
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
    private bool isSwordAttack = false;  // �÷��� �߰�
    private int swordCount = 3;
    public GameObject[] swords;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scanner = player.GetComponent<Scanner>();

     
    }

    void Update()
    {
        AttackBaseOnWeaponType();

            int playerLevel = player.GetComponent<Player>().level;
            attackSpeed = Mathf.Max(0.2f, 1f - (playerLevel * 0.05f));
        
        if (player.GetComponent<Player>().LevelUp())
        {
            AddSword();
        }
    }

    void AddSword()
    {
        GameObject newSword = Instantiate(bulletPrefab[0], player.transform.position, Quaternion.identity);
        Array.Resize(ref swords, swords.Length + 1);
        swords[swords.Length - 1] = newSword;

        // Bullets ��ũ��Ʈ�� �� ������ ������Ʈ
        Bullets bulletsScript = FindObjectOfType<Bullets>();
        bulletsScript.UpdateSwords();
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
        if (!isSwordAttack)
        {
            swords = new GameObject[swordCount];

            for (int i = 0; i < swordCount; i++)
            {
                swords[i] = Instantiate(bulletPrefab[0], player.transform.position, Quaternion.identity);
                Bullets bulletsScript = FindObjectOfType<Bullets>();
                bulletsScript.UpdateSwords();
            }

            isSwordAttack = true;
        }
    }

    IEnumerator StartCrossbowAttack()
    {
        isCrossbowCoroutineRunning = true;  // �÷��� ����

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