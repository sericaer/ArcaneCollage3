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

            if(_child != null)
            {
                _child.transform.SetParent(this.transform);
            }
        }
    }

    private GameObject _child;

    public T CreateInstance<T>(T obj)
        where T:MonoBehaviour
    {
        if (this.child != null)
        {
            var curr = this.child.GetComponent<T>();
            if (curr != null)
            {
                return curr;
            }
            else
            {
                child = null;
            }
        }

        var instance = Instantiate<T>(obj);
        child = instance.gameObject;
        return instance;
    }
}

