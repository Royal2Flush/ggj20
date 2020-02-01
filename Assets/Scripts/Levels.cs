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
            new Level
            {
                target = new MyTransform
                {
                    x = 5, y = 3, rotation = 7, scale = 1
                },
                start = new MyTransform
                {
                    x = 3, y = 5, rotation = 0, scale = 1
                },
                sprite = Sprites[0],
            },
            new Level
            {
                target = new MyTransform
                {
                    x = 7, y = 1, rotation = 4, scale = 2
                },
                start = new MyTransform
                {
                    x = 1, y = 5, rotation = 7, scale = 2
                },
                sprite = Sprites[0],
            },
        };

        
    }

    Level GenerateLevel()
    {
        int targetX = Random.Range(1, Screen.width / 100 - 1);
        int targetY = Random.Range(1, Screen.width / 100 - 1);
        int targetRot = Random.Range(-3, 4);
        int targetScale = Random.Range(1, 5);

        int playerX = Random.Range(1, Screen.width / 100 - 1);
        int playerY = 10 - playerX;
        int playerRot = Random.Range(-3, 4);
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