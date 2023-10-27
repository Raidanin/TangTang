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
    public AnimationCurve noiseCurve;  // ������ ����� ���� �ִϸ��̼� Ŀ��
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
            enemyPos = new Vector3(scanner.closestEnemy.transform.position.x, transform.position.y, scanner.closestEnemy.transform.position.z); // y��ǥ�� ȭ���� y��ǥ�� ����
            enemyDirection = (enemyPos - transform.position).normalized;
        }
        else
        {
            enemyDirection = player.transform.forward;
            enemyDirection.y = 0f; // y���� �������� �̵�
        }

        if (bullettype == BulletType.shuriken && scanner.closestEnemy != null)
        {
            hasTargetForShuriken = true;
            initialShurikenTarget = scanner.closestEnemy;  // �ʱ� Ÿ�� ����

        }
    }


    // Update is called once per frame
    void Update()
    {
        FlyBaseBulletType();

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // ������Ʈ�� ī�޶� ������ ������
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

    void SwordMove()
    {
        float distanceFromPlayer = 1.5f; // �÷��̾�� �� ������ �Ÿ�
        float rotationSpeed = 90.0f; // ȸ�� �ӵ� (��/��)

        for (int i = 0; i < swords.Length; i++)
        {
            // ������ ������Ʈ�մϴ�.
            swordAngles[i] += rotationSpeed * Time.deltaTime;

            // ȸ���� ��ġ�� ����մϴ�.
            Vector3 offset = new Vector3(Mathf.Sin(swordAngles[i] * Mathf.Deg2Rad), 1, Mathf.Cos(swordAngles[i] * Mathf.Deg2Rad)) * distanceFromPlayer;

            // ���� �� ��ġ�� �����մϴ�.
            swords[i].transform.position = player.transform.position + offset;

            // ���� �÷��̾ �ٶ󺸵��� ȸ����ŵ�ϴ�.
            swords[i].transform.LookAt(player.transform.position);
            swords[i].transform.localRotation *= Quaternion.Euler(-135, 0, 0);
        }
    }

    void ShurikenMove()
    {
        Vector3 finalDirection;

        if (hasTargetForShuriken)
        {
            if (initialShurikenTarget != null && initialShurikenTarget.activeInHierarchy)  // �ʱ� Ÿ���� ���� �����ϰ� Ȱ��ȭ�Ǿ� �ִٸ�
            {
                Vector3 targetPosition = initialShurikenTarget.transform.position;
                Vector3 directionToTarget = (targetPosition - transform.position).normalized;

                // �ִϸ��̼� Ŀ�긦 �̿��Ͽ� ����� �߰�
                float noise = noiseCurve.Evaluate(shurikenLifeTime);
                Vector3 noiseDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                finalDirection = Vector3.Lerp(directionToTarget, noiseDirection, noise);
                finalDirection.Normalize();
            }
            else  // �ʱ� Ÿ���� ����� ���
            {
                hasTargetForShuriken = false;  // Ÿ���� �Ҿ����Ƿ� false�� ����
                finalDirection = enemyDirection;  // �ʱ� �������� ����
            }
        }
        else
        {
            finalDirection = enemyDirection;  // �ʱ� �������� ����
        }

        finalDirection.y = 0f;  // Y�� �̵��� 0���� ����
        transform.Translate(finalDirection * bulletSpeed * Time.deltaTime, Space.World);

        // ���� �ð� ������Ʈ
        shurikenLifeTime += Time.deltaTime;

        Quaternion yRotation = Quaternion.AngleAxis(shurikenLifeTime * 360, Vector3.up);
        Quaternion zRotation = Quaternion.Euler(0, 0, -90);  // Z������ -90�� ȸ��
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
