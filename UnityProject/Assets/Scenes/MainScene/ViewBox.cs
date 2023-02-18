using UnityEngine;

public class ViewBox : MonoBehaviour
{
    public GameObject child
    {
        get
        {
            return _child;
        }
        set
        {
            Destroy(_child);
            _child = value;

            _child.transform.SetParent(this.transform);
        }
    }

    private GameObject _child;
}

