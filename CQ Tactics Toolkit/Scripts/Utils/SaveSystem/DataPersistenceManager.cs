using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
// using PixelCrushers.DialogueSystem;

namespace CQFramework.SaveSystem
{
    public class DataPersistenceManager : MonoBehaviour
    {
        private GameData gameData;
        [Header("需要保存数据的游戏物体")]
        [SerializeField]
        private List<IDataPersistence> dataPersistenceObjects = new List<IDataPersistence>();
        [Header("存档文件路径")]
        [SerializeField]
        private string saveFilePath;
        //当前存档索引
        private int currentDataIndex;
        public List<GameData> dataSlots = new List<GameData>(new GameData[3]);
        public static DataPersistenceManager instance { get; private set; }
        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("已经存在一个DataPersistenceManager");
            }
            instance = this;
            saveFilePath = Application.persistentDataPath + "/SAVE DATA/";
        }

        // private void Start() 
        // {
        //     dataPersistenceObjects = FindAllDataPersistenceObjects();
        //     LoadGame(1);
        // }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                SaveGame(0);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadGame(0);
            }
        }

        public void RegisterDataPersistence(IDataPersistence dataPersistence)
        {
            if (!dataPersistenceObjects.Contains(dataPersistence))
            {
                dataPersistenceObjects.Add(dataPersistence);
            }
        }
        /// <summary>
        /// 新游戏
        /// </summary>
        public void NewGame(out GameData gameData)
        {
            gameData = new GameData();
            // DialogueManager.ResetDatabase(DatabaseResetOptions.RevertToDefault);
        }

        /// <summary>
        /// 加载游戏
        /// </summary>
        public void LoadGame(int saveIndex)
        {
            currentDataIndex = saveIndex;
            var resultPath = saveFilePath + "save" + saveIndex + ".sav";
            GameData loadGameData = null;
            if (File.Exists(resultPath))
            {
                try
                {
                    var stringData = File.ReadAllText(resultPath);
                    loadGameData = JsonConvert.DeserializeObject<GameData>(stringData);
                    // JsonUtility.FromJsonOverwrite(stringData,loadGameData);
                }
                catch (System.Exception e)
                {
                    Debug.Log($"尝试读取文件:{resultPath}数据出错。\n{e}");
                }
            };
            if (loadGameData == null)
            {
                Debug.Log($"没有{saveIndex}号存档");
                NewGame(out loadGameData);
            }
            // 加载游戏
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.ILoadData(loadGameData);
            }
            // dataSlots[saveIndex].dataSlotDic[saveIndex] = gameData;
        }

        public void SaveGame(int saveIndex)
        {
            gameData = new GameData();
            // 保存游戏
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.ISaveData(ref gameData);
            }
            dataSlots[saveIndex] = gameData;
            var resultPath = saveFilePath + "save" + saveIndex + ".sav";
            var jsonData = JsonConvert.SerializeObject(dataSlots[saveIndex], Formatting.Indented);
            //不存在文件则先创建文件夹
            if (!File.Exists(resultPath))
            {
                Directory.CreateDirectory(saveFilePath);
            }
            // Debug.Log("DATA" + saveIndex + "SAVED!");
            File.WriteAllText(resultPath, jsonData);
            Debug.Log(resultPath);
        }

        /// <summary>
        /// 读取所有存档的游戏数据
        /// </summary>
        private void ReadGameData()
        {
            if (Directory.Exists(saveFilePath))
            {
                for (int i = 0; i < dataSlots.Count; i++)
                {
                    var resultPath = saveFilePath + "save" + i + ".sav";
                    if (File.Exists(resultPath))
                    {
                        var stringData = File.ReadAllText(resultPath);
                        var jsonData = JsonConvert.DeserializeObject<GameData>(stringData);
                        dataSlots[i] = jsonData;
                    }
                }
            }
        }
        //退出自动保存
        private void OnApplicationQuit()
        {
            SaveGame(currentDataIndex);
        }

        /// <summary>
        /// 获取所有要保存数据的物体
        /// </summary>
        /// <returns></returns>
        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
            return new List<IDataPersistence>(dataPersistenceObjects);
        }
    }
}
