using UnityEngine;
public class SheepMerge : MonoBehaviour
{
    public GameObject mergedSheepPrefab;
    public ParticleSystem mergeEffect;
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
            SheepMovement myMovement = GetComponent<SheepMovement>();
            SheepMovement otherMovement = collision.gameObject.GetComponent<SheepMovement>();
            
            // Don't merge if either sheep is being dragged
            if (myMovement != null && myMovement.isBeingDragged) return;
            if (otherMovement != null && otherMovement.isBeingDragged) return;
            
            // Only merge if both sheep can merge
            if (otherSheep != null && otherSheep.canMerge && Random.value > 0.5f)
            {
                // Set both sheep to non-mergeable to prevent chain reactions
                canMerge = false;
                otherSheep.canMerge = false;
                
                // Spawn merge effect
                if (mergeEffect != null)
                {
                    Instantiate(mergeEffect, transform.position, Quaternion.identity);
                }
                
                // Create merged sheep at midpoint between the two
                Vector3 midPoint = (transform.position + collision.transform.position) / 2;
                GameObject newSheep = Instantiate(mergedSheepPrefab, midPoint, Quaternion.identity);
                
                // Update sheep count: -2 for destroyed sheep, +1 for new sheep = net -1
                if (sheepManager != null)
                {
                    sheepManager.UpdateSheepCount(-1);
                }
                
                // Destroy both original sheep
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
            else
            {
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