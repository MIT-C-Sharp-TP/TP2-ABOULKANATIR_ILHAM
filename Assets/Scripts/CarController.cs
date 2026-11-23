using UnityEngine;
using UnityEngine.InputSystem;

// Ce script contrôle la voiture.
// Update lit les touches du clavier.
// FixedUpdate applique les mouvements physiques.
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Driving")]
    [SerializeField] private float acceleration = 6f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float brakeForce = 14f;
    [SerializeField] private float drag = 1.5f;
    [SerializeField] private float turnSpeed = 80f;

    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 4.5f;
    [SerializeField] private float groundCheck = 0.35f;

    private Rigidbody rb;

    private float throttle;
    private float steering;
    private bool braking;
    private bool jumpWanted;
    private float speed;

    // Récupération du Rigidbody de la voiture.
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Lecture des commandes du clavier à chaque frame.
    void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
        {
            return;
        }

        // W accélère et S fait reculer la voiture.
        throttle = (keyboard.wKey.isPressed ? 1f : 0f)
                 - (keyboard.sKey.isPressed ? 1f : 0f);

        // D tourne à droite et A tourne à gauche.
        steering = (keyboard.dKey.isPressed ? 1f : 0f)
                 - (keyboard.aKey.isPressed ? 1f : 0f);

        // Left Shift active le frein.
        braking = keyboard.leftShiftKey.isPressed;

        // Mémoriser l'appui sur Espace jusqu'au prochain FixedUpdate.
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            jumpWanted = true;
        }
    }

    // Application des mouvements avec le moteur physique.
    void FixedUpdate()
    {
        float step = Time.fixedDeltaTime;

        if (braking)
        {
            // Réduire progressivement la vitesse jusqu'à zéro.
            speed = Mathf.MoveTowards(speed, 0f, brakeForce * step);
        }
        else if (throttle != 0f)
        {
            // Accélérer dans la direction demandée.
            speed += throttle * acceleration * step;
        }
        else
        {
            // Ralentir progressivement lorsque aucune touche n'est pressée.
            speed = Mathf.MoveTowards(speed, 0f, drag * step);
        }

        // Limiter la vitesse maximale.
        speed = Mathf.Clamp(speed, -maxSpeed * 0.4f, maxSpeed);

        // Calculer la vitesse dans la direction avant de la voiture.
        Vector3 velocity = transform.forward * speed;

        // Conserver la vitesse verticale gérée par la gravité.
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;

        // La voiture tourne uniquement lorsqu'elle est en mouvement.
        if (Mathf.Abs(speed) > 0.3f)
        {
            float angle = steering * Mathf.Sign(speed) * turnSpeed * step;

            rb.MoveRotation(
                rb.rotation * Quaternion.Euler(0f, angle, 0f)
            );
        }

        // Vérifier si un saut est demandé.
        if (jumpWanted)
        {
            jumpWanted = false;

            if (Grounded())
            {
                rb.linearVelocity += Vector3.up * jumpSpeed;
            }
        }
    }

    // Vérifier si le sol se trouve juste sous la voiture.
    private bool Grounded()
    {
        Vector3 from = transform.position + Vector3.up * 0.1f;

        return Physics.Raycast(
            from,
            Vector3.down,
            groundCheck + 0.1f
        );
    }
}
