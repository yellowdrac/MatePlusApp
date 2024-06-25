using UnityEngine;
using System.Collections.Generic; 
// PROBANDO PROPIEDADES EN VEZ DE USAR SETTERS/GETTERS
public static class PlayerLevelInfo
{
	public static string user { get; set; }
	public static string pass { get; set; }
	public static int currentLevel{ get; set; }
	public static int currentXP{ get; set; }
	public static int currentZone{ get; set; }
	public static int totalQuestions { get; set; }
	public static int totalAnsCorrectQuestions { get; set; }
	public static int[] ansCorrectQuestionsPerZone { get; set; }
	public static int[] questionsPerZone { get; set; }
	public static float[] timePerZone {get; set;}
	
	public static int[] ansCorrectQuestionsPerArea { get; set; }
	public static int[] questionsPerArea { get; set; }
	public static float[] timePerArea {get; set;}
	
	public static List<Challenge> challenges { get; set; } = new List<Challenge>();
	public static int[] timePerSubject { get; set; }
	public static int[] timePerQuestion { get; set; }
	public static bool[] stateAnswers 	{ get; set; }
	public static List<List<int>> challengesRandoms { get; set; } = new List<List<int>>(); // Lista doble de enteros
}

public struct Challenge
{
	public string txtChallenge;
	public string urlImage;
	public string urlImageRightSolution;
	public float timeResult;
	public bool correct;
	public string urlSolution;
	public string txtRightSol;
	public bool correctOptionIsImage;

	public Challenge(string txtChallenge, string urlImage,string urlImageRightSolution, float timeResult, bool correct, string urlSolution, string txtRightSol, bool correctOptionIsImage)
	{
		this.txtChallenge = txtChallenge;
		this.urlImage = urlImage;
		this.urlImageRightSolution = urlImageRightSolution;
		this.timeResult = timeResult;
		this.correct = correct;
		this.urlSolution = urlSolution;
		this.txtRightSol = txtRightSol;
		this.correctOptionIsImage = correctOptionIsImage;
	}
}