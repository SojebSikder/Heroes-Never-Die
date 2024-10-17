using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private string checkpointName;
    public Vector3 checkpointOffset;
    public float checkpointRange = 2.75f;
    public LayerMask checkpointMask;

    // Start is called before the first frame update
    void Start()
    {
        checkpointName = gameObject.name;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos += transform.right * checkpointOffset.x;
        pos += transform.up * checkpointOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, checkpointRange, checkpointMask);

        if (colInfo != null)
        {

            if (checkpointName == "Checkpoint_1")
            {
                GameManager.Instance.GoLevel2();
            }

            if (checkpointName == "Checkpoint_2")
            {
                Destroy(gameObject);
                // stop the player from moving
                GameObject.Find("Player").GetComponent<PlayerController>().enabled = false;
                GameObject.Find("Player").GetComponent<Animator>().enabled = false;

                // start the conversation
                GameObject.Find("SojebHumayra_Conversation").GetComponent<DialogueTrigger>().TriggerDialogue(true);
            }
            // colInfo.GetComponent<PlayerController>().TakeDamage(checkpointDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * checkpointOffset.x;
        pos += transform.up * checkpointOffset.y;

        Gizmos.DrawWireSphere(pos, checkpointRange);
    }
}
