using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsHandler : MonoBehaviour
{
    // �����������
    private Rigidbody rb;
    private CharacterController controller;
    private PlayerModel playerModel;
    private float gravity;
    private float jumpForce;
    private float fallForce;
    private float speed;

    public event Action CollidedWithObstacle;

    // �������������
    public void Init(Rigidbody rb, CharacterController controller, PlayerModel playerModel, float gravity, float jumpForce, float fallForce, float speed)
    {
        this.rb = rb;
        this.controller = controller;
        this.playerModel = playerModel;
        this.gravity = gravity;
        this.jumpForce = jumpForce;
        this.fallForce = fallForce;
        this.speed = speed;
    }

    // ����� ��� ��������� ������
    public void HandleFixedUpdate(Vector3 movementDirection)
    {
        movementDirection.z = speed;
        movementDirection.y += gravity * Time.fixedDeltaTime;
        controller.Move(movementDirection * Time.fixedDeltaTime);
        playerModel.DistanceScore = transform.position.z / 2.0F;
    }

    // ����� ��� ��������� �������� � ���������
    public void HandleControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "obstacle")
        {
            CollidedWithObstacle?.Invoke();
            Time.timeScale = 0;
        }
    }

    // ����� ��� ��������� ���������
    public void HandleTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Candy")
        {
            playerModel.CandyCount++;
            Destroy(other.gameObject);
            // ��������, ��� ������ ���� � ������ �����, �� ������� �����
            playerModel.Weight += 3;
        }

        if (playerModel.Weight <= 20)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if (playerModel.Weight >= 80)
        {
            rb.isKinematic = true;
        }
    }
}
