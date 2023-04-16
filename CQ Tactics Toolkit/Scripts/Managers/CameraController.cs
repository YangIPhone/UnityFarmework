using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQFramework;
namespace CQTacticsToolkit{
    public class CameraController : MonoBehaviour
    {

        private static CameraController _instance;
        public static CameraController Instance { get { return _instance; } }
        [Header("自由漫游模式")]
        [SerializeField] private bool roamMode;
        [SerializeField] private float roamSpeed=20f;
        [SerializeField] private Transform followTarget;
        [SerializeField] private float offset = 0.1f;
        [SerializeField] private float smoothOffset = 0.05f;
        [SerializeField] private bool isShake = false;
        private Vector3 _originalPos;
        private float _timeAtCurrentFrame;
        private float _timeAtLastFrame;
        private float _fakeDelta;
        public void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
        private void OnEnable() {
            EventHandler.StartNewCharacterTurn += OnStartNewCharacterTurn;
        }
        private void OnDisable() {
            EventHandler.StartNewCharacterTurn -= OnStartNewCharacterTurn;
        }
        void Update()
        {
            //计算一个假的增量时间，这样我们就可以在游戏暂停时Shake。
            _timeAtCurrentFrame = Time.realtimeSinceStartup;
            _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
            _timeAtLastFrame = _timeAtCurrentFrame;
            if((Input.GetAxis("Horizontal")!=0||Input.GetAxis("Vertical")!=0))
            {
                roamMode = TurnBasedController.Instance.isBattleing;
            }
            if(Input.GetKeyDown(KeyCode.Space))
            {
                roamMode = false;
            }
        }

        void LateUpdate()
        {
            if (!roamMode&&followTarget!=null&&!isShake&&(transform.position - followTarget.position).sqrMagnitude > offset * offset)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(followTarget.position.x, followTarget.position.y, transform.position.z), smoothOffset);
            }else{
                transform.position = new Vector3(transform.position.x + Input.GetAxis("Horizontal") * roamSpeed * Time.deltaTime,
                transform.position.y + Input.GetAxis("Vertical") * roamSpeed * Time.deltaTime, transform.position.z);
            }
        }

        /// <summary>
        /// 相机抖动
        /// </summary>
        /// <param name="duration">抖动持续时间</param>
        /// <param name="amount"></param>
        public static void Shake(float duration, float amount)
        {
            Instance._originalPos = Instance.gameObject.transform.localPosition;
            Instance.StopAllCoroutines();
            Instance.StartCoroutine(Instance.cShake(duration, amount));
        }

        public IEnumerator cShake(float duration, float amount)
        {
            isShake = true;
            while (duration > 0)
            {
                transform.localPosition = _originalPos + Random.insideUnitSphere * amount;
                duration -= _fakeDelta;
                yield return null;
            }
            isShake = false;
            // transform.localPosition = _originalPos;
        }

        /// <summary>
        /// 获取当前跟随目标
        /// </summary>
        public Transform GetFollowTarget()
        {
            return followTarget;
        }

        public void SetFollowTarget(Transform target)
        {
            followTarget = target;
        }
        private void OnStartNewCharacterTurn(Character newCharacter)
        {
            followTarget = newCharacter.transform;
            roamMode=false;
        }
    }
}
