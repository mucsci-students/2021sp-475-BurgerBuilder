using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DifficultyStatic
{
    public static int difficulty { get; set; } = 2;
    public static float trashChance { get; set; } = 0.0f;
    public static float fallingSpeed { get; set; } = -9.81f;
    public static string playfabScoreboard { get; set; } = "";
    public static float foodSpawnRate {get; set; } = 1.0f;

    public static bool trackpadMode {get; set; } = false;
}

