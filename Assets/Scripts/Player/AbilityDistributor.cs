using System;
using UnityEngine;

namespace PlatformingGame.Controller
{
    public class AbilityDistributor : MonoBehaviour
    {
        [SerializeField] private PlayerAbilityScriptableObject _ability;

        private void OnTriggerEnter2D(Collider2D other)
        {
            //TODO: some ui, vfx, sfx
            if (other.CompareTag("Player"))
            {
                PlayerController controller = other.GetComponent<PlayerController>();
                Debug.Log("ac");
                controller.ConStat.Roll = _ability;
                controller.ConStat.Init(controller);
            }
        }
    }
}