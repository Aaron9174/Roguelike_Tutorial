using UnityEngine;

public class FoodObject : CellObject
{
    public override void playerEntered()
    {
        Destroy(gameObject);

        // TODO: make this actually increase the food amount
        Debug.Log("Increase food..");
    }
}