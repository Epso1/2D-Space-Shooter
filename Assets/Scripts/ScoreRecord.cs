[System.Serializable]
public class ScoreRecord
{
    public string initials;
    public int score;

    public ScoreRecord(string initials, int score)
    {
        this.initials = initials;
        this.score = score;
    }
}
