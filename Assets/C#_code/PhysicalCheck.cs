using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalCheck : MonoBehaviour
{

    public bool is_ground;  //�Ƿ����
    public bool front_ground;   //ǰ���Ƿ�����
    public bool behind_ground;  //���Ƿ�����
    public bool front_wall;     //ǰ���Ƿ�ǽ��
    public bool behind_wall;    //���Ƿ�ǽ��

    [Header("������")]
    public Vector2 f_offSet;    //ǰ��λ�Ʋ�ֵ
    public Vector2 b_offSet;    //��λ�Ʋ�ֵ
    public Vector2 offSet;  //λ�Ʋ�ֵ
    [Header("���ǽ��")]
    public Vector2 f_w_offSet;    //ǰ��ǽ��λ�Ʋ�ֵ
    public Vector2 b_w_offSet;    //��ǽ��λ�Ʋ�ֵ
    public float chekR;     //��ⷶΧ
    public LayerMask ground;    //������
    public LayerMask wall;      //���ǽ��


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

    //������
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

    //���Ƽ�ⷶΧ
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + offSet, chekR);
        Gizmos.DrawWireSphere((Vector2)transform.position + f_offSet, chekR);
        Gizmos.DrawWireSphere((Vector2)transform.position + b_offSet, chekR);
        Gizmos.DrawWireSphere((Vector2)transform.position + f_w_offSet, chekR);
        Gizmos.DrawWireSphere((Vector2)transform.position + b_w_offSet, chekR);
    }
}
