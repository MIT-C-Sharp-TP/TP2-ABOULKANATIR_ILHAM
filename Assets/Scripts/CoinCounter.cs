using UnityEngine;

// Ce script compte les pièces collectées.
// Il écoute l'événement Coin.Collected sans dépendre directement des pièces.
public class CoinCounter : MonoBehaviour
{
    private int coins;          // Nombre de pièces déjà collectées.
    private int coinsToWin;     // Nombre total de pièces dans la scène au départ.

    void OnEnable()
    {
        // S'abonner à l'événement déclenché par une pièce collectée.
        Coin.Collected += AddCoin;
    }

    void OnDisable()
    {
        // Se désabonner pour éviter les doubles comptages lors des redémarrages.
        Coin.Collected -= AddCoin;
    }

    void Start()
    {
        // Compter toutes les pièces présentes dans la scène au démarrage.
        coinsToWin = FindObjectsByType<Coin>(FindObjectsSortMode.None).Length;

        // Afficher le nombre total de pièces à collecter.
        Debug.Log($"Collect the {coinsToWin} coins!");
    }

    private void AddCoin()
    {
        // Incrémenter le compteur après chaque collecte.
        coins++;

        // Afficher le score actuel.
        Debug.Log($"Coins: {coins} / {coinsToWin}");

        // Vérifier si toutes les pièces ont été collectées.
        if (coins == coinsToWin)
        {
            Debug.Log("All coins collected!");
        }
    }
}