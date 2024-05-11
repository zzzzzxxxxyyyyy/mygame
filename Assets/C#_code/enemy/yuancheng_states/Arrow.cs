using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    //public GameObject arrow_break;
    public float damage;
    public float hit_power2;
    public float speed;
    public float break_time;                //��ʧʱ��
    public GameObject arrow_break;

    new public Rigidbody2D rigidbody;
    public Transform this_position;         //��ǰλ��
    public Vector2 chen_position;           //�£���ң���λ��
    public Vector2 dir_position;          //���䷽��
    
    private float timer;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        this_position = transform.Find("xxl_y");
    }

    private void OnEnable()
    {
        //������λ�µ�λ��
        timer = break_time;
        chen_position = GameObject.Find("chen").transform.position;
        ShootThis();
    }

    // Update is called once per frame
    void Update()
    {
        DestroyThis();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!collision.GetComponent<chenPlayer>().spurt_status)
            {
                collision.GetComponent<ChenUI>().ReduceHp(damage);
                collision.GetComponent<chenPlayer>().HitByEnemy(transform, hit_power2);
                GameObject temp_object = ObjectPool.Instance.GetObject(arrow_break);
                temp_object.transform.position = transform.position;
                temp_object.GetComponent<ArrowBreak>().vector = dir_position;
                ObjectPool.Instance.PushGameObject(this.gameObject);
            }
        }
        if (collision.CompareTag("is_atk"))
        {
            ObjectPool.Instance.PushGameObject(this.gameObject);
        }
    }

    public void ShootThis()
    {
        //���䷽��
        dir_position = (chen_position - new Vector2(transform.position.x, transform.position.y - 2.4f)).normalized;
        transform.right = dir_position;
        rigidbody.velocity = dir_position * speed;
    }

    public void DestroyThis() {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ObjectPool.Instance.PushGameObject(this.gameObject);
        }
    }
}
