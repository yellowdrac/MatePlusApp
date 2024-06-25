public static class Feedback
{
    public static void Do(eFeedbackType type)
    {
        switch (type)
        {
	        case eFeedbackType.StartChallenge:
	        case eFeedbackType.ChallengeAccepted:
	        case eFeedbackType.WalkGravel:
	        case eFeedbackType.WalkGrass:
	        case eFeedbackType.SwordAttack:
	        case eFeedbackType.ButtonClick:
	        case eFeedbackType.DeathScream:
			case eFeedbackType.ButtonHover:
			case eFeedbackType.IntroMusic:
			case eFeedbackType.CastleOpened:
			case eFeedbackType.WalkSnow:
			case eFeedbackType.WalkWood:
			case eFeedbackType.WalkSand:
			case eFeedbackType.WalkMud:
			case eFeedbackType.PortalPassed:
			case eFeedbackType.SandZone23Ambient:
			case eFeedbackType.SnowZone5Ambient:
			case eFeedbackType.WoodZone6Ambient:
			case eFeedbackType.MudZone7Ambient:
			case eFeedbackType.LavaZone8Ambient:
				SFXPlayer.PlaySFX(type);
				break;
	        //case eFeedbackType.Soundtrack:
		        //SFXPlayer.PlaySFX(type);
		       // break;
        }
    }
    public static void Stop(eFeedbackType type)
    {
	    switch (type)
	    {
		    case eFeedbackType.IntroMusic:
		    case eFeedbackType.WalkSnow:
		    case eFeedbackType.WalkWood:
		    case eFeedbackType.SnowZone5Ambient:
		    case eFeedbackType.SandZone23Ambient:
		    case eFeedbackType.MudZone7Ambient:
		    case eFeedbackType.LavaZone8Ambient:
		    case eFeedbackType.WoodZone6Ambient:
		    case eFeedbackType.WalkGrass:
		    case eFeedbackType.WalkSand:
		    case eFeedbackType.WalkMud:
		    case eFeedbackType.WalkGravel:
			    SFXPlayer.StopSFX(type);
			    break;
	    }
    }
}