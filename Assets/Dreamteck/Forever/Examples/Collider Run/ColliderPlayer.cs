namespace Dreamteck.Forever
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ColliderPlayer : MonoBehaviour
    {
        public float minRadius = 2f;
        public float maxRadius = 10f;

        public float verticalSensitivity = 1f;
        public float horizontalSenzitivity = 1f;

        public float verticalSpeed = 10f;
        public float horizontalSpeed = 10f;

        public float startSpeed = 30f;
        public float maxSpeed = 70f;
        public float acceleration = 0.1f;

        public Transform cameraTransform;
        Vector3 camLocalPos;

        Runner runner;
        public Vector2 input = Vector2.zero;
        public Vector2 lerpInput = Vector2.zero;

        Vector2 cameraInput;
        public float cameraSensitivity = 1f;
        public Vector2 cameraMaxPan = Vector2.one;
        public Vector3 cameraMaxRotate = Vector3.zero;

        Rigidbody rb;

        private void Awake()
        {
            runner = GetComponent<Runner>();
            rb = GetComponent<Rigidbody>();
            runner.followSpeed = startSpeed;
            camLocalPos = cameraTransform.localPosition;
            EndScreen.onRestartClicked += OnRestart;
            Cursor.lockState = CursorLockMode.Locked;
            runner.motion.offset = Vector2.zero;
        }

        void OnRestart()
        {
            LevelGenerator.instance.Restart();
            runner.follow = true;
            runner.followSpeed = startSpeed;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void FixedUpdate()
        {
            rb.isKinematic = !LevelGenerator.instance.ready;
            //Handle input from mouse

            input.x += Input.GetAxis("Horizontal") * horizontalSenzitivity;
            input.y -= Input.GetAxis("Vertical") * verticalSensitivity;
            input.x = Mathf.Clamp01(input.x);
            input.y = Mathf.Clamp01(input.y);
            //if (input.x > 360f) input.x -= 360f * Mathf.FloorToInt(input.x / 360f);
            //if (input.x < 360f) input.x += 360f * Mathf.FloorToInt(input.x / -360f);
            Vector2 lastInput = lerpInput;
            lerpInput.x = Mathf.Lerp(lerpInput.x, input.x, Time.deltaTime * horizontalSpeed);
            lerpInput.y = Mathf.Lerp(lerpInput.y, input.y, Time.deltaTime * verticalSpeed);
            Vector2 inputDelta = Vector2.zero;
            inputDelta.x = Mathf.DeltaAngle(lastInput.x, lerpInput.x);
            inputDelta.y = lerpInput.y - lastInput.y;
            inputDelta.x /= horizontalSenzitivity;
            inputDelta.y /= verticalSensitivity;

            //Gradually increase the speed
            runner.followSpeed = Mathf.MoveTowards(runner.followSpeed, maxSpeed, Time.deltaTime * acceleration);


            //Handle Camera
            if (runner.follow)
            {
                cameraInput.x += inputDelta.x * cameraSensitivity;
                cameraInput.y += inputDelta.y * cameraSensitivity;
                cameraInput.x = Mathf.Clamp(cameraInput.x, -1f, 1f);
                cameraInput.y = Mathf.Clamp(cameraInput.y, -1f, 1f);
            }
            cameraTransform.localRotation = Quaternion.Euler(cameraInput.y * cameraMaxRotate.x, cameraInput.x * lerpInput.y * cameraMaxRotate.y, -cameraInput.x * Mathf.Abs(1f - cameraInput.y) * cameraMaxRotate.z);
            cameraTransform.localPosition = camLocalPos - Vector3.right * cameraInput.x * cameraMaxPan.x * lerpInput.y + Vector3.up * cameraInput.y * cameraMaxPan.y;
            cameraInput.x = Mathf.Lerp(cameraInput.x, 0f, Time.deltaTime * 3f);
            cameraInput.y = Mathf.Lerp(cameraInput.y, 0f, Time.deltaTime * 3f);

            //Offset the runner to move the player
            //runner.motion.rotationOffset = Vector3.forward * lerpInput.x;
            runner.motion.offset = new Vector2(Mathf.Lerp(-8f, 8f, lerpInput.x), Mathf.Lerp(8f, -8f, lerpInput.y));
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Crash
            runner.follow = false;
            Cursor.lockState = CursorLockMode.None;
            EndScreen.Open();
        }
    }
}
