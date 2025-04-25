using Bap.Manager;
using UnityEngine;


public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform _position;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManger.Instance.LastCheckPoint = _position.position;
        }
    }
}