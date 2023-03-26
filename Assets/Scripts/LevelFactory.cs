



public class LevelFactory
{
    public static Level CreateLevel(int levelNum)
    {
        switch (levelNum)
        {
            case 1:
                return new Level1(1, 2);
            default:
                return null;
        }
    }
}