using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour
{
    public BulletType bullettype;
    private GameObject player;
    private Scanner scanner;
    public float bulletSpeed;
    private Vector3 enemyDirection;
    private Vector3 enemyPos;

    public float disableDelay = 1f;
    private Camera mainCamera;
    public float pushForce = 10f;




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scanner = player.GetComponent<Scanner>();
        mainCamera = Camera.main;

        

        if (scanner.closestEnemy != null)
        { 
            enemyPos = new Vector3 (scanner.closestEnemy.transform.position.x, transform.position.y, scanner.closestEnemy.transform.position.z);
            enemyDirection = (enemyPos - transform.position).normalized;
        }
        else enemyDirection = player.transform.TransformDirection(Vector3.forward);

    }


    // Update is called once per frame
    void FixedUpdate()
    {

        FlyBaseBulletType();

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // ������Ʈ�� ī�޶� ������ ������
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            // 1�� �Ŀ� ��Ȱ��ȭ
            Invoke("DisableObject", disableDelay);
        }

    }

    void DisableObject()
    {
        // ������Ʈ�� ��Ȱ��ȭ
        gameObject.SetActive(false);
    }

    void FlyBaseBulletType()
    {
        switch (bullettype)
        {
            case BulletType.crossbowArrow:
                CrossBowArrowMove();

              

                break;



        }
    }

    void CrossBowArrowMove()
    {
        transform.Translate(enemyDirection * bulletSpeed * Time.deltaTime);
        
    }

    void BulletHit(Collider other)
    {
        Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();

        if (otherRigidbody != null)
        {
            // �ڷ� ���� ���� (�ڷ� ��ġ�� ȿ��)
            Vector3 pushDirection = (other.transform.position - player.transform.position).normalized;
            otherRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }

        // Enemy ��ũ��Ʈ���� OnHit �޼��带 ȣ��
        EnemyClass enemyScript = other.gameObject.GetComponent<EnemyClass>();
        if (enemyScript != null)
        {
            enemyScript.OnHit();
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        BulletHit(other);
    }

    public enum BulletType
    {
        crossbowArrow,
    }
}
