using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private enum MovementState { idle, running, jumping, falling, jumping2 }

    [SerializeField] private AudioSource jumpSoundEffect;

    private int jumpCount = 0; // Contador de saltos
    [SerializeField] private int maxJumpCount = 2; // Máximo de saltos permitidos

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        // Verificar si el jugador está tocando el suelo
        if (IsGrounded())
        {
            // Reiniciar el contador de saltos solo si el jugador ha aterrizado
            if (Mathf.Abs(rb.velocity.y) < 0.001f) // Usamos Mathf.Abs para obtener el valor absoluto y un umbral pequeño para la velocidad
            {
                jumpCount = 0;
            }
        }

        // Permitir un salto si el jugador está en el suelo o si aún no ha usado el doble salto
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpSoundEffect.Play();
            jumpCount++; // Incrementar el contador de saltos
        }

        UpdateAnimationState();
    }

    private bool IsGrounded()
    {
        // Utilizamos un pequeño 'raycast' hacia abajo desde el centro del 'collider' para verificar si el jugador está en el suelo
        RaycastHit2D raycastHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f);
        return raycastHit.collider != null;
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        // Si el jugador está yendo hacia arriba, entonces está saltando
        if (rb.velocity.y > .1f)
        {
            // Si jumpCount es 1, entonces es el primer salto
            if (jumpCount == 1)
            {
                state = MovementState.jumping;
            }
            // Si jumpCount es mayor que 1, entonces es un doble salto
            else if (jumpCount > 1)
            {
                state = MovementState.jumping2;
            }
        }
        // Si el jugador está yendo hacia abajo, entonces está cayendo
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        // Cambiar la animación basada en el estado del movimiento
        anim.SetInteger("state", (int)state);
    }

}
