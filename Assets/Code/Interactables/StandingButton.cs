using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody))]
public class StandingButton : MonoBehaviour
{
    public bool IsButtonActive => ObjectsOnButton > 0;
    [SerializeField] private GameObject Assigneddoor;
    private int ObjectsOnButton;
    private Door DoorScriptRefrence;
    private Collider2D Colli;
    private Rigidbody Rigid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Colli = GetComponent<BoxCollider2D>();
        Rigid = GetComponent<Rigidbody>();
        Colli.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer != 0)
        {
            ObjectsOnButton++;
            if(ObjectsOnButton > 0)
            {
                DoorScriptRefrence.UpdateDoor(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
            if (other.gameObject.layer != 0)
            {
                ObjectsOnButton --;
                if (ObjectsOnButton <= 0)
                {
                    DoorScriptRefrence.UpdateDoor(false);
                }
                ObjectsOnButton = 0;
            }
    }
}
