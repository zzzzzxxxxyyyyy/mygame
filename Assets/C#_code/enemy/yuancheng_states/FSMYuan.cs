using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ParameterYuan
{
    [Header("组件")]
    public Rigidbody2D rigidBody2D;    //引用刚体
    public Transform transform;
    public Animator animator;          //引用动画器

    public PhysicalCheck physicalCheck;
    public enemyHp enemyHp;


    public Vector3 face_dir;        //面朝方向
    [Header("移动速度")]
    public float patrol_speed;      //巡逻速度
    public float run_speed;         //逃跑速度
    public float chase_speed;       //追击速度
    [Header("重力模式")]
    public bool gravity;   //击飞判断 
    [Header("受击")]
    public bool is_hit;            //是否被攻击
    public bool is_invincible;    //是否死亡无敌
    public bool is_down;            //是否倒地
    public float chen_position;     //陈的位置
    public Vector2 hit_power;       //击飞力度
    public float hit_y;         //击飞的y轴向量
    [Header("追击距离检测")]
    public float check_distance;    //检测距离
    public float chase_time;        //追击时间
    public float run_time;          //逃逸时间
    public float save_distance;     //安全距离
    public LayerMask layer;         //检测图层
    [Header("攻击距离检测")]
    public bool ready_atk;
    public float reload_time;           //攻击间隔
    public float temp_time;            //中间时间
    public float attack_distance;       //检测距离
    public GameObject arrow;            //箭矢
    [Header("受击特效")]
    public GameObject[] effects;

}
public class FSMYuan : MonoBehaviour
{
    private IState nowState;
    public ParameterYuan parameter;
    //使用字典注册所有的状态
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
        ReAtk();                                                            //攻击间隔
        parameter.face_dir = new Vector3(-transform.localScale.x, 0, 0);    //实时获得面朝方向
        parameter.hit_y = parameter.rigidBody2D.velocity.y;                 //实时获得y轴向量
        nowState.OnUpdate();

    }

    private void FixedUpdate()
    {
        nowState.OnFixUpdate();
    }

    //切换状态
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

    /**
     * 判定绘制
     * */
    //发现玩家
    public bool FoundChen()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * parameter.face_dir, parameter.check_distance, parameter.layer);
        if (hit)
        {
            return true;
        }
        return false;
    }
    //攻击玩家
    public bool AttackChen()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * parameter.face_dir, parameter.attack_distance, parameter.layer);
        if (hit)
        {
            return true;
        }
        return false;
    }
    //安全距离
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
     * 功能代码
     * */
    //敌人悬崖反转
    public void EnemyFlip()
    {
        if ((!parameter.physicalCheck.behind_ground && parameter.face_dir.x > 0) || (!parameter.physicalCheck.front_ground && parameter.face_dir.x < 0))
        {
            transform.localScale = new Vector3(parameter.face_dir.x, 1, 1);
        }
    }
    //敌人无条件反转
    public void EnemyFlip2() {
        transform.localScale = new Vector3(parameter.face_dir.x, 1, 1);
    }
    public void Move(float speed)
    {
        parameter.rigidBody2D.velocity = new Vector2(speed * parameter.face_dir.x * Time.deltaTime, parameter.rigidBody2D.velocity.y);
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
        parameter.rigidBody2D.velocity = new(0, parameter.rigidBody2D.velocity.y);
        //击飞
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
    //受击特效
    public void HitEffect(int x)
    {
        GameObject temp_effect = ObjectPool.Instance.GetObject(parameter.effects[x]);
        temp_effect.transform.position = new Vector2(transform.position.x, transform.position.y);
    }


    //面朝玩家
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

    //攻击射箭
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



    //动画播放完毕后自动调用销毁
    private void DestoryThis()
    {
        ObjectPool.Instance.PushGameObject(this.gameObject);
    }
}
