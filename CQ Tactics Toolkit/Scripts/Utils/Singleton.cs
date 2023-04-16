using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:Singleton<T>
{
    private static T instance;
    public static T Instance{get=>instance;}
    // Start is called before the first frame update
    public virtual void Awake() {
        if(instance!=null){
            Debug.LogError("已经存在一个DataPersistenceManager");
        }else{
            instance = this as T;
        }
    }

    private void OnDestroy() {
        if(instance == this){
            instance = null;
        }
    }
}
