using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class Fighter : Creature
{
    [Header("Fighter Values")]
    public int attackDamage;
    public float attackRange;
    public Weapon weapon;
    public RtsObject target;

    public IEnumerator StartAttacking(RtsObject rtsObject)
    {
        while (Vector3.Distance(rtsObject.transform.position, transform.position) > attackRange / 2 + (rtsObject.transform.lossyScale.x / 2))
        {
            navMeshAgent.SetDestination(rtsObject.transform.position);
            yield return new WaitForSeconds(0.5f);
        }
        target = rtsObject;

        yield return new WaitUntil(() => Vector3.Distance(rtsObject.transform.position, transform.position) < attackRange / 2 + (rtsObject.transform.lossyScale.x / 2));

        transform.LookAt(target.transform.position);

        navMeshAgent.SetDestination(transform.position);
        weapon.GetComponent<Animator>().SetBool("attacking", true);

        yield return null;
    }

    public void Attack()
    {
        if (target != null)
        {
            transform.LookAt(target.transform.position);

            if (target is Golem)
            {
                Golem golem = (Golem)target;
                if (Vector3.Distance(golem.transform.position, transform.position) < attackRange + (golem.transform.lossyScale.x / 2))
                {
                    golem.TakeDamage(attackDamage, this);
                }
                if (golem == null) // Enemy is dead
                {
                    // Stop swinging
                    weapon.GetComponent<Animator>().SetBool("attacking", false);
                }
            }
            else if (target is Building)
            {
                Building building = (Building)target;
                if (Vector3.Distance(building.transform.position, transform.position) < attackRange + (building.transform.lossyScale.x / 2))
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

    public void TakeDamage(int amount)
    {
        if (health - amount <= 0)
        {
            Die();
        }
        else
        {
            health -= amount;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public override void CheckAction()
    {
        // If unit is selected
        if (UnitSelection.instance.selectedObjects.Contains(this))
        {
            // If the action key has been pressed
            if (Input.GetKeyDown(Keybinds.instance.actionKey))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Worker creatures have multiple actions
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, GlobalReferences.instance.fighterActionMask))
                {
                    // Clicked on a unit
                    if ((GlobalReferences.instance.unitMask.value & (1 << hit.transform.gameObject.layer)) != 0)
                    {
                        // If unit is a golem
                        if (hit.transform.GetComponent<Golem>())
                        {
                            // Start attacking it
                            StartCoroutine(StartAttacking(hit.transform.GetComponent<Golem>()));

                            Vector3 pos = hit.transform.position;
                            pos.y += 1;
                            UnitSelection.instance.ShowAttackIndicator(hit.transform);
                        }
                    }
                    // Clicked on a building, attack it
                    else if ((GlobalReferences.instance.buildingMask.value & (1 << hit.transform.gameObject.layer)) != 0)
                    {
                        // Start attacking it
                        StartCoroutine(StartAttacking(hit.transform.GetComponent<Building>()));

                        Vector3 pos = hit.transform.position;
                        pos.y += 3;
                        UnitSelection.instance.ShowAttackIndicator(pos);
                    }
                    // Clicked on the ground, move to that location
                    else if ((GlobalReferences.instance.groundMask.value & (1 << hit.transform.gameObject.layer)) != 0)
                    {
                        // Disable the weapon's animation
                        weapon.GetComponent<Animator>().SetBool("attacking", false);

                        StopCoroutine(StartAttacking(target));
                        target = null;

                        MoveTo(hit.point);
                        UnitSelection.instance.ShowMoveLocationIndicator(hit.point);
                    }
                }
            }
        }
    }
}
