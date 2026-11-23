using UnityEngine;

// Cette caméra suit la voiture.
// LateUpdate s'exécute après Update pour suivre la position finale de la voiture.
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 offset =
        new Vector3(0f, 2.2f, -4.5f);

    [SerializeField] private float smoothing = 5f;

    void LateUpdate()
    {
        // Vérifier que la cible est bien définie.
        if (target == null)
        {
            return;
        }

        // Calculer la position souhaitée derrière la voiture.
        Vector3 wanted = target.position +
                         target.TransformDirection(offset);

        // Déplacer progressivement la caméra.
        transform.position = Vector3.Lerp(
            transform.position,
            wanted,
            smoothing * Time.deltaTime
        );

        // Orienter la caméra vers la voiture.
        transform.LookAt(target.position + Vector3.up * 0.3f);
    }
}