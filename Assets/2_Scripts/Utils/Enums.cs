﻿// -------------------------------------------------
// INPUT 
// -------------------------------------------------

public enum eMouseButton
{
    Left = 0,
    Right = 1,
    Middle = 2
}

public enum eControllerType
{ 
    Keyboard,
    XBOXJoystick,
    SwitchJoystick,
    PS4Joystick
}

// -------------------------------------------------
// GAME 
// -------------------------------------------------

public enum eScreen
{
    Splash,
    Game,
    Menu,
    Loading,
    Introduction,
    Final
}

public enum eDirection
{ 
    Left,
    Right,
    Up,
    Down
}

// REPEATING CODE AS SFX.
// COULD HAVE JUST FEEDBACK ENUM AND TRY TO CAST IT AS SFX ENUM.
// IN CASE OF SUCCESS, SOUND EFFECT EXISTS.
public enum eFeedbackType
{ 
    ButtonHover,
    ButtonClick,
    StartChallenge,
    ChallengeAccepted,
    WalkGravel,
    DeathScream,
    WalkSnow,
    WalkMud,
    WalkWood,
    WalkGrass,
    WalkSand,
    IntroMusic,
    SwordAttack,
    CastleOpened,
    PortalPassed,
    SnowZone5Ambient,
    SandZone23Ambient,
    WoodZone6Ambient,
    LavaZone8Ambient,
    MudZone7Ambient,
}

public enum eAnimation
{
    None,
    Idle,
    Walk,
    Attack,
    Death,
    Hit,
    Jump,
    Dissapear,
    Explote,
    IceMelt,
    IceMeltHigh,
    Revive
}
public enum eEnemyType
{
    Skeleton,
}