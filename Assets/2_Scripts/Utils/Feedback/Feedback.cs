public static class Feedback
{
    public static void Do(eFeedbackType type)
    {
        switch (type)
        {
	        case eFeedbackType.StartChallenge:
	        case eFeedbackType.ChallengeAccepted:
	        case eFeedbackType.WalkGravel:
	        case eFeedbackType.ButtonClick:
			case eFeedbackType.ButtonHover:
			case eFeedbackType.IntroMusic:	
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
		    case eFeedbackType.WalkGravel:
			    SFXPlayer.StopSFX(type);
			    break;
	    }
    }
}