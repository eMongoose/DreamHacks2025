using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPanel;  // Assign your UI Panel
    public TextMeshProUGUI popupText; // Assign TMP Text
    public Button closeButton;  // Assign your close button

    private void Start()
    {
        popupPanel.SetActive(false); // Ensure it's hidden at start
        closeButton.onClick.AddListener(ClosePopup);
        StartCoroutine(ShowPopupRoutine()); // Start the timer loop
    }

    private IEnumerator ShowPopupRoutine()
    {
        while (true)  // Infinite loop for recurring popups
        {
            yield return new WaitForSeconds(30f);  // Wait 30 seconds
            ShowPopup("Pay Us.");  // Message to display
        }
    }

    public void ShowPopup(string message)
    {
        popupText.text = message;
        popupPanel.SetActive(true);
        Time.timeScale = 0f;  // Pause game
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        Time.timeScale = 1f;  // Resume game
    }
}