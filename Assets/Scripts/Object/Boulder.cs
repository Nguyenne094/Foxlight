using UnityEngine;
using UnityEngine.Serialization;

namespace Stuffs
{
    using Bap.System.Health;
    public class Boulder : MonoBehaviour
    {
        [Min(0), SerializeField] private int _damage = 10;
        [SerializeField] private LayerMask _targetLayer;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            bool isFalling = GetComponent<Rigidbody2D>().linearVelocityY < 0;
            if (isFalling && 
                ((1 << collision.gameObject.layer) & _targetLayer) != 0 &&
                 collision.transform.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
            {
                if (enemy != null)
                {
                    // Check if the collision is from above
                    ContactPoint2D contact = collision.GetContact(0);
                    if (contact.normal.y > 0)
                    {
                        enemy.TakeDamage(_damage, transform);
                        //TODO: Play sfx, vfx like boulder break
                    }
                    
                    // Debug the normal vector
                    Debug.DrawRay(contact.point, contact.normal, Color.red, 2.0f);
                }
                else
                {
                    Debug.LogWarning(collision.gameObject.name + ": don't have Enemy component");
                }
            }
        }
    }
}