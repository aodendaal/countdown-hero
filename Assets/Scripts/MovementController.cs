using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private AudioSource heroWalk;
    private InputSystem_Actions inputActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    private void Move(Vector2 direction)
    {
        Debug.Log(direction);
        heroWalk.Play();
        transform.Translate(new Vector3((int)direction.x, (int)direction.y, 0f));
    }

    void OnEnable()
    {
        inputActions.Enable();
    }
    void OnDisable()
    {
        inputActions.Disable();
    }
}
