using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxes : MonoBehaviour
{
    public GameObject dropItem;

    public bool tutorialBoxDestroyed = false;
    public bool tutorial = false;
    public bool tutorialItemDrop = true;


    public int maxHealth = 10;
    public int currentHealth;

    bool boxDamaged;

    

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void DamageBox(int damage)
    {
        currentHealth -= damage; 

        if(currentHealth <= 0)
        {
            if(tutorial == true)
            {
                TutorialBoxDestroyed();
            }
            else
            {
                boxDestroyed();
            }
        }

        if (currentHealth > 0 && currentHealth < maxHealth && boxDamaged == false)
        {
            //TODO: Change sprite to broken box
            boxDamaged = true;            
        }
    }

    void boxDestroyed()
    {
        //TODO: Box Destroy animation or particle effect
        //TODO: Box Destroy sound effect
        //TODO: Chance to drop item
        Destroy(this.gameObject);
    }

    void TutorialBoxDestroyed()
    {
        //TODO: Box Destroy animation or particle effect
        //TODO: Box Destroy sound effect
        if (tutorialItemDrop == true)
        {
            Instantiate(dropItem, this.transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}
