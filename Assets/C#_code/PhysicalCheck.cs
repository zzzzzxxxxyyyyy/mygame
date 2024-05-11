using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalCheck : MonoBehaviour
{

    public bool is_ground;  //是否地面
    public bool front_ground;   //前方是否悬崖
    public bool behind_ground;  //后方是否悬崖
    public bool front_wall;     //前方是否墙壁
    public bool behind_wall;    //后方是否墙壁

    [Header("检测地面")]
    public Vector2 f_offSet;    //前方位移差值
    public Vector2 b_offSet;    //后方位移差值
    public Vector2 offSet;  //位移差值
    [Header("检测墙面")]
    public Vector2 f_w_offSet;    //前方墙壁位移差值
    public Vector2 b_w_offSet;    //后方墙壁位移差值
    public float chekR;     //检测范围
    public LayerMask ground;    //检测地面
    public LayerMask wall;      //检测墙壁


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        CheckWall();
    }

    //地面检测
    public void CheckGround() {
        is_ground =  Physics2D.OverlapCircle( (Vector2)transform.position + offSet, chekR, ground);
        front_ground = Physics2D.OverlapCircle((Vector2)transform.position + f_offSet, chekR, ground);
        behind_ground = Physics2D.OverlapCircle((Vector2)transform.position + b_offSet, chekR, ground);
    }

    public void CheckWall()
    {
        front_wall = Physics2D.OverlapCircle((Vector2)transform.position + f_w_offSet, chekR, wall);
        behind_wall = Physics2D.OverlapCircle((Vector2)transform.position + b_w_offSet, chekR, wall);
    }

    //绘制检测范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + offSet, chekR);
        Gizmos.DrawWireSphere((Vector2)transform.position + f_offSet, chekR);
        Gizmos.DrawWireSphere((Vector2)transform.position + b_offSet, chekR);
        Gizmos.DrawWireSphere((Vector2)transform.position + f_w_offSet, chekR);
        Gizmos.DrawWireSphere((Vector2)transform.position + b_w_offSet, chekR);
    }
}
