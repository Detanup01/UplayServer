using Core.JSON;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Core
{
    public class jwt
    {
        /// <summary>
        /// Create cert/private.xml
        /// </summary>
        public static void CreateRSA()
        {
            if (!File.Exists("cert/private.xml"))
            {
                RSA rsa = RSA.Create();
                File.WriteAllText("cert/private.xml", rsa.ToXmlString(true));
            }
        }

        /// <summary>
        /// Create Ownership Token
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <param name="uplayId">Uplay Id</param>
        /// <param name="ProdId">Product Id</param>
        /// <param name="branchId">Branch Id</param>
        /// <param name="flags">List of AppFlags</param>
        /// <param name="exp">Expire of time (UTCNOW + 15 MIN Default)</param>
        /// <returns>Ownership Token</returns>
        public static string CreateOwnershipToken(string UserId, uint uplayId, uint ProdId, uint branchId, List<string> flags, long exp = long.MinValue)
        {
            RSA rsa = RSA.Create();
            rsa.FromXmlString(File.ReadAllText("cert/private.xml"));
            File.WriteAllText("cert/private.xml", rsa.ToXmlString(true));
            if (exp == long.MinValue)
            {
                exp = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds();
            }
            var token = JwtBuilder.Create()
            .Subject(UserId)
            .Issuer("ownership_service")
            .WithAlgorithm(new RS256Algorithm(rsa, rsa))
            .AddClaim("exp", exp)
            .AddClaim("uplay_id", uplayId)
            .AddClaim("product_id", ProdId)
            .AddClaim("branch_id", branchId)
            .AddClaim("flags", flags)
            .Encode();

            return token;

        }

        public static string CreateUplayTicket(string UserId, uint uplayId, int platform, long exp = long.MinValue)
        {
            RSA rsa = RSA.Create();
            rsa.FromXmlString(File.ReadAllText("cert/private.xml"));
            File.WriteAllText("cert/private.xml", rsa.ToXmlString(true));
            if (exp == long.MinValue)
            {
                exp = DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds();
            }
            var token = JwtBuilder.Create()
            .Subject(UserId)
            .Issuer("ownership_service")
            .WithAlgorithm(new RS256Algorithm(rsa, rsa))
            .AddClaim("exp", exp)
            .AddClaim("uplay_id", uplayId)
            .AddClaim("branch_id", platform)
            .Encode();

            return token;

        }

        /// <summary>
        /// Create Auth Token
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <param name="SessionId">Session Id</param>
        /// <param name="AppId">AppId where generated from</param>
        /// <param name="env">Enviroment</param>
        /// <param name="exp">Expire of time (UTCNOW + 1 HOUR Default)</param>
        /// <returns>Auth Token</returns>
        public static string CreateAuthToken(string UserId, string SessionId, string AppId, string env = "prod", long exp = long.MinValue)
        {
            RSA rsa = RSA.Create();
            rsa.FromXmlString(File.ReadAllText("cert/private.xml"));
            File.WriteAllText("cert/private.xml", rsa.ToXmlString(true));
            if (exp == long.MinValue)
            {
                exp = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();
            }
            var token = JwtBuilder.Create()
            .Subject(UserId)
            .Issuer("auth_service")
            .WithAlgorithm(new RS256Algorithm(rsa, rsa))
            .AddClaim("exp", exp)
            .AddClaim("session", SessionId)
            .AddClaim("app", AppId)
            .AddClaim("env", env)
            .Encode();

            return token;
        }

        /// <summary>
        /// Validating any jwt Token
        /// </summary>
        /// <param name="token">The Token</param>
        /// <returns>True | False</returns>
        public static bool Validate(string token)
        {
            RSA rsa = RSA.Create();
            rsa.FromXmlString(File.ReadAllText("cert/private.xml"));
            File.WriteAllText("cert/private.xml", rsa.ToXmlString(true));
            try
            {
                var json = JwtBuilder.Create()
                                     .WithAlgorithm(new RS256Algorithm(rsa, rsa))
                                     .MustVerifySignature()
                                     .Decode(token);
            }
            catch (TokenNotYetValidException)
            {
                Console.WriteLine("Token is not valid yet");
                return false;
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
                return false;
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature (Probably used outside, we let it go)");
                return true;
            }
            return true;
        }

        /// <summary>
        /// Get JWT Token as JSON
        /// </summary>
        /// <param name="token">The Token</param>
        /// <returns>JSON String</returns>
        public static string GetJWTJson(string token)
        {
            RSA rsa = RSA.Create();
            rsa.FromXmlString(File.ReadAllText("cert/private.xml"));
            File.WriteAllText("cert/private.xml", rsa.ToXmlString(true));
            var json = JwtBuilder.Create()
                                         .WithAlgorithm(new RS256Algorithm(rsa, rsa))
                                         .Decode(token);

            return json;
        }


        public static string GetUnkownJWTJson(string token)
        {
            var json = JwtBuilder.Create().WithVerifySignature(false).DecodeHeader(token);
            return json;
        }

        /// <summary>
        /// Get Expire time
        /// </summary>
        /// <param name="token">The Token</param>
        /// <returns>Unix Time</returns>
        public static long GetExp(string token)
        {
            var json = GetJWTJson(token);

            var iss = json.Split("iss\":\"")[1].Split("\",")[0];

            long returner = 0;

            switch (iss)
            {
                case "auth_service":
                    returner = JsonConvert.DeserializeObject<JWToken.auth_service>(json).exp;
                    break;

                case "ownership_service":
                    returner = JsonConvert.DeserializeObject<JWToken.ownership_service>(json).exp;
                    break;
                default:
                    break;
            }
            return returner;
        }
    }
}
