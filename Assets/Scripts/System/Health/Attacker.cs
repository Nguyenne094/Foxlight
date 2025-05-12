using UnityEngine;

namespace Bap.System.Health
{
    public interface IAttack
    {
        public int Damage { get; set; }
    }
    
    public class Attacker : MonoBehaviour, IAttack
    {
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private int _damage;
        [SerializeField] private bool _useTriggerCollider = true;
        
        public int Damage { get => _damage; set => _damage = value; }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (_useTriggerCollider && ((1 << other.gameObject.layer) & _targetLayer) != 0)
            {
                if (other.TryGetComponent<IHealth>(out var health))
                {
                    health.TakeDamage(Damage, transform);
                }
            }
        }
    }
}