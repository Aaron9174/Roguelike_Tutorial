public class FoodObject : CellObject
{
    public int foodAmount = 2;

    public override bool playerWantsToEnter()
    {
        GameManager.mInstance.changeFood(foodAmount);
        return true;
    }

    public override void playerEntered()
    {
        Destroy(gameObject);
    }
}