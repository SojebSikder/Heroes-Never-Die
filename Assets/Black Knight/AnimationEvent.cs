using UnityEngine;
using System;
using System.Collections;

//Just for demonstration, you can replace it with your own code logic.
public class AnimationEvent : MonoBehaviour
{

	public GameObject enemy;
	public int attackDamage = 20;

	private int atkTimes = 0;

	public void AttackStart()
	{
		// Debug.Log ("Attack Start");

		//Just for demonstration, you can replace it with your own code logic.
		// atkTimes++;
		// if (enemy && atkTimes <= 3)
		// {
		// 	Animator enemyAnimator = enemy.GetComponent<Animator>();
		// 	if (atkTimes == 1)
		// 	{
		// 		// enemyAnimator.SetTrigger ("hit_1");
		// 		enemyAnimator.SetTrigger("Hurt");
		// 	}
		// 	else if (atkTimes == 2)
		// 	{
		// 		// enemyAnimator.SetTrigger ("hit_2");
		// 		enemyAnimator.SetTrigger("Hurt");
		// 	}
		// 	else if (atkTimes == 3)
		// 	{
		// 		// enemyAnimator.SetTrigger ("hit_2");
		// 		enemyAnimator.SetTrigger("Hurt");
		// 		enemyAnimator.SetTrigger("Death");
		// 	}
		// }

		PlayerController playerController = enemy.GetComponent<PlayerController>();
		playerController.TakeDamage(attackDamage);
	}

	public void AttackStartEffectObject()
	{
		// Debug.Log ("Fire Effect Object");
	}

}
