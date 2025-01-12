using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// This event is triggered when the player character enters a trigger with a VictoryZone component.
    /// </summary>
    public class PlayerEnteredVictoryZone : Simulation.Event<PlayerEnteredVictoryZone>
    {
        public VictoryZone victoryZone;
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            if (model != null)
            {
                if (model.player != null && model.player.animator != null)
                {
                    model.player.animator.SetTrigger("victory");
                    model.player.controlEnabled = false;
                }
                else
                {
                    Debug.LogError("Player or Animator is not set. Please ensure the Player is correctly set up in the scene.");
                }
            }
            else
            {
                Debug.LogError("PlatformerModel is null. Make sure it is correctly initialized.");
            }
        }
    }
}
