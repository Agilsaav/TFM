using Valve.VR;
using UnityEngine;
using WavesBehavior;
using Movement;

namespace GameInput
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] WaveSpawner mainSpawner;
        [SerializeField] HumanSoundSpawner humanSoundSpawner;
        [SerializeField] SteamVR_Action_Vector2 moveAction;

        VRMovement mover;

        private void Awake()
        {
            mover = GetComponent<VRMovement>();
        }


        void Update()
        {
            //Move
            Vector2 axis = moveAction.GetAxis(SteamVR_Input_Sources.Any);
            if (axis != Vector2.zero) mover.Move(axis);

            //Spawn main wave
            if (Input.GetKeyDown(KeyCode.H) || SteamVR_Actions._default.Teleport.GetStateDown(SteamVR_Input_Sources.Any)) mainSpawner.SpawnMainWave();

            //Spawn Human Sound
            if (Input.GetKeyDown(KeyCode.T) || SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.Any)) humanSoundSpawner.InstantiateSound();

            //Quit App:
            if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        }
    }
}

