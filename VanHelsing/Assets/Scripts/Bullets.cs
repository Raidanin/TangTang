using UnityEngine;

public class Bullets : MonoBehaviour
{
    public BulletType bullettype;
    private GameObject player;
    private Scanner scanner;
    public float bulletSpeed = 10;
    private Vector3 enemyDirection;
    private Vector3 enemyPos;
    private float[] swordAngles;

    public float disableDelay = 1f;
    private Camera mainCamera;
    public float pushForce = 10f;
    private GameObject[] swords;
    public AnimationCurve noiseCurve;  // 슈리켄 노이즈를 위한 애니메이션 커브
    private float shurikenLifeTime = 0f;
    private bool hasTargetForShuriken = false;
    private GameObject initialShurikenTarget;




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scanner = player.GetComponent<Scanner>();
        mainCamera = Camera.main;

        GameObject[] swords = GameObject.FindGameObjectsWithTag("Sword");
        swordAngles = new float[swords.Length];
        for (int i = 0; i < swords.Length; i++)
        {
            swordAngles[i] = i * (360.0f / swords.Length);
        }

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

        if (bullettype == BulletType.shuriken && scanner.closestEnemy != null)
        {
            hasTargetForShuriken = true;
            initialShurikenTarget = scanner.closestEnemy;  // 초기 타겟 설정

        }
    }


    // Update is called once per frame
    void Update()
    {
        FlyBaseBulletType();

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // 오브젝트가 카메라 밖으로 나가면
        DisableBullet(viewportPosition);

        ShukikenTargetReset();

    }

    private void ShukikenTargetReset()
    {
        if (hasTargetForShuriken && scanner.closestEnemy == null)
        {
            hasTargetForShuriken = false;
        }
    }

    private void DisableBullet(Vector3 viewportPosition)
    {
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
            case BulletType.sword:
                SwordMove();
                break;
            case BulletType.shuriken:
                ShurikenMove();
                break;
        }
    }

    void CrossBowArrowMove()
    {
        transform.Translate(enemyDirection * bulletSpeed * Time.deltaTime, Space.World);
        Quaternion rotation = Quaternion.LookRotation(enemyDirection);
        transform.rotation = rotation * Quaternion.Euler(90f, 0, 0);

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

    void SwordMove()
    {
        float distanceFromPlayer = 1.5f; // 플레이어와 검 사이의 거리
        float rotationSpeed = 90.0f; // 회전 속도 (도/초)

        for (int i = 0; i < swords.Length; i++)
        {
            // 각도를 업데이트합니다.
            swordAngles[i] += rotationSpeed * Time.deltaTime;

            // 회전할 위치를 계산합니다.
            Vector3 offset = new Vector3(Mathf.Sin(swordAngles[i] * Mathf.Deg2Rad), 1, Mathf.Cos(swordAngles[i] * Mathf.Deg2Rad)) * distanceFromPlayer;

            // 검의 새 위치를 설정합니다.
            swords[i].transform.position = player.transform.position + offset;

            // 검이 플레이어를 바라보도록 회전시킵니다.
            swords[i].transform.LookAt(player.transform.position);
            swords[i].transform.localRotation *= Quaternion.Euler(-135, 0, 0);
        }
    }

    void ShurikenMove()
    {
        Vector3 finalDirection;

        if (hasTargetForShuriken)
        {
            if (initialShurikenTarget != null && initialShurikenTarget.activeInHierarchy)  // 초기 타겟이 아직 존재하고 활성화되어 있다면
            {
                Vector3 targetPosition = initialShurikenTarget.transform.position;
                Vector3 directionToTarget = (targetPosition - transform.position).normalized;

                // 애니메이션 커브를 이용하여 노이즈를 추가
                float noise = noiseCurve.Evaluate(shurikenLifeTime);
                Vector3 noiseDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                finalDirection = Vector3.Lerp(directionToTarget, noiseDirection, noise);
                finalDirection.Normalize();
            }
            else  // 초기 타겟이 사라진 경우
            {
                hasTargetForShuriken = false;  // 타겟을 잃었으므로 false로 설정
                finalDirection = enemyDirection;  // 초기 방향으로 설정
            }
        }
        else
        {
            finalDirection = enemyDirection;  // 초기 방향으로 설정
        }

        finalDirection.y = 0f;  // Y축 이동을 0으로 설정
        transform.Translate(finalDirection * bulletSpeed * Time.deltaTime, Space.World);

        // 수명 시간 업데이트
        shurikenLifeTime += Time.deltaTime;

        Quaternion yRotation = Quaternion.AngleAxis(shurikenLifeTime * 360, Vector3.up);
        Quaternion zRotation = Quaternion.Euler(0, 0, -90);  // Z축으로 -90도 회전
        transform.rotation = yRotation * zRotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        BulletHit(other);
    }

    public void UpdateSwords()
    {
        swords = GameObject.FindGameObjectsWithTag("Sword");
        swordAngles = new float[swords.Length];
        for (int i = 0; i < swords.Length; i++)
        {
            swordAngles[i] = i * (360.0f / swords.Length);
        }
    }
    public enum BulletType
    {
        crossbowArrow,
        sword,
        shuriken,
    }
}
