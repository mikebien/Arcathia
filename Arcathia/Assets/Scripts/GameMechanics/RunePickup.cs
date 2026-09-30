using Unity.Netcode;
using UnityEngine;

public class RunePickup : NetworkBehaviour
{
    public SpellType spellType = SpellType.Water; // Set to Water or Lightning in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        // Networked check: Ensure the object touching the rune is a Player
        PlayerMagicNetwork playerMagic = other.GetComponentInParent<PlayerMagicNetwork>();

        if (playerMagic != null)
        {
            // Only execute locally if this player is controlled by the local client on this device
            if (playerMagic.IsOwner)
            {
                SpellbookUI spellbook = FindFirstObjectByType<SpellbookUI>();
                if (spellbook != null)
                {
                    spellbook.AddRuneToInventory(spellType);
                    Debug.Log($"Picked up {spellType} Rune!");
                }

                // Request the server to despawn this rune across all clients
                DespawnRuneRpc();
            }
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void DespawnRuneRpc()
    {
        if (NetworkObject != null && NetworkObject.IsSpawned)
        {
            NetworkObject.Despawn();
        }
    }
}