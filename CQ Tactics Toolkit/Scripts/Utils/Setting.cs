using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Settings
{
    public const float itemfadeDuration = 0.35f; //场景物体透明花费时间
    public const float fadeDuration = 0.5f;//加载面板透明切换时间
    public const float secondThreshold = 0.1f;//显示secondThreshold等于游戏1秒     
    public const int secondHold = 59;//一分钟60秒(0-59)
    public const int minuteHold = 59;//0-59
    public const int hourHold = 23;//0-23
    public const int dayHold = 30;//一个月30天
    public const int seasonHold = 3;//一年4个季节(0-3)
    public const int 初始移动范围 = 50;
    public const int 神识增强使用范围 = 100;
    public const int 身法增强移动范围 = 25;
    // public const KeyCode[] shortcutKeyArr = new KeyCode[1]{KeyCode.Alpha1};
    public static string[] stringHourHold = new string[24] { "子时", "丑时", "丑时", "寅时", "寅时", "卯时", "卯时", "辰时", "辰时", "巳时", "巳时", "午时", "午时", "未时", "未时", "申时", "申时", "酉时", "酉时", "戌时", "戌时", "亥时", "亥时", "子时" };
}
