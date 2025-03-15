using TMPro;
using UnityEngine;

public class MoneyCollision : MonoBehaviour
{
    
    public TextMeshProUGUI text;
    
    private int sheepCrossCount = 0;

    private void OnTriggerEnter(Collider collision)
    {
        // Check if the object entering the trigger is a sheep
        if (collision.CompareTag("Sheep"))
        {
            // This counts as one crossing
            sheepCrossCount++;
            text.text = ($"${sheepCrossCount}");
            Debug.Log("Sheep crossed the fence! Total crossings: " + sheepCrossCount);
        }
    }
    
    
    
}
