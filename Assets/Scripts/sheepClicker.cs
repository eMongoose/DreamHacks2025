using UnityEngine;

public class Sheep : MonoBehaviour
{
    // Drag your Sell Button GameObject into this field in the Inspector.
    public GameObject sellButton;

    void Start()
    {
        // Hide the "Sell" button at the start (no null-check).
        sellButton.SetActive(false);
    }

    void OnMouseDown()
    {
        // When clicked, show the button (no null-check).
        sellButton.SetActive(true);
    }
}