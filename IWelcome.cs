namespace MyASP
{
    public interface IWelcome
    {
        string GetMessage();
    }
    public class WelcomeSer : IWelcome
    {
        public string GetMessage()
        {
            return "service";
        }
    }
}