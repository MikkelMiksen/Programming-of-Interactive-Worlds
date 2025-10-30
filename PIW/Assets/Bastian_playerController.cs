using UnityEngine;

public class Bastian_playerController : MonoBehaviour
{
    [SerializeField] float bastian_player_speed = 2;
    private CharacterController character_controller;
    Vector3 move_vector = Vector3.zero;
    float gravity = -9.82f * 1.5f;
    Vector3 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        character_controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {


        velocity.y += gravity * Time.deltaTime;

        move_vector = new Vector3(Input.GetAxis("Horizontal"), velocity.y, Input.GetAxis("Vertical"));
        move_vector = transform.TransformDirection(move_vector);

        Vector2 hori_move = new Vector2(move_vector.x, move_vector.z);
        hori_move.Normalize();

        move_vector.x = hori_move.x;
        move_vector.z = hori_move.y;

        character_controller.Move(move_vector * bastian_player_speed * Time.deltaTime);

        if (character_controller.isGrounded)
        {
            velocity.y = 0;
        }
    }
}
