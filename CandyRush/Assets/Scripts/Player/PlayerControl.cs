using System;
using System.Collections;
using UnityEngine;

namespace Alexender.Runer
{
    public class PlayerControl : MonoBehaviour
    {
        // Dependencies
        private Rigidbody rb;
        private CharacterController controller;

        // Movement
        [SerializeField] private float lineDistance = 4;

        // Physics
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float fallForce;
        [SerializeField] private float gravity;
        [SerializeField] private float weight = 40;

        // Parameters
        [SerializeField] private float maxSpeed = 110;

        // Own field
        private Vector3 movementDirection;
        private int currentLine = 1;
        private float subtraction = 0f;

        public event Action CollidedWithObstacle;

        public PlayerModel Model { get; set; }

        private void Awake()
        {
            Model = new PlayerModel();
        }

        void Start()
        {
            controller = GetComponent<CharacterController>();
            rb = GetComponent<Rigidbody>();

            StartCoroutine(SpeedIncrease());
        }

        private void Update()
        {
            subtraction += Time.deltaTime;

            if (subtraction >= 5f)
            {
                subtraction = 0f;
                weight *= 0.8f;
            }

            if (SwipeController.swipeRight)
            {
                if (currentLine < 2)
                    currentLine++;
            }

            if (SwipeController.swipeLeft)
            {
                if (currentLine > 0)
                    currentLine--;
            }

            if (SwipeController.swipeUp)
            {
                if (controller.isGrounded)
                    Jump();
            }

            if (SwipeController.swipeDown && !controller.isGrounded)
            {
                Fall();
            }

            //if (controller.isGrounded)
            //   anim.SetTrigger("isRunning");

            Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
            if (currentLine == 0)
                targetPosition += Vector3.left * lineDistance;
            else if (currentLine == 2)
                targetPosition += Vector3.right * lineDistance;

            if (transform.position == targetPosition)
                return;
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = 25 * Time.deltaTime * diff.normalized;

            if (moveDir.sqrMagnitude < diff.sqrMagnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
        }

        private void Jump()
        {
            movementDirection.y = jumpForce;
        }

        private void Fall()
        {
            movementDirection.y = -fallForce;
        }

        private void FixedUpdate()
        {
            movementDirection.z = speed;
            movementDirection.y += gravity * Time.fixedDeltaTime;
            controller.Move(movementDirection * Time.fixedDeltaTime);
            Model.DistanceScore = transform.position.z / 2.0F;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.tag == "obstacle")
            {
                CollidedWithObstacle?.Invoke();
                // TODO: need fix
                //int lastRunScore = int.Parse(scoreScript.scoreText.text.ToString());
                //PlayerPrefs.SetInt("lastRunScore", lastRunScore);
                Time.timeScale = 0;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Candy")
            {
                Model.CandyCount++;
                Destroy(other.gameObject);
                weight += 3;
            }

            if (weight <= 20)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else if (weight >= 80)
            {
                rb.isKinematic = true;
            }
        }

        private IEnumerator SpeedIncrease()
        {
            yield return new WaitForSeconds(1);
            if (speed < maxSpeed)
            {
                speed += 1;
                StartCoroutine(SpeedIncrease());
            }
        }
    }
}