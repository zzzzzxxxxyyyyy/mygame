using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgController : MonoBehaviour
{
    //�����
    private Transform cameraT;//ǰһ֡�����λ��
    private Vector3 lastCameraPoi;//��һ֡�����λ��

    //�������ƶ�����
    public Vector2 SpeedTime = new Vector2(1f, 1f);

    //����ͼ���
    private float textUnitSizex;


    // Start is called before the first frame update
    void Start()
    {

        cameraT = Camera.main.transform;
        lastCameraPoi = cameraT.position;
        //��ȡ����ͼ�����
        Sprite sprite = transform.GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        //����ͼ��ȳ����ؿ��
        textUnitSizex = texture.width / sprite.pixelsPerUnit;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delMovement = cameraT.position - lastCameraPoi;
        transform.position += new Vector3(delMovement.x * SpeedTime.x, delMovement.y * SpeedTime.y, 0);
        //���º�һ֡�����λ��
        lastCameraPoi = cameraT.position;

    }
}
