public class FoodObject : CellObject
{
    public int foodAmount = 2;

    public override void playerEntered()
    {
        Destroy(gameObject);

        GameManager.mInstance.changeFood(foodAmount);
    }
}