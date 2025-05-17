using System;
using Bap.State_Machine;
using PlatformingGame.Controller;
using UnityEngine;

public class AbilityDistributor : MonoBehaviour
{
    [SerializeField] private PlayerAbilityScriptableObject _ability;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //TODO: some ui, vfx, sfx
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerContext>())
            {
                var controller = other.GetComponent<PlayerContext>();
                controller.ControllerStatSo.Roll = _ability;
                controller.ControllerStatSo.Init(controller);
            }
        }
    }
}