public class Question
{
    public string question;
    public string[] answers;
    public int correctAnswerIndex;

    public Question(string question, string[] answers, int correctAnswerIndex)
    {
        this.question = question;
        this.answers = answers;
        this.correctAnswerIndex = correctAnswerIndex;
    }
    
    public override string ToString()
    {
        string result = question + "\n";
        foreach (string answer in answers)
        {
            result += answer + "\n";
        }
        result += correctAnswerIndex;
        return result;
    }
}