using System;
using UnityEngine;

// Script qui permet à la pièce de tourner et d'être collectée par la voiture.
public class Coin : MonoBehaviour
{
    // Événement statique déclenché lorsqu'une pièce est collectée.
    public static event Action Collected;

    // Vitesse de rotation de la pièce, en degrés par seconde.
    [SerializeField] private float rotationSpeed = 90f;

    // Effet visuel facultatif lors de la collecte.
    [SerializeField] private GameObject collectEffect;

    void Update()
    {
        // Fait tourner la pièce autour de son axe vertical.
        // Time.deltaTime garantit une vitesse indépendante du nombre d'images par seconde.
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        // Seule la voiture portant le tag Player peut collecter la pièce.
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Instancie l'effet visuel uniquement s'il a été configuré.
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }

        // Informe les objets qui écoutent l'événement qu'une pièce a été collectée.
        Collected?.Invoke();

        // Supprime la pièce de la scène.
        Destroy(gameObject);
    }
}