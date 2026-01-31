using System;
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

    public bool RemoveDish(int dishesRemoved = 1)
    {
        if(dishCount >= dishesRemoved)
        {
            dishCount -= dishesRemoved;
            for(int i = dishView.Length - 1; i >= dishCount; i--)
            {
                dishView[i].SetActive(false);
            }

            return true;
        }

        return false;
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
