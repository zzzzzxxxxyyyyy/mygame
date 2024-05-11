using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBomb : MonoBehaviour
{
    public GameObject boom;

    public float speed;
    new public Rigidbody2D rigidbody;
    public Transform this_position;         //��ǰλ��
    public Vector2 chen_position;           //�£���ң���λ��
    //public Vector2 mou_position;            //����
    public Vector2 dir_position;          //���䷽��
    public Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        this_position = transform.Find("wurenji");
    }

    private void OnEnable()
    {
        //������λ�µ�λ��
        chen_position = GameObject.Find("chen").transform.position;
        animator.SetBool("is_shoot", true);
        ShootThis();
    }

    // Update is called once per frame
    void Update()
    {
       //mou_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void ShootThis() {
        //���䷽��
        dir_position = (chen_position - new Vector2(transform.position.x,transform.position.y)).normalized;
        transform.right = dir_position;
        rigidbody.velocity = dir_position * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject temp_boom = ObjectPool.Instance.GetObject(boom);
        temp_boom.transform.position = transform.position;
        animator.SetBool("is_shoot", false);
        ObjectPool.Instance.PushGameObject(this.gameObject);
    }
}
