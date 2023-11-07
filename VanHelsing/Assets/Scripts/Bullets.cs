using System.Collections;
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
    public AnimationCurve positionCurve; // 총알의 위치 변화를 위한 애니메이션 커브
    public AnimationCurve noiseCurve; // 총알의 노이즈 변화를 위한 애니메이션 커브
    public float yOffset = 1f; // 목표 지점의 y축 오프셋
    public Vector2 minNoise = new Vector2(-3f, -0.25f); // 노이즈 최소 범위
    public Vector2 maxNoise = new Vector2(3f, 1f); // 노이즈 최대 범위

    private Coroutine homingCoroutine;
    private GameObject initialShurikenTarget;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scanner = player.GetComponent<Scanner>();
        mainCamera = Camera.main;
        swords = GameObject.FindGameObjectsWithTag("Sword");
        swordAngles = new float[swords.Length];

        for (int i = 0; i < swords.Length; i++)
        {
            swordAngles[i] = i * (360.0f / swords.Length);
        }

        if (scanner.closestEnemy != null)
        {
            enemyPos = new Vector3(scanner.closestEnemy.transform.position.x, transform.position.y, scanner.closestEnemy.transform.position.z);
            enemyDirection = (enemyPos - transform.position).normalized;
            if (bullettype == BulletType.shuriken)
            {
                initialShurikenTarget = scanner.closestEnemy;
                if (homingCoroutine != null)
                {
                    StopCoroutine(homingCoroutine);
                }
                homingCoroutine = StartCoroutine(ShurikenMove());
            }
        }
        else
        {
            enemyDirection = player.transform.forward;
            enemyDirection.y = 0f; // y축은 수평으로 이동
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (bullettype != BulletType.shuriken)
        {
            FlyBaseBulletType();
        }
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        DisableBullet(viewportPosition);
    }

    private void DisableBullet(Vector3 viewportPosition)
    {
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            Invoke("DisableObject", disableDelay);
        }
    }
    void DisableObject()
    {
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
                // ShurikenMove() 호출 부분 삭제
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


    IEnumerator ShurikenMove()
    {
        Vector3 startPosition = transform.position;
        float time = 0f;

        while (time < 1f && initialShurikenTarget != null)
        {
            Vector3 targetPosition = initialShurikenTarget.transform.position + new Vector3(0, yOffset, 0);
            Vector3 direction = (targetPosition - startPosition).normalized;
            Vector3 horizontalNoiseDirection = Vector3.Cross(direction, Vector3.up).normalized;

            Vector2 noise = new Vector2(
                Random.Range(minNoise.x, maxNoise.x),
                Random.Range(minNoise.y, maxNoise.y)
            );

            float noisePosition = noiseCurve.Evaluate(time);
            Vector3 noiseVector = new Vector3(
                horizontalNoiseDirection.x * noisePosition * noise.x,
                0, // y축 노이즈 제거
                horizontalNoiseDirection.z * noisePosition * noise.x
            );

            transform.position = Vector3.Lerp(startPosition, targetPosition, positionCurve.Evaluate(time)) + noiseVector;
            transform.LookAt(targetPosition);

            time += Time.deltaTime * bulletSpeed;

            yield return null;
        }

        // 적이 없어지면 총알을 비활성화합니다.
        if (initialShurikenTarget == null)
        {
            DisableObject();
        }
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
