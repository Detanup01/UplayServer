namespace ServerCore.Controller;

public class RandomController
{
    public static string RandomString(int lenght)
    {
        Random random = new Random();
        string str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string ran = string.Empty;

        for (int i = 0; i < lenght; i++)
        {
            int x = random.Next(str.Length);
            ran = ran + str[x];
        }

        return ran;
    }
}
