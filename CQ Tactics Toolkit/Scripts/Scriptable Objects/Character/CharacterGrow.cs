using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CQTacticsToolkit
{
    //生成包含角色类别和等级的角色属性。
    [CreateAssetMenu(fileName = "CharacterGrow", menuName = "CQ Tactics Toolkit/Character/CharacterGrow", order = 1)]
    public class CharacterGrow : ScriptableObject{

        [Header("生命值成长")]
        public AnimationCurve HealthGrow = new AnimationCurve(new Keyframe(0f,0f),
        new Keyframe(1f, 10f), 
        new Keyframe(2f, 25f), 
        new Keyframe(3f, 45f), 
        new Keyframe(4f, 75f), 
        new Keyframe(5f, 110f), 
        new Keyframe(6f, 150f), 
        new Keyframe(7f, 195f), 
        new Keyframe(8f, 245f), 
        new Keyframe(9f, 300f), 
        new Keyframe(10f, 400f));
        [Header("灵力值成长")]
        public AnimationCurve ManaGrow = new AnimationCurve(new Keyframe(0f, 0f),
        new Keyframe(1f, 5f),
        new Keyframe(2f, 25f),
        new Keyframe(3f, 60f),
        new Keyframe(4f, 110f),
        new Keyframe(5f, 150f),
        new Keyframe(6f, 195f),
        new Keyframe(7f, 240f),
        new Keyframe(8f, 300f),
        new Keyframe(9f, 380f),
        new Keyframe(10f, 500f));
        [Header("攻击力成长")]
        public AnimationCurve StrenghtGrow = new AnimationCurve(new Keyframe(0f, 0f),
        new Keyframe(1f, 5f),
        new Keyframe(2f, 10f),
        new Keyframe(3f, 15f),
        new Keyframe(4f, 25f),
        new Keyframe(5f, 40f),
        new Keyframe(6f, 60f),
        new Keyframe(7f, 80f),
        new Keyframe(8f, 100f),
        new Keyframe(9f, 125f),
        new Keyframe(10f, 175f));
        [Header("防御力成长")]
        public AnimationCurve EnduranceGrow = new AnimationCurve(new Keyframe(0f, 0f),
        new Keyframe(1f, 5f),
        new Keyframe(2f, 10f),
        new Keyframe(3f, 15f),
        new Keyframe(4f, 25f),
        new Keyframe(5f, 40f),
        new Keyframe(6f, 60f),
        new Keyframe(7f, 80f),
        new Keyframe(8f, 100f),
        new Keyframe(9f, 125f),
        new Keyframe(10f, 175f));
        [Header("速度成长")]
        public AnimationCurve SpeedGrow = AnimationCurve.EaseInOut(0f, 1f, 10f, 100f);
    }

}
