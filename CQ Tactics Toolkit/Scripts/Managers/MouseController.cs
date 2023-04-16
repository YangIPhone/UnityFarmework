using UnityEngine;
using System.Linq;
using CQFramework;

namespace CQTacticsToolkit{
    public class MouseController : MonoBehaviour
    {
        private static MouseController _instance;
        public static MouseController Instance { get { return _instance; } }
        [SerializeField]public OverlayTile focusedOnTile;
        private OverlayTile newFocusedOnTile;
        private Vector3 previousMousePosition;

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

        void Start()
        {   
            GetComponent<SpriteRenderer>().enabled = true;
            previousMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (previousMousePosition != mousePos)
            {
                newFocusedOnTile = GetFocusedOnTile2D(mousePos);
                if (newFocusedOnTile)
                {
                    if (focusedOnTile != newFocusedOnTile)
                    {
                        transform.position = newFocusedOnTile.transform.position;
                        focusedOnTile = newFocusedOnTile;
                        //TODO 发送鼠标焦点变化事件
                        EventHandler.CallFocusOnNewTile(newFocusedOnTile);
                    }
                }
                previousMousePosition = mousePos;
            }
        }

        //Returns the tile you are currently moused over
        public OverlayTile GetFocusedOnTile2D(Vector3 mousePos)
        {
            Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);
            if (hits.Length > 0)
            {
                var firstHit = hits.OrderByDescending(i => i.collider.transform.position.z).ToList().Find(x=>x.collider.CompareTag("OverlayTile"));
                if(firstHit) return firstHit.collider.gameObject.GetComponent<OverlayTile>();
            }
            return null;
        }
    }
}
