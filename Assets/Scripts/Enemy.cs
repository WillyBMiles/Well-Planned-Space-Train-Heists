using DG.Tweening;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable
{

    public static event Action GotHit;
    [SerializeField] int maxHealth;
    public int MaxHealth => maxHealth;
    [SerializeField, ReadOnly(true)] int health;
    public int Health => health;
    List<Material> materials = new();
    [SerializeField] ParticleSystem damageSystem;
    [SerializeField] int numberOfParticles;
    [SerializeField] float explosionSize;
    [SerializeField] Transform middle;
    public int numberOfCubesToDrop;


    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        
    }

    private void Start()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            materials.AddRange(r.materials);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        GameController.Instance.OnHitDamage += HitDamage;
    }

    private void HitDamage(GameObject collider, int damage)
    {
        if (collider == gameObject)
        {
            TakeDamage(damage);
            
        }
    }

    public void TakeDamage(int damage)
    {
        damageSystem.Emit(numberOfParticles);
        animator.SetTrigger("Damage");
        GotHit?.Invoke();

        health -= damage;
        //EFFECT
        if (health <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        gameObject.SetActive(false);
        Explosion.CreateExplosion(middle.transform.position, transform.rotation, explosionSize);

        for (int i=0; i < numberOfCubesToDrop; i++)
        {
            var cube = ObjectPool.SpawnObject<RewardCube>("RewardCube");
            cube.transform.position = transform.position + UnityEngine.Random.insideUnitSphere * 2f;
            cube.transform.rotation = UnityEngine.Random.rotationUniform;

        }
    }


    private void OnDisable()
    {
        GameController.Instance.OnHitDamage -= HitDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPoolable()
    {
        health = maxHealth;
    }
}
