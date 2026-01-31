using UnityEngine;

public class DishesDeskInteractable : InteractableBaseController
{
    [SerializeField] private GameObject[] dishView;

    private int dishCount = 0;

    public void AddDish()
    {
        if(dishCount < dishView.Length)
        {
            dishView[dishCount].SetActive(true);
            dishCount++;
        }
    }

    public void RemoveDish()
    {
        if(dishCount > 0)
        {
            dishCount--;
            dishView[dishCount].SetActive(false);
        }
    }

    public override void Hover(bool active, PlayerController playerController = null)
    {
        // TODO: implement This Method
    }

    public override void Interact(PlayerController playerController = null)
    {
        RemoveDish();
    }

    public int GetDishCount()
    {
        return dishCount;
    }
}
