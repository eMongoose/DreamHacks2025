using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewSheep : MonoBehaviour
{
    public GameObject sheepPrefab;       // The sheep prefab to spawn
    public Transform spawnPoint;         // Where new sheep will appear
    public Button spawnButton;           // Reference to the spawn button
    public TextMeshProUGUI sheepCounter; // Text to display sheep count
    
    private int sheepCount = 0;          // Current number of sheep
    
    void Start()
    {
        // Add click listener to the spawn button
        if (spawnButton != null)
        {
            spawnButton.onClick.AddListener(SpawnNewSheep);
        }
        
        // Initialize the counter
        UpdateSheepCounter();
        
        // Count any sheep that might already exist in the scene
        CountExistingSheep();
    }
    
    // Spawn a new sheep at the spawn point
    public void SpawnNewSheep()
    {
        if (sheepCount >= 20)
        {
            Debug.Log("Sheep Count Reached");
            return;
        }
        
        if (sheepPrefab != null && spawnPoint != null)
        {
            // Instantiate new sheep at spawn point
            GameObject newSheep = Instantiate(sheepPrefab, spawnPoint.position, Quaternion.identity);
            
            // Add a small random force to prevent sheep from stacking perfectly
            Rigidbody rb = newSheep.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 randomForce = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                rb.AddForce(randomForce, ForceMode.Impulse);
            }
            
            // Increment counter
            sheepCount++;
            UpdateSheepCounter();
        }
    }
    
    // Count existing sheep in the scene
    private void CountExistingSheep()
    {
        GameObject[] existingSheep = GameObject.FindGameObjectsWithTag("Sheep");
        sheepCount = existingSheep.Length;
        UpdateSheepCounter();
    }
    
    // Update the UI counter
    private void UpdateSheepCounter()
    {
        if (sheepCounter != null)
        {
            sheepCounter.text = "Sheep: " + sheepCount;
        }
    }
    
    // This method should be called when sheep are merged or destroyed
    public void UpdateSheepCount(int change)
    {
        sheepCount += change;
        UpdateSheepCounter();
    }
    
    // Alternatively, we can use this method to recount all sheep
    public void RecountAllSheep()
    {
        CountExistingSheep();
    }
}