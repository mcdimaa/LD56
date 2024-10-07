using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Golem : Unit
{
    [Header("Golem Values")]
    public int attackDamage;
    public float attackRange;
    public Weapon weapon;
    public RtsObject target;

    private void Start()
    {
        StartCoroutine(StartAttacking(GlobalReferences.instance.mainCamp));
    }

    public IEnumerator StartAttacking(RtsObject rtsObject)
    {
        navMeshAgent.SetDestination(rtsObject.transform.position);
        target = rtsObject;
        
        yield return new WaitUntil(() => Vector3.Distance(rtsObject.transform.position, transform.position) < attackRange);

        transform.LookAt(target.transform.position);

        navMeshAgent.SetDestination(transform.position);
        weapon.GetComponent<Animator>().SetBool("attacking", true);

        yield return null;
    }

    public void Attack()
    {
        if (target != null)
        {
            if (target is Fighter)
            {
                Fighter fighter = (Fighter)target;
                if (Vector3.Distance(fighter.transform.position, transform.position) < attackRange)
                {
                    fighter.TakeDamage(attackDamage);
                }
                else
                {
                    // Too far, go back to attacking main camp
                    weapon.GetComponent<Animator>().SetBool("attacking", false);
                    StartCoroutine(StartAttacking(GlobalReferences.instance.mainCamp));
                }
                if (fighter == null) // Enemy is dead
                {
                    // Stop swinging
                    weapon.GetComponent<Animator>().SetBool("attacking", false);
                }
            }
            else if (target is Building)
            {
                Building building = (Building)target;
                if (Vector3.Distance(building.transform.position, transform.position) < attackRange)
                {
                    building.TakeDamage(attackDamage);
                }
                if (building == null) // Enemy is dead
                {
                    // Stop swinging
                    weapon.GetComponent<Animator>().SetBool("attacking", false);
                }
            }
        }
        else
        {
            // Stop swinging
            weapon.GetComponent<Animator>().SetBool("attacking", false);
        }
    }

    public void TakeDamage(int amount, Creature attacker)
    {
        if (health - amount <= 0)
        {
            Die();
        }
        else
        {
            health -= amount;
            StartCoroutine(StartAttacking(attacker));
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
