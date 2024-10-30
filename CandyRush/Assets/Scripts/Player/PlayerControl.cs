using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private CharacterController controller;
    //private Animator anim;
    private Vector3 dir;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallForce;
    [SerializeField] private float gravity;
    [SerializeField] private int candy;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private Text candyText;
    [SerializeField] private Score scoreScript;
    [SerializeField] public Rigidbody rb;

    private int lineToMove = 1;
    public float lineDistance = 4; 
    private float maxSpeed = 110;
    private float subtraction = 0f;
    public float weight = 40;
    public bool canControl = true;

    void Start()
    {
       // anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        StartCoroutine(SpeedIncrease());
        Time.timeScale = 1;
        rb = GetComponent<Rigidbody>();
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
            if (lineToMove < 2)
                lineToMove++;
        }

        if (SwipeController.swipeLeft)
        {
            if (lineToMove > 0)
                lineToMove--;
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
        if (lineToMove == 0)
            targetPosition += Vector3.left * lineDistance;
        else if (lineToMove == 2)
            targetPosition += Vector3.right * lineDistance;

        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
    }

    private void Jump()
    {
        dir.y = jumpForce;
    }

    private void Fall()
    {
        dir.y = -fallForce;
    }

    private void FixedUpdate()
    {
        dir.z = speed;
        dir.y += gravity * Time.fixedDeltaTime;
        controller.Move(dir * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "obstacle")
        {
            losePanel.SetActive(true);
            int lastRunScore = int.Parse(scoreScript.scoreText.text.ToString());
            PlayerPrefs.SetInt("lastRunScore", lastRunScore);
            Time.timeScale = 0;
        }
    }

     private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Candy")
        {
            candy++;
            candyText.text = candy.ToString();
            Destroy(other.gameObject);
            weight += 3;
        }

        if (weight <= 20)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canControl = false; 
        }
        else if (weight >= 80)
        {
            rb.isKinematic = true;
            canControl = false;
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