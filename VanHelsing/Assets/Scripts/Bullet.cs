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
            enemyPos = new Vector3(scanner.closestEnemy.transform.position.x, transform.position.y, scanner.closestEnemy.transform.position.z); // y좌표는 화살의 y좌표로 설정
            enemyDirection = (enemyPos - transform.position).normalized;
        }
        else
        {
            enemyDirection = player.transform.forward;
            enemyDirection.y = 0f; // y축은 수평으로 이동
        }

    }


    // Update is called once per frame
    void FixedUpdate()
    {

        FlyBaseBulletType();

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // 오브젝트가 카메라 밖으로 나가면
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            // 1초 후에 비활성화
            Invoke("DisableObject", disableDelay);
        }

    

    }

    void DisableObject()
    {
        // 오브젝트를 비활성화
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
        transform.Translate(enemyDirection * bulletSpeed * Time.deltaTime, Space.World);
        Quaternion rotation = Quaternion.LookRotation(enemyDirection);
        transform.rotation = rotation * Quaternion.Euler(90f,0,0);


    }

    void BulletHit(Collider other)
    {
        Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();

        if (otherRigidbody != null)
        {
            // 뒤로 힘을 가함 (뒤로 밀치는 효과)
            Vector3 pushDirection = (other.transform.position - player.transform.position).normalized;
            otherRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }

        // Enemy 스크립트에서 OnHit 메서드를 호출
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
