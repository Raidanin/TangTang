using UnityEngine;

public class Player : MonoBehaviour
{
    public float curruntExp = 0;
    public float maxExp;
    public int level = 1;
    public float maxHp = 3;
    private float curruntHp;
    private Animator annim;



    // Start is called before the first frame update
    void Start()
    {
        curruntExp = 0;
        level = 1;
        curruntHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        Dead();
        LevelUp();
    }


    private void OnCollisionEnter(Collision collision)
    {

        curruntHp--;
    }

    public bool LevelUp()
    {
        if (curruntExp >= maxExp)
        {
            curruntExp = curruntExp - maxExp;
            level++;
            //������ ����Ʈ ���
            return true;
        }
        return false;
    }
    void Dead()
    {

        if (curruntHp <= 0)
        {
            //��� �ִϸ��̼� ���
            gameObject.SetActive(false);

            Time.timeScale = 0.01f;
        }
    }

}
