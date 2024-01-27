using UnityEngine;
using Phezu.Util;
using FMODUnity;

[DefaultExecutionOrder(-1)]
public class FModEvents : Singleton<FModEvents> {
    [field: SerializeField] public EventReference Ambience { get; private set; }
    [field: SerializeField] public EventReference MainMenuTheme { get; private set; }
    [field: SerializeField] public EventReference BackgroundMusic { get; private set; }
    [field: SerializeField] public EventReference NormalAttack { get; private set; }
    [field: SerializeField] public EventReference SpecialAttack { get; private set; }
    [field: SerializeField] public EventReference CoolDownNormal { get; private set; }
    [field: SerializeField] public EventReference CoolDownSpecial { get; private set; }
    [field: SerializeField] public EventReference HitTargetNormal { get; private set; }
    [field: SerializeField] public EventReference HitTargetSpecial { get; private set; }
    [field: SerializeField] public EventReference EarnMoney { get; private set; }
    [field: SerializeField] public EventReference StartMatchCountDown { get; private set; }
    [field: SerializeField] public EventReference EndLastSeconds { get; private set; }
    [field: SerializeField] public EventReference WinMatch { get; private set; }
    [field: SerializeField] public EventReference LoseMatch { get; private set; }
    [field: SerializeField] public EventReference ScreamLlama { get; private set; }

    public enum EventReferenceType {
        Ambience,
        MainMenuTheme,
        BackgroundMusic,
        NormalAttack,
        SpecialAttack,
        CoolDownNormal,
        CoolDownSpecial,
        HitTargetNormal,
        HitTargetSpecial,
        EarnMoney,
        StartMatchCountDown,
        EndLastSeconds,
        WinMatch,
        LoseMatch,
        ScreamLlama,
    }

    public EventReference GetEventReference(EventReferenceType type) {
        switch (type) {
            case EventReferenceType.Ambience:
                return Ambience:
            case EventReferenceType.MainMenuTheme:
                return MainMenuTheme;
            case EventReferenceType.BackgroundMusic:
                return BackgroundMusic;
            case EventReferenceType.NormalAttack:
                return NormalAttack;
            case EventReferenceType.SpecialAttack:
                return SpecialAttack;
            case EventReferenceType.CoolDownNormal:
                return CoolDownNormal;
            case EventReferenceType.CoolDownSpecial:
                return CoolDownSpecial;
            case EventReferenceType.HitTargetNormal:
                return HitTargetNormal;
            case EventReferenceType.HitTargetSpecial:
                return HitTargetSpecial;
            case EventReferenceType.EarnMoney:
                return EarnMoney;
            case EventReferenceType.StartMatchCountDown:
                return StartMatchCountDown;
            case EventReferenceType.EndLastSeconds:
                return EndLastSeconds;
            case EventReferenceType.WinMatch:
                return WinMatch;
            case EventReferenceType.LoseMatch:
                return LoseMatch;
            case EventReferenceType.ScreamLlama:
                return ScreamLlama;
            default:
                return default;
        }
    }
}