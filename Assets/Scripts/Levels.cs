using UnityEngine;

public partial class GameStates
{
    void InitLevels()
    {
        levels = new Level[]
        {
            GenerateLevel(),
            GenerateLevel(),
            GenerateLevel(),
            GenerateLevel(),
            GenerateLevel()
        };
    }

    Level GenerateLevel()
    {
        int LolRandom(int x1, int y1, int x2, int y2) =>
            Random.value < 0.5f
                ? Random.Range(x1, y1)
                : Random.Range(x2, y2);

        int targetX = Random.Range(1, Screen.width / 100 - 1);
        int targetY = Random.Range(1, Screen.height / 100 - 1);
        int targetRot = Random.Range(1, 7);
        int targetScale = Random.Range(1, 5);

        int playerX = Mathf.Clamp(targetX + LolRandom(-6, -2, 2, 6), 1, Screen.width / 100 - 1);
        int playerY = Mathf.Clamp(targetY + LolRandom(-6, -2, 2, 6), 1, Screen.height / 100 - 1);
        int playerRot = Random.Range(1, 7); if (playerRot == targetRot) playerRot = Random.Range(1, 7);
        int playerScale = Random.Range(1, 5); if (playerScale == targetScale) playerScale = Random.Range(1, 5);

        Color bgColor = new Color(
            Random.Range(0.6f, 0.9f),
            Random.Range(0.6f, 0.9f),
            Random.Range(0.6f, 0.9f),
            1f);
        Color spriteColor = new Color(1 - bgColor.r, 1 - bgColor.g, 1 - bgColor.b, 1);

        return new Level
        {
            target = new MyTransform
            {
                x = targetX,
                y = targetY,
                rotation = targetRot,
                scale = targetScale,
            },
            start = new MyTransform
            {
                x = playerX,
                y = playerY,
                rotation = playerRot,
                scale = playerScale
            },
            bgColor = bgColor,
            spriteColor = spriteColor,
            sprite = Sprites[0]
        };
    }
}