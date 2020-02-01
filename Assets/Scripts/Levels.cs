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
        int targetX = Random.Range(1, Screen.width / 100 - 1);
        int targetY = Random.Range(1, Screen.height / 100 - 1);
        int targetRot = Random.Range(1, 7);
        int targetScale = Random.Range(1, 5);

        int playerX = Mathf.Clamp(targetX + Random.Range(-5, 5), 1, Screen.width / 100 - 1);
        int playerY = Mathf.Clamp(targetY + Random.Range(-5, 5), 1, Screen.height / 100 - 1);
        int playerRot = Random.Range(1, 7);
        int playerScale = Random.Range(1, 5);

        const float valueMin = 0.3f;
        const float valueMax = 0.9f;

        Color bgColor = new Color(Random.Range(valueMin, valueMax), 
            Random.Range(valueMin, valueMax), 
            Random.Range(valueMin, valueMax), 
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