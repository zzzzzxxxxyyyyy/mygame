using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ParameterYuan
{
    [Header("���")]
    public Rigidbody2D rigidBody2D;    //���ø���
    public Transform transform;
    public Animator animator;          //���ö�����

    public PhysicalCheck physicalCheck;
    public enemyHp enemyHp;


    public Vector3 face_dir;        //�泯����
    [Header("�ƶ��ٶ�")]
    public float patrol_speed;      //Ѳ���ٶ�
    public float run_speed;         //�����ٶ�
    public float chase_speed;       //׷���ٶ�
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
    public float check_distance;    //������
    public float chase_time;        //׷��ʱ��
    public float run_time;          //����ʱ��
    public float save_distance;     //��ȫ����
    public LayerMask layer;         //���ͼ��
    [Header("����������")]
    public bool ready_atk;
    public float reload_time;           //�������
    public float temp_time;            //�м�ʱ��
    public float attack_distance;       //������
    public GameObject arrow;            //��ʸ
    [Header("�ܻ���Ч")]
    public GameObject[] effects;

}
public class FSMYuan : MonoBehaviour
{
    private IState nowState;
    public ParameterYuan parameter;
    //ʹ���ֵ�ע�����е�״̬
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();

    private void Awake()
    {
        parameter.rigidBody2D = GetComponent<Rigidbody2D>();
        parameter.transform = GetComponent<Transform>();
        parameter.animator = GetComponent<Animator>();
        parameter.enemyHp = GetComponentInChildren<enemyHp>();
        parameter.physicalCheck = GetComponent<PhysicalCheck>();


    }

    // Start is called before the first frame update
    void Start()
    {
        states.Add(StateType.Patrol, new PatrolStateYuan(this));
        states.Add(StateType.Chase, new ChaseStateYuan(this));
        states.Add(StateType.Attack, new AttackStateYuan(this));
        states.Add(StateType.Escape, new EscapeStateYuan(this));
        states.Add(StateType.Hit, new HitStateYuan(this));
        states.Add(StateType.Down, new DownStateYuan(this));
        states.Add(StateType.Death, new DeathStateYuan(this));


        SwitchState(StateType.Patrol);
    }
    void Update()
    {
        ReAtk();                                                            //�������
        parameter.face_dir = new Vector3(-transform.localScale.x, 0, 0);    //ʵʱ����泯����
        parameter.hit_y = parameter.rigidBody2D.velocity.y;                 //ʵʱ���y������
        nowState.OnUpdate();

    }

    private void FixedUpdate()
    {
        nowState.OnFixUpdate();
    }

    //�л�״̬
    public void SwitchState(StateType type)
    {
        //��ǰ״̬����ʱ�Ƴ���ǰ״̬
        if (nowState != null)
        {
            nowState.OnExit();
        }
        nowState = states[type];
        //ִ���µ�״̬
        nowState.OnEnter();
    }

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
    //��ȫ����
    public bool SavePosition()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * parameter.face_dir, parameter.save_distance, parameter.layer);
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
        Debug.DrawRay(transform.position, Vector2.right * parameter.face_dir * parameter.save_distance, Color.red);
    }

    /**
     * ���ܴ���
     * */
    //�������·�ת
    public void EnemyFlip()
    {
        if ((!parameter.physicalCheck.behind_ground && parameter.face_dir.x > 0) || (!parameter.physicalCheck.front_ground && parameter.face_dir.x < 0))
        {
            transform.localScale = new Vector3(parameter.face_dir.x, 1, 1);
        }
    }
    //������������ת
    public void EnemyFlip2() {
        transform.localScale = new Vector3(parameter.face_dir.x, 1, 1);
    }
    public void Move(float speed)
    {
        parameter.rigidBody2D.velocity = new Vector2(speed * parameter.face_dir.x * Time.deltaTime, parameter.rigidBody2D.velocity.y);
    }
    //�ܻ���ת
    public void HitFlip()
    {

        bool isBack = parameter.chen_position - parameter.transform.position.x > 0;
        if (!isBack)
        {
            parameter.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            parameter.transform.localScale = new Vector3(-1, 1, 1);
        }
        //ͣ��
        parameter.rigidBody2D.velocity = new(0, parameter.rigidBody2D.velocity.y);
        //����
        if (parameter.gravity)
        {
            StartCoroutine(HitFly(parameter.hit_power));
        }
        else
        {
            Vector2 tempV = new(parameter.hit_power.x, 0);
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


    //�泯���
    public void FaceChen()
    {
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

    //�������
    public void ShootArrowAtk()
    {
        GameObject temp_arrow = ObjectPool.Instance.GetObject(parameter.arrow);
        temp_arrow.transform.position = new Vector2(transform.position.x - 1.5f, transform.position.y + 0.2f);
        temp_arrow.GetComponent<Arrow>().ShootThis();
    }

    public void ReAtk() {
        parameter.temp_time -= Time.deltaTime;
        if (parameter.temp_time <= 0f)
        {
            parameter.ready_atk = true;
        }
    }



    //����������Ϻ��Զ���������
    private void DestoryThis()
    {
        ObjectPool.Instance.PushGameObject(this.gameObject);
    }
}
