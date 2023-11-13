using UnityEngine;

public class Exp : MonoBehaviour
{
    public float expValue = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(Vector3.up, 90f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
            GameObject thisGameObject = this.gameObject;
            GameObject otherGameObject = other.gameObject;
        if (other.CompareTag("Exp") && this.gameObject.activeInHierarchy)
        {

            Exp otherExp = other.GetComponent<Exp>();

            // expValue ���ϱ�
            expValue += otherExp.expValue;

            // �ڽ��� ���� Ȱ�� ���¶�� �ٸ� ��ü�� ��Ȱ��ȭ
            otherGameObject.SetActive(false);
        }

        if(other.CompareTag("Player"))
        {
            other.GetComponent<Player>().currentExp += expValue;
            thisGameObject.SetActive(false);   
        }
    }
}
 