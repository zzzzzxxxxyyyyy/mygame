using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Collections;

[Serializable]
public class ParameterWu
{
    [Header("���")]
    public Rigidbody2D rigidBody2D;    //���ø���
    public Seeker seeker;
    public Transform transform;
    public Animator animator;          //���ö�����

    public CapsuleCollider2D capsuleCollider2D;//����2d������ײ��
    public PhysicalCheck physicalCheck;
    public enemyHp enemyHp;


    public Vector3 face_dir;        //�泯����
    [Header("�ƶ��ٶ�")]
    public float patrol_speed;      //Ѳ���ٶ�
    public float reload_speed;  //���»غ��ٶ�
    public float max_up;        //���߶�
    public LayerMask groud_layer;       //������  
    public float up_speed;      //�����ٶ�
    public float pat_time;          //Ѳ��ʱ��
    public float idle_time;         //�ȴ�ʱ��
    [Header("�ܻ�")]
    public bool is_hit;            //�Ƿ񱻹���
    public bool is_invincible;    //�Ƿ������޵�
    public bool is_down;
    public float chen_position;     //�µ�λ��
    public Vector2 hit_power;       //��������
    public float hit_y;         //�����ɵ�y������
    [Header("����������")]
    public LayerMask layer;         //���ͼ��
    public Vector2 attack_offset;   //����
    public Vector2 attack_siez;      //����С
    public float attack_distance;    //������
    public float attack_r;          //��ⷶΧ      
    public bool is_boom = true ;        //�Ǵ���ը��
    [Header("����װ��")]
    public Transform BoomBox;   //װ��Ŀ��
    public float next_postion;  //������һ��Ŀ���Զִ���ƶ�
    public Path path;                  //��ǰ·��
    public int current_way = 0;        //Ŀ��·��
    public bool end_path = false;       //�յ�Ŀ�� 
    public GameObject bomb;
    [Header("�ܻ���Ч")]
    public GameObject[] effects;
}
public class FSMBoomWu : MonoBehaviour
{
    private IState nowState;
    public ParameterWu parameter;
    //ʹ���ֵ�ע�����е�״̬
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();


    private void Awake()
    {
        parameter.rigidBody2D = GetComponent<Rigidbody2D>();
        parameter.transform = GetComponent<Transform>();
        parameter.animator = GetComponent<Animator>();
        parameter.seeker = GetComponent<Seeker>();
        parameter.enemyHp = GetComponentInChildren<enemyHp>();
        parameter.physicalCheck = GetComponent<PhysicalCheck>();


    }

    // Start is called before the first frame update
    void Start()
    {
        states.Add(StateType.Idle, new IdleStateWu(this));
        states.Add(StateType.Patrol, new PatrolStateWu(this));
        states.Add(StateType.Load, new LoadState(this));
        states.Add(StateType.Attack, new AttackStateWu(this));
        states.Add(StateType.Hit, new HitStateWu(this));
        states.Add(StateType.Death, new DeathStateWu(this));

        SwitchState(StateType.Patrol);

        InvokeRepeating("UpdatePath", 0f,0.5f);
    }



    // Update is called once per frame
    void Update()
    {
        parameter.face_dir = new Vector3(-transform.localScale.x, 0, 0);    //ʵʱ����泯����
        nowState.OnUpdate();
    }

    private void FixedUpdate()
    {
        nowState.OnFixUpdate();
    }


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

    public void OnPathComplete(Path p) {
        if (!p.error) {
            parameter.path = p;
            parameter.current_way = 0;
        }
    }
    //�������
    public bool AttackChen()
    {
        return Physics2D.BoxCast(parameter.transform.position + (Vector3)parameter.attack_offset, parameter.attack_siez, 0, parameter.face_dir, parameter.attack_distance, parameter.layer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)parameter.attack_offset + new Vector3(parameter.attack_distance * -transform.localScale.x, 0), parameter.attack_r);
        Debug.DrawRay(transform.position, -Vector2.up * parameter.max_up, Color.red);
    }

    //Ѱ·
    public void BlackBox(float speed) {
        //�ж��Ƿ���·��
        if (parameter.path == null) {
            return;
        }
        //�ж��Ƿ�ﵽ�յ�·��
        if (parameter.current_way >= parameter.path.vectorPath.Count)
        {
            parameter.end_path = true;
            return;
        }
        else {
            parameter.end_path = false;
        }
        //��һ��·���㷽��
        Vector2 v_dir = ((Vector2)parameter.path.vectorPath[parameter.current_way] - parameter.rigidBody2D.position).normalized;
        Vector2 force = v_dir * speed * Time.deltaTime;
        parameter.rigidBody2D.AddForce(force);

        float distance = Vector2.Distance(parameter.rigidBody2D.position, parameter.path.vectorPath[parameter.current_way]);
        if (distance < parameter.next_postion) {
            parameter.current_way++;
        }
        //Ѱ·ת��
        if (parameter.rigidBody2D.velocity.x > 0.1f) {
            parameter.transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (parameter.rigidBody2D.velocity.x < 0.1f) {
            parameter.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    //����·��
    public void UpdatePath() {
        if (parameter.seeker.IsDone()) {
            parameter.seeker.StartPath(parameter.rigidBody2D.position, parameter.BoomBox.position, OnPathComplete);
        } 
    }
    //���䵼��
    public void ShootBomb() {
        GameObject temp_bomb = ObjectPool.Instance.GetObject(parameter.bomb);
        temp_bomb.transform.position = new Vector2(transform.position.x - 0.133f, transform.position.y - 0.722f);
        temp_bomb.GetComponent<ShootBomb>().ShootThis();
    }
    //�ƶ�
    public void Move(float speed)
    {
        parameter.rigidBody2D.velocity = new Vector2(speed * parameter.face_dir.x * Time.deltaTime, parameter.rigidBody2D.velocity.y);
    }
    //����
    public void UpMove(float speed) {
        parameter.rigidBody2D.velocity = new Vector2(parameter.rigidBody2D.velocity.x, speed * Time.deltaTime);
    }
    //�Ƿ���������ߴ�
    public bool is_up() { 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up,parameter.max_up, parameter.groud_layer);
        if (hit) {
            return true;
        }
        return false;
    }
    //���˷�ת
    public void EnemyFlip()
    {
        transform.localScale = new Vector3(parameter.face_dir.x, 1, 1);
    }
    //�ı�����
    public void SetGravity(float param) {
        parameter.rigidBody2D.gravityScale = param;
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
        StartCoroutine(HitFly(new Vector2(parameter.hit_power.x * 5 ,parameter.hit_power.y)));
    }
    private IEnumerator HitFly(Vector2 v)
    {
        parameter.rigidBody2D.AddForce(v, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
    }
    //�����Żض������
    public void DestroyThis() {
        ObjectPool.Instance.PushGameObject(this.gameObject);
    }

    //�ܻ���Ч
    public void HitEffect(int x)
    {
        GameObject temp_effect = ObjectPool.Instance.GetObject(parameter.effects[x]);
        temp_effect.transform.position = new Vector2(transform.position.x, transform.position.y);
    }


    public void PlayFlyVoice() {
        VoiceManagerEnemy.FlyVoice();
    }
}
