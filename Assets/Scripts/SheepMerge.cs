using UnityEngine;
public class SheepMerge : MonoBehaviour
{
    public GameObject mergedSheepPrefab;
    public AudioSource mergeFailSound;
    private bool canMerge = true;   
    private float mergeCooldown = 0.5f;
    private NewSheep sheepManager;
    
    private void Start()
    {
        // Find the SheepManager in the scene
        sheepManager = FindObjectOfType<NewSheep>();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!canMerge) return;
        
        if (collision.gameObject.CompareTag("Sheep"))
        {
            SheepMerge otherSheep = collision.gameObject.GetComponent<SheepMerge>();
            SheepKill myKill = GetComponent<SheepKill>();
            SheepKill otherKill = collision.gameObject.GetComponent<SheepKill>();
            
            // Don't merge if either sheep is being dragged
            if (myKill != null && myKill.isBeingDragged) return;
            if (otherKill != null && otherKill.isBeingDragged) return;
            
            // Only merge if both sheep can merge
            if (otherSheep != null && otherSheep.canMerge && Random.value > 0.5f)
            {
                // Set both sheep to non-mergeable to prevent chain reactions
                canMerge = false;
                otherSheep.canMerge = false;
                
                // Create merged sheep at midpoint between the two
                Vector3 midPoint = (transform.position + collision.transform.position) / 2;
                GameObject newSheep = Instantiate(mergedSheepPrefab, midPoint, Quaternion.identity);
                
                // Update sheep count: -2 for destroyed sheep, +1 for new sheep = net -1
                if (sheepManager != null)
                {
                    sheepManager.UpdateSheepCount(-1);
                    mergeFailSound.Play();
                }
                
                // Destroy both original sheep
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
            else
            {
                // Play merge fail sound if merging fails
                if (mergeFailSound != null)
                {
                    mergeFailSound.Play();
                }

                // Set cooldown on merge to prevent rapid collision checks
                canMerge = false;
                Invoke("ResetCanMerge", mergeCooldown);
            }
        }
    }
    
    private void ResetCanMerge()
    {
        canMerge = true;
    }
}
