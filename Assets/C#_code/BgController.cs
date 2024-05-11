using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgController : MonoBehaviour
{
    //摄像机
    private Transform cameraT;//前一帧摄像机位置
    private Vector3 lastCameraPoi;//后一帧摄像机位置

    //子物体移动倍速
    public Vector2 SpeedTime = new Vector2(1f, 1f);

    //纹理图宽度
    private float textUnitSizex;


    // Start is called before the first frame update
    void Start()
    {

        cameraT = Camera.main.transform;
        lastCameraPoi = cameraT.position;
        //获取纹理图及宽度
        Sprite sprite = transform.GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        //纹理图宽度除像素宽度
        textUnitSizex = texture.width / sprite.pixelsPerUnit;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delMovement = cameraT.position - lastCameraPoi;
        transform.position += new Vector3(delMovement.x * SpeedTime.x, delMovement.y * SpeedTime.y, 0);
        //更新后一帧摄像机位置
        lastCameraPoi = cameraT.position;

    }
}
