using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Collections;

[Serializable]
public class ParameterWu
{
    [Header("组件")]
    public Rigidbody2D rigidBody2D;    //引用刚体
    public Seeker seeker;
    public Transform transform;
    public Animator animator;          //引用动画器

    public CapsuleCollider2D capsuleCollider2D;//引用2d胶囊碰撞器
    public PhysicalCheck physicalCheck;
    public enemyHp enemyHp;


    public Vector3 face_dir;        //面朝方向
    [Header("移动速度")]
    public float patrol_speed;      //巡逻速度
    public float reload_speed;  //重新回盒速度
    public float max_up;        //最大高度
    public LayerMask groud_layer;       //地面检测  
    public float up_speed;      //飞升速度
    public float pat_time;          //巡逻时间
    public float idle_time;         //等待时间
    [Header("受击")]
    public bool is_hit;            //是否被攻击
    public bool is_invincible;    //是否死亡无敌
    public bool is_down;
    public float chen_position;     //陈的位置
    public Vector2 hit_power;       //击飞力度
    public float hit_y;         //被击飞的y轴向量
    [Header("攻击距离检测")]
    public LayerMask layer;         //检测图层
    public Vector2 attack_offset;   //中心
    public Vector2 attack_siez;      //检测大小
    public float attack_distance;    //检测距离
    public float attack_r;          //检测范围      
    public bool is_boom = true ;        //是带有炸弹
    [Header("重新装弹")]
    public Transform BoomBox;   //装弹目标
    public float next_postion;  //距离下一个目标多远执行移动
    public Path path;                  //当前路径
    public int current_way = 0;        //目标路径
    public bool end_path = false;       //终点目标 
    public GameObject bomb;
    [Header("受击特效")]
    public GameObject[] effects;
}
public class FSMBoomWu : MonoBehaviour
{
    private IState nowState;
    public ParameterWu parameter;
    //使用字典注册所有的状态
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
        parameter.face_dir = new Vector3(-transform.localScale.x, 0, 0);    //实时获得面朝方向
        nowState.OnUpdate();
    }

    private void FixedUpdate()
    {
        nowState.OnFixUpdate();
    }


    public void SwitchState(StateType type)
    {
        //当前状态存在时推出当前状态
        if (nowState != null)
        {
            nowState.OnExit();
        }
        nowState = states[type];
        //执行新的状态
        nowState.OnEnter();
    }

    public void OnPathComplete(Path p) {
        if (!p.error) {
            parameter.path = p;
            parameter.current_way = 0;
        }
    }
    //攻击玩家
    public bool AttackChen()
    {
        return Physics2D.BoxCast(parameter.transform.position + (Vector3)parameter.attack_offset, parameter.attack_siez, 0, parameter.face_dir, parameter.attack_distance, parameter.layer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)parameter.attack_offset + new Vector3(parameter.attack_distance * -transform.localScale.x, 0), parameter.attack_r);
        Debug.DrawRay(transform.position, -Vector2.up * parameter.max_up, Color.red);
    }

    //寻路
    public void BlackBox(float speed) {
        //判断是否还有路径
        if (parameter.path == null) {
            return;
        }
        //判断是否达到终点路径
        if (parameter.current_way >= parameter.path.vectorPath.Count)
        {
            parameter.end_path = true;
            return;
        }
        else {
            parameter.end_path = false;
        }
        //下一个路径点方向
        Vector2 v_dir = ((Vector2)parameter.path.vectorPath[parameter.current_way] - parameter.rigidBody2D.position).normalized;
        Vector2 force = v_dir * speed * Time.deltaTime;
        parameter.rigidBody2D.AddForce(force);

        float distance = Vector2.Distance(parameter.rigidBody2D.position, parameter.path.vectorPath[parameter.current_way]);
        if (distance < parameter.next_postion) {
            parameter.current_way++;
        }
        //寻路转向
        if (parameter.rigidBody2D.velocity.x > 0.1f) {
            parameter.transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (parameter.rigidBody2D.velocity.x < 0.1f) {
            parameter.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    //更新路径
    public void UpdatePath() {
        if (parameter.seeker.IsDone()) {
            parameter.seeker.StartPath(parameter.rigidBody2D.position, parameter.BoomBox.position, OnPathComplete);
        } 
    }
    //发射导弹
    public void ShootBomb() {
        GameObject temp_bomb = ObjectPool.Instance.GetObject(parameter.bomb);
        temp_bomb.transform.position = new Vector2(transform.position.x - 0.133f, transform.position.y - 0.722f);
        temp_bomb.GetComponent<ShootBomb>().ShootThis();
    }
    //移动
    public void Move(float speed)
    {
        parameter.rigidBody2D.velocity = new Vector2(speed * parameter.face_dir.x * Time.deltaTime, parameter.rigidBody2D.velocity.y);
    }
    //上升
    public void UpMove(float speed) {
        parameter.rigidBody2D.velocity = new Vector2(parameter.rigidBody2D.velocity.x, speed * Time.deltaTime);
    }
    //是否上升到最高处
    public bool is_up() { 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up,parameter.max_up, parameter.groud_layer);
        if (hit) {
            return true;
        }
        return false;
    }
    //敌人反转
    public void EnemyFlip()
    {
        transform.localScale = new Vector3(parameter.face_dir.x, 1, 1);
    }
    //改变重力
    public void SetGravity(float param) {
        parameter.rigidBody2D.gravityScale = param;
    }
    //受击反转
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
        //停滞
        StartCoroutine(HitFly(new Vector2(parameter.hit_power.x * 5 ,parameter.hit_power.y)));
    }
    private IEnumerator HitFly(Vector2 v)
    {
        parameter.rigidBody2D.AddForce(v, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
    }
    //死亡放回对象池中
    public void DestroyThis() {
        ObjectPool.Instance.PushGameObject(this.gameObject);
    }

    //受击特效
    public void HitEffect(int x)
    {
        GameObject temp_effect = ObjectPool.Instance.GetObject(parameter.effects[x]);
        temp_effect.transform.position = new Vector2(transform.position.x, transform.position.y);
    }


    public void PlayFlyVoice() {
        VoiceManagerEnemy.FlyVoice();
    }
}
