using UnityEngine;

// PROBANDO PROPIEDADES EN VEZ DE USAR SETTERS/GETTERS
public static class PlayerLevelInfo
{
	public static int currentLevel{ get; set; }
	public static int currentXP{ get; set; }
	public static int currentZone{ get; set; }
	public static int totalQuestions { get; set; }
	public static int[] timePerZone {get; set;}
	public static int[] timePerSubject { get; set; }
	public static int[] timePerQuestion { get; set; }
	public static bool[] stateAnswers 	{ get; set; }
}