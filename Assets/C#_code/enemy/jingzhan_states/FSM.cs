using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*public enum StateType{ 
    Patrol,Death, Attack, Chase,Hit,Down
}*/

[Serializable]
public class Parameter {

    [Header("���")]
    public Rigidbody2D rigidBody2D;    //���ø���
    public Transform transform;         
    public Animator animator;          //���ö�����

    public CapsuleCollider2D capsuleCollider2D;//����2d������ײ��
    public PhysicalCheck physicalCheck;
    public enemyHp enemyHp;


    public Vector3 face_dir;        //�泯����
    [Header("�ƶ��ٶ�")]
    public float patrol_speed;      //Ѳ���ٶ�
    public float chase_speed;         //׷���ٶ�
    [Header("����ģʽ")]
    public bool gravity;   //�����ж�   
    [Header("�ܻ�")]
    public bool is_hit;            //�Ƿ񱻹���
    public bool is_invincible;    //�Ƿ������޵�
    public bool is_down;            //�Ƿ񵹵�
    public float chen_position;     //�µ�λ��
    public Vector2 hit_power;       //��������
    public float hit_y;         //���ɵ�y������
    [Header("׷��������")]
    /*public Vector2 center_offset;   //����
    public Vector2 check_siez;      //����С*/
    public float check_distance;    //������
    public LayerMask layer;         //���ͼ��
    [Header("����������")]
  /*  public Vector2 attack_offset;   //����
    public Vector2 attack_siez;      //����С
    public float attack_r;          //��ⷶΧ      */
    public float attack_distance;    //������
    public float attack_speed;      //����λ��
    [Header("�ܻ���Ч")]
    public GameObject[] effects;

}
public class FSM : MonoBehaviour
{
    private IState nowState;
    public Parameter parameter;
    //ʹ���ֵ�ע�����е�״̬
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();
    // Start is called before the first frame update
    void Start()
    {
        states.Add(StateType.Patrol , new PatrolState(this));
        states.Add(StateType.Hit, new HitState(this));
        states.Add(StateType.Death, new DeathState(this));
        states.Add(StateType.Down, new DownState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.Attack, new AttackState(this));

        SwitchState(StateType.Patrol);
    }

    private void Awake()
    {
        parameter.rigidBody2D = GetComponent<Rigidbody2D>();
        parameter.transform = GetComponent<Transform>();
        parameter.animator = GetComponent<Animator>();
        parameter.capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        parameter.enemyHp = GetComponentInChildren<enemyHp>();
        parameter.physicalCheck = GetComponent<PhysicalCheck>();


    }

    // Update is called once per frame
    void Update()
    {
        parameter.face_dir = new Vector3(-transform.localScale.x, 1, 1);    //ʵʱ����泯����
        parameter.hit_y = parameter.rigidBody2D.velocity.y;                 //ʵʱ���y������
        nowState.OnUpdate();
        
    }

    private void FixedUpdate()
    {
        nowState.OnFixUpdate();
    }

    //�л�״̬
    public void SwitchState(StateType type) {
        //��ǰ״̬����ʱ�Ƴ���ǰ״̬
        if (nowState != null) {
            nowState.OnExit();
        }
        nowState = states[type];
        //ִ���µ�״̬
        nowState.OnEnter();
    }

    //���˷�ת
    public void EnemyFlip()
    {
        if ((!parameter.physicalCheck.behind_ground && parameter.face_dir.x > 0) || (!parameter.physicalCheck.front_ground && parameter.face_dir.x < 0))
        {
            transform.localScale = new Vector3(parameter.face_dir.x, 1, 1);
        }
    }


    /*    //�������
        public bool FoundChen() {
            return Physics2D.BoxCast(parameter.transform.position + (Vector3)parameter.center_offset , parameter.check_siez , 0 , parameter.face_dir,parameter.check_distance,parameter.layer);
        }
        //�������
        public bool AttackChen()
        {
            return Physics2D.BoxCast(parameter.transform.position + (Vector3)parameter.attack_offset, parameter.attack_siez, 0, parameter.face_dir, parameter.attack_distance, parameter.layer);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position + (Vector3)parameter.center_offset + new Vector3(parameter.check_distance * -transform.localScale.x,0) , 0.2f);
            Gizmos.DrawWireSphere(transform.position + (Vector3)parameter.attack_offset + new Vector3(parameter.attack_distance * -transform.localScale.x, 0), parameter.attack_r);
        }*/
    /**
     * �ж�����
     * */
    //�������
    public bool FoundChen()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * parameter.face_dir, parameter.check_distance, parameter.layer);
        if (hit)
        {
            return true;
        }
        return false;
    }
    //�������
    public bool AttackChen()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * parameter.face_dir, parameter.attack_distance, parameter.layer);
        if (hit)
        {
            return true;
        }
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector2.right * parameter.face_dir * parameter.check_distance, Color.blue);
        Debug.DrawRay(transform.position, Vector2.right * parameter.face_dir * parameter.attack_distance, Color.yellow);
    }
    //�泯���
    public void FaceChen() {
        float info_transform_x = GameObject.Find("chen").transform.position.x;
        bool isBack = info_transform_x - parameter.transform.position.x > 0;
        if (!isBack)
        {
            parameter.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            parameter.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Move(float speed)
    {
        parameter.rigidBody2D.velocity = new Vector2(speed * parameter.face_dir.x * Time.deltaTime, parameter.rigidBody2D.velocity.y);
    }

    //����������Ϻ��Զ���������
    private void DestoryThis()
    {
        ObjectPool.Instance.PushGameObject(this.gameObject);
    }

    //�ܻ���ת
    public void HitFlip()
    {

        bool isBack = parameter.chen_position - parameter.transform.position.x > 0 ;
        if (!isBack)
        {
            parameter.transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            parameter.transform.localScale = new Vector3(-1, 1, 1);
        }
        //ͣ��
        parameter.rigidBody2D.velocity = new(0, parameter.rigidBody2D.velocity.y);
        //����
        if (parameter.gravity)
        {
            StartCoroutine(HitFly(parameter.hit_power));
        }
        else {
            Vector2 tempV = new(parameter.hit_power.x,0);
            StartCoroutine(HitFly(tempV));
        }
    }
    private IEnumerator HitFly(Vector2 v)
    {
        parameter.rigidBody2D.AddForce(v, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
    }
    //�ܻ���Ч
    public void HitEffect(int x)
    {
        GameObject temp_effect = ObjectPool.Instance.GetObject(parameter.effects[x]);
        temp_effect.transform.position = new Vector2(transform.position.x, transform.position.y);
    }
}
