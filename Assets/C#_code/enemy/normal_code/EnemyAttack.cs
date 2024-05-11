using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage;   //�˺�
    public float hit_power; //��������

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            VoiceManager.ChenHit();
            collision.GetComponent<ChenUI>().ReduceHp(damage);
            collision.GetComponent<chenPlayer>().HitByEnemy(this.transform,hit_power);
        }
    }
}
