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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        AttackBaseOnWeaponType();
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
            Instantiate(bulletPrefab[0], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(attackSpeed);
        }
    }

    public enum WeaponType
    {
        crossbow,
        sword,

    }
}
