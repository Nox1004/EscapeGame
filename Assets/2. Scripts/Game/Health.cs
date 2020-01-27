using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EscapeGame.Object;

public class Health : MonoBehaviour
{
    //[Header("Audio Clip")]
    //public AudioClip heartbeatSound;

    [SerializeField] float maxHp = 0;

    public float Hp { get; private set; }
    public float MaxHp { get { return maxHp; } }
    
    /// <summary>
    /// 데미지를 받는 처리
    /// </summary>
    /// <param name="damage">받는량</param>
    public void Damaged(float damage)
    {
        Hp -= damage;

        if (Hp < 0)
            Hp = 0;
    }

    private void Awake()
    {
        Initialize(MaxHp);
    }

    private void Initialize(float max)
    {
        Hp = max;
    }

}
