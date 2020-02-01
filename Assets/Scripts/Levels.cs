using UnityEngine;

public partial class GameStates
{
    void InitLevels()
    {
        var randomColors = new[]
        {
            RandomColor(),
            RandomColor(),
            RandomColor(),
            RandomColor(),
            RandomColor(),
            RandomColor(),
            RandomColor(),
            RandomColor(),
            RandomColor(),
        };

        levels = new Level[]
        {
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

        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].bgColor = randomColors[i];
            levels[i].spriteColor = Complement(randomColors[i]);
        }
    }

    Color Complement(Color c) => new Color(1 - c.r, 1 - c.g, 1 - c.b, 1);
    Color RandomColor() => new Color(Random.Range(0.3f, 0.9f), Random.Range(0.3f, 0.9f), Random.Range(0.3f, 0.9f), 1f);
}