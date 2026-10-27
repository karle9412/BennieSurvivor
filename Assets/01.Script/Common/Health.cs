using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxhealth = 100;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxhealth;
    public bool Isdead => CurrentHealth <= 0;

    private void Awake()
    {
        CurrentHealth = maxhealth;
    }

    public void TakeDamage(int damage)
    {
        if (Isdead || damage <= 0)
            return;

        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        Debug.Log($"{gameObject.name} 체력 : {CurrentHealth} / {maxhealth}"); 
        if(Isdead)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} 사망");

        //임시 사망 처리
        gameObject.SetActive(false);
    }

    [ContextMenu("테슽트 : 10 피해")]
    private void TestDamage()
    {
        if (!Application.isPlaying)
            return;
        TakeDamage(10);
    }
}
