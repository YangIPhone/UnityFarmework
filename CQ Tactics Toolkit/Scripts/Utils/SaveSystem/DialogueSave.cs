using UnityEngine;
// using PixelCrushers.DialogueSystem;
using CQFramework.SaveSystem;

public class DialogueSave : MonoBehaviour, IDataPersistence
{
    private void Start()
    {
        IDataPersistence dataPersistence = this;
        dataPersistence.RegisterDataPersistence();
    }
    void IDataPersistence.ILoadData(GameData gameData)
    {
        // PersistentDataManager.ApplySaveData(gameData.DialogueData);
    }

    void IDataPersistence.ISaveData(ref GameData gameData)
    {
        // gameData.DialogueData = PersistentDataManager.GetSaveData();
    }
}