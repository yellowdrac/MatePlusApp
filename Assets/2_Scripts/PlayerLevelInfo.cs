using UnityEngine;

// PROBANDO PROPIEDADES EN VEZ DE USAR SETTERS/GETTERS
public static class PlayerLevelInfo
{
	public static  int currentLevel{ get; set; }
	public static  int currentZone{ get; set; }
	public static  int playerLives{ get; set; }
	public static  int playerKeyParts{ get; set; }
	public static  int totalQuestions { get; set; }
	public static  int correctAnswers { get; set; }
	public static  int timePerQuestion { get; set; }
	public static  Color[] colors{ get; set; }
	public static  int colorsCount{ get; set; }
	public static  bool heart{ get; set; }
	
	public static bool fromLevel { get; set; }

}