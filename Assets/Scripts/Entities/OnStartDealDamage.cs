using Entities;
using UnityEngine;

public class OnStartDealDamage : MonoBehaviour
{
    [SerializeField] float damageToDo;

    private void Start()
    {
        this.GetComponent<Entity>().DealDamage(damageToDo);
    }
}
