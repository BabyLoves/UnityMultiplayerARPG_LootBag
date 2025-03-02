using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiplayerARPG
{
    public class ActivateButtonActivator_LootBag : MonoBehaviour
    {
        public GameObject[] activateObjects;
        public TextWrapper buttonTextWrapper;
        public string activateButtonText = "ACTIVE";
        public string lootButtonText = "LOOT";

        private bool canActivate;
        private PlayerCharacterController controller;
        private ShooterPlayerCharacterController shooterController;
        private BuildingEntity targetBuilding;
        
        private void LateUpdate()
        {
            canActivate = false;

            controller = BasePlayerCharacterController.Singleton as PlayerCharacterController;
            shooterController = BasePlayerCharacterController.Singleton as ShooterPlayerCharacterController;

            if (controller != null)
            {
                canActivate = controller.ActivatableEntityDetector.players.Count > 0 ||
                    controller.ActivatableEntityDetector.npcs.Count > 0 ||
                    controller.ActivatableEntityDetector.buildings.Count > 0;
                
                if (controller.ActivatableEntityDetector.buildings.Count > 0)
                    targetBuilding = controller.ActivatableEntityDetector.buildings[0];
            }

            if (shooterController != null && shooterController.SelectedEntity != null)
            {
                canActivate = shooterController.SelectedEntity is BasePlayerCharacterEntity || shooterController.SelectedEntity is NpcEntity;
                if (!canActivate)
                {
                    targetBuilding = shooterController.SelectedEntity as BuildingEntity;
                    if (targetBuilding != null && !targetBuilding.IsBuildMode && targetBuilding.Activatable)
                        canActivate = true;
                }
            }

            foreach (GameObject obj in activateObjects)
            {
                obj.SetActive(canActivate);
            }

            if (targetBuilding != null && targetBuilding is LootBagEntity)
                buttonTextWrapper.text = lootButtonText;
            else
                buttonTextWrapper.text = activateButtonText;
        }
    }
}