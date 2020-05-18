using Valve.VR;
using UnityEngine;
using WavesBehavior;
using Movement;
using Boids;


//Input Manager class
namespace GameInput
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] WaveSpawner mainSpawner;
        [SerializeField] HumanSoundSpawner humanSoundSpawner;
        [SerializeField] BoidsManager boidsManager;
        [SerializeField] SteamVR_Action_Vector2 moveAction;

        VRMovement mover;

        private void Awake()
        {
            mover = GetComponent<VRMovement>();
        }

        void Update()
        {
            //Move
           // Vector2 axis = moveAction.GetAxis(SteamVR_Input_Sources.Any);  //Activate with VR
            Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); //Activate without VR! Only for testing.
            if (axis != Vector2.zero) mover.Move(axis);

            //Spawn main wave
            if (Input.GetKeyDown(KeyCode.H) || SteamVR_Actions._default.Teleport.GetStateDown(SteamVR_Input_Sources.Any)) mainSpawner.SpawnMainWave();

            //Change main wave
            if (Input.GetKeyDown(KeyCode.C)) mainSpawner.ChangeIndex();

            //Spawn Human Sound
            if (Input.GetKeyDown(KeyCode.T) || SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.Any)) humanSoundSpawner.InstantiateSound();

            //Set boid target 
            if (Input.GetKeyDown(KeyCode.F)) boidsManager.SetTarget(true);

            //Set boid target False
            if (Input.GetKeyDown(KeyCode.G)) boidsManager.SetTarget(false);

            //Quit App:
            if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        }
    }
}

