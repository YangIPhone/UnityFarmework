using UnityEngine;
using System.Linq;
using CQFramework;

namespace CQFramework.CQTacticsToolkit{
    public class MouseController : MonoBehaviour
    {
        private static MouseController _instance;
        public static MouseController Instance { get { return _instance; } }
        [SerializeField]public OverlayTile focusedOnTile;
        private OverlayTile newFocusedOnTile;
        private Vector3 previousMousePosition;
        private bool mapIsInited = false;
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
        private void OnEnable()
        {
            EventHandler.MapInited += OnMapInited;
        }
        private void OnDisable()
        {
            EventHandler.MapInited -= OnMapInited;
        }
        void Start()
        {   
            // activeCharacter = PlayerTeamContainer.Instance.characters[0];
            previousMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(mapIsInited)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (previousMousePosition != mousePos)
                {
                    newFocusedOnTile = GetFocusedOnTile2D(mousePos);
                    if (newFocusedOnTile!=null)
                    {
                        if (focusedOnTile != newFocusedOnTile)
                        {
                            transform.position = newFocusedOnTile.gridWorldPosition;
                            focusedOnTile = newFocusedOnTile;
                            //TODO 发送鼠标焦点变化事件
                            EventHandler.CallFocusOnNewTile(newFocusedOnTile);
                        }
                    }
                    previousMousePosition = mousePos;
                }
            }
        }
        private void OnMapInited()
        {
            GetComponent<SpriteRenderer>().enabled = true;
            mapIsInited = true;
        }
        //Returns the tile you are currently moused over
        public OverlayTile GetFocusedOnTile2D(Vector3 mousePos)
        {
            Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
            OverlayTile overlayTile = null;
            for(int i=MapManager.Instance.tilemaps.Count;i>=0;i--){
                overlayTile = MapManager.Instance.GetOverlayByTransform(new Vector3(mousePos2d.x, mousePos2d.y, i));
                if(overlayTile!=null) return overlayTile;
            }
            // RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);
            // if (hits.Length > 0)
            // {
            //     var firstHit = hits.OrderByDescending(i => i.collider.transform.position.z).ToList().Find(x=>x.collider.CompareTag("OverlayTile"));
            //     if(firstHit) return firstHit.collider.gameObject.GetComponent<OverlayTile>();
            // }
            return overlayTile;
        }
    }
}
