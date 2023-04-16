using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Canvas))]
[DisallowMultipleComponent]
public class UIController : Singleton<UIController>
{
    [Header("初始化界面")]
    [SerializeField]
    private Page initialPage;
    [SerializeField]
    private GameObject firstFocusItem;
    private Canvas rootCanvas;
    private Stack<Page> pageStack = new Stack<Page>();

    public override void Awake()
    {
        rootCanvas = GetComponent<Canvas>();
        base.Awake();
    }
    // Start is called before the first frame update
    private void Start()
    {
        // if (firstFocusItem != null)
        // {
        //     EventSystem.current.SetSelectedGameObject(firstFocusItem);
        // }
        // if (initialPage != null)
        // {
        //     PushPage(initialPage);
        // }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(pageStack.Count > 0)
            {
                OnCancel();
            }else{
                if (initialPage != null)
                {
                    PushPage(initialPage);
                }
            }
        }
    }
    /// <summary>
    /// 绑定InputSystem，绑定后按下指定按键时调用
    /// </summary>
    private void OnCancel()
    {
        if(rootCanvas.enabled && rootCanvas.gameObject.activeInHierarchy)
        {
            if(pageStack.Count>0){
                PopPage();
            }
        }
    }
    public bool IsPageInStack(Page page)
    {
        return pageStack.Contains(page);
    }
    
    public bool IsPageOnTopStack(Page page)
    {
        return pageStack.Count>0 && pageStack.Peek() == page;
    }
    public void PushPage(Page page)
    {
        page.Enter(true);
        if (pageStack.Count > 0)
        {
            Page currentPage = pageStack.Peek();
            if (currentPage.exitOnNewPagePush)
            {
                currentPage.Exit(false);
            }
        }
        pageStack.Push(page);
    }

    /// <summary>
    /// 弹出栈顶的页面
    /// </summary>
    public void PopPage()
    {
        if (pageStack.Count > 1)
        {
            Page page = pageStack.Pop();
            page.Exit(true);
            Page newCurrentPage = pageStack.Peek();
            if(newCurrentPage.exitOnNewPagePush)
            {
                newCurrentPage.Enter(true);
            }
        }else
        {
            Debug.LogWarning("只有最后一个页面了");
            Page page = pageStack.Pop(); 
            page.Exit(true);
        }
    }

    public void PopAllPage()
    {
        for (int i = 1; i < pageStack.Count; i++)
        {
            PopPage();
        }
    }
}
