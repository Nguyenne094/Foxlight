using System;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DisappearPlatform : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _reappearDuation;

    private Collider2D _col;
    private SpriteRenderer _sprite;
    
    private void Awake()
    {
        _col = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _col.isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Disappear();
    }

    private async Task Disappear()
    {
        //TODO: Play Animation
        await Task.Delay((int)(_lifeTime * 1000));
        //TODO: Play Animation, Sfx, Vfx
        _col.isTrigger = true;
        _sprite.enabled = false;
        Reappear();
    }

    private async Task Reappear()
    {
        await Task.Delay((int)(_reappearDuation * 1000));
        _col.isTrigger = false;
        _sprite.enabled = true;
    }
}