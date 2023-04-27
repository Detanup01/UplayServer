using ClientKit.UbiServices.Records;
using SharedLib.Shared;
using System.Text;
using static ClientKit.UbiServices.Public.V3;

namespace ClientKit.Lib
{
    public class LoginLib
    {
        public static LoginJson? TryLoginWithArgsCLI(string[] args)
        {
            Console.WriteLine(string.Join(", ", args));
            LoginJson? login = null;

            if (ParameterLib.HasParameter(args, "-ref"))
            {
                var refticket = ParameterLib.GetParameter<string>(args, "-ref");
                login = LoginRemember(refticket);
            }
            else if (ParameterLib.HasParameter(args, "-b64"))
            {
                var b64 = ParameterLib.GetParameter<string>(args, "-b64");
                if (ParameterLib.HasParameter(args, "-rem"))
                {
                    var remticket = ParameterLib.GetParameter<string>(args, "-rem");
                    login = LoginB64Device(b64, remticket);
                }
                else
                {
                    login = LoginBase64(b64);
                }
            }
            else if ((ParameterLib.HasParameter(args, "-username") || ParameterLib.HasParameter(args, "-user")) && (ParameterLib.HasParameter(args, "-password") || ParameterLib.HasParameter(args, "-pass")))
            {
                var username = ParameterLib.GetParameter<string>(args, "-username") ?? ParameterLib.GetParameter<string>(args, "-user");
                var password = ParameterLib.GetParameter<string>(args, "-password") ?? ParameterLib.GetParameter<string>(args, "-pass");
                login = Login(username, password);
            }
            else
            {
                Console.WriteLine("Please enter your Email:");
                string username = Console.ReadLine()!;
                Console.WriteLine("Please enter your Password:");
                string password = ReadPassword();
                login = Login(username, password);
            }
            if (login.Ticket == null)
            {
                Console.WriteLine("Your account has 2FA, please enter your code:");
                var code2fa = Console.ReadLine();
                if (code2fa == null)
                {
                    Console.WriteLine("Code cannot be empty!");
                    return null;
                }
                else
                {
                    login = TryLoginWith2FA(login, code2fa);
                }
            }
            return login;
        }

        public static LoginJson? TryLoginWithArgs(string[] args)
        {
            LoginJson? login = null;
            if (ParameterLib.HasParameter(args, "-ref"))
            {
                var refticket = ParameterLib.GetParameter<string>(args, "-ref");
                login = LoginRemember(refticket);
            }
            else if (ParameterLib.HasParameter(args, "-b64"))
            {
                var b64 = ParameterLib.GetParameter<string>(args, "-b64");
                if (ParameterLib.HasParameter(args, "-rem"))
                {
                    var remticket = ParameterLib.GetParameter<string>(args, "-rem");
                    login = LoginB64Device(b64, remticket);
                }
                else
                {
                    login = LoginBase64(b64);
                }
            }
            else if ((ParameterLib.HasParameter(args, "-username") || ParameterLib.HasParameter(args, "-user")) && (ParameterLib.HasParameter(args, "-password") || ParameterLib.HasParameter(args, "-pass")))
            {
                var username = ParameterLib.GetParameter<string>(args, "-username") ?? ParameterLib.GetParameter<string>(args, "-user");
                var password = ParameterLib.GetParameter<string>(args, "-password") ?? ParameterLib.GetParameter<string>(args, "-pass");
                login = Login(username, password);
            }
            return login;
        }

        public static LoginJson? TryLoginWith2FA(LoginJson? login, string code2fa)
        {
            LoginJson? ret = login;
            if (login.Ticket == null && login.TwoFactorAuthenticationTicket != null)
            {
                ret = Login2FA(login.TwoFactorAuthenticationTicket, code2fa);
            }
            return ret;
        }

        //  Thanks from SteamRE!
        public static string ReadPassword()
        {
            ConsoleKeyInfo keyInfo;
            var password = new StringBuilder();

            do
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.Remove(password.Length - 1, 1);
                        Console.Write("\b \b");
                    }

                    continue;
                }
                /* Printable ASCII characters only */
                var c = keyInfo.KeyChar;
                if (c >= ' ' && c <= '~')
                {
                    password.Append(c);
                    Console.Write('*');
                }
            } while (keyInfo.Key != ConsoleKey.Enter);

            return password.ToString();
        }
    }
}
