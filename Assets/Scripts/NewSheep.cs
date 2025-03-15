using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewSheep : MonoBehaviour
{
    public GameObject[] sheepPrefabs;    // Array of different sheep prefabs
    public float[] rarityWeights;        // Probability weights for each sheep type
    public Transform spawnPoint;         // Where new sheep will appear
    public Button spawnButton;           // Reference to the spawn button
    public TextMeshProUGUI sheepCounter; // Text to display sheep count
    public int sheepMax = 20;

    private int sheepCount = 0; // Current number of sheep

    void Start()
    {
        // Validate that rarity weights match the sheep prefab count
        if (sheepPrefabs.Length != rarityWeights.Length)
        {
            Debug.LogError("Sheep prefabs and rarity weights must be the same length!");
            return;
        }

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
        if (sheepCount >= sheepMax)
        {
            Debug.Log("Sheep Count Reached");
            return;
        }

        GameObject selectedSheep = SelectSheepByRarity();
        if (selectedSheep != null && spawnPoint != null)
        {
            // Instantiate the selected sheep type
            GameObject newSheep = Instantiate(selectedSheep, spawnPoint.position, Quaternion.identity);

            // Add a small random force to prevent stacking
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

    // Selects a sheep prefab based on rarity weights
    private GameObject SelectSheepByRarity()
    {
        float totalWeight = 0f;
        foreach (float weight in rarityWeights)
        {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < sheepPrefabs.Length; i++)
        {
            cumulativeWeight += rarityWeights[i];
            if (randomValue < cumulativeWeight)
            {
                return sheepPrefabs[i];
            }
        }

        return sheepPrefabs[sheepPrefabs.Length - 1]; // Fallback (should never happen)
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
}
