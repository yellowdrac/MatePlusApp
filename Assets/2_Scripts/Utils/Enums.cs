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
    Introduction
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
    IntroMusic
}

public enum eAnimation
{
    None,
    Idle,
    Walk,
    Attack,
    Death,
    Hit,
    Jump
}
public enum eEnemyType
{
    Skeleton,
}