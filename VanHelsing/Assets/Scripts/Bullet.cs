using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour
{
    public BulletType bullettype;
    private GameObject player;
    private Scanner scanner;
    public float bulletSpeed;
    private Rigidbody rb;
    private Vector3 enemyDirection;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scanner = player.GetComponent<Scanner>();
        rb = GetComponent<Rigidbody>();
        if (scanner.closestEnemy != null)
            enemyDirection = (scanner.closestEnemy.transform.position - transform.position).normalized;
        else enemyDirection = player.transform.TransformDirection(Vector3.forward);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FlyBaseBulletType();
    }

    void FlyBaseBulletType()
    {
        switch (bullettype)
        {
            case BulletType.crossbowArrow:
                CrossBowArrow();
                break;



        }
    }

    void CrossBowArrow()
    {
            rb.velocity = enemyDirection * bulletSpeed; 
    }


    public enum BulletType
    {
        crossbowArrow,
    }
}
