using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using Newtonsoft.Json;
using ServerCore.DB;
using ServerCore.Models.Requests;
using ServerCore.Models.Responses;
using ServerCore.Models.User;
using System.Text;

namespace ServerCore.HTTP.V3;

internal class Users
{
    [HTTP("POST", "/v3/users/validateCreation")]
    public static bool validateCreation(HttpRequest request, ServerStruct serverStruct)
    {
        ValidateCreation? validate = JsonConvert.DeserializeObject<ValidateCreation>(request.Body);
        if (validate == null)
        {
            // TODO: Handle error
            serverStruct.Response.MakeErrorResponse("cannot be parsed!");
            serverStruct.SendResponse();
            return true;
        }
        UserLogin? user = DBUser.Get<UserLogin>(x => x.NameOnPlatform == validate.nameOnPlatform);
        if (user == null)
        {
            user = new()
            {
                UserId = Guid.NewGuid(),
                Country = validate.country,
                DateOfBirth = validate.dateOfBirth,
                Email = validate.email,
                LegalOptinsKey = validate.legalOptinsKey,
                NameOnPlatform = validate.nameOnPlatform,
                PreferredLanguage = validate.preferredLanguage,
                Password = validate.password, // do we want to do anything with the pass? we currently store as is. not really good idea. (Hashing atleast should work tbh.)
            };
            DBUser.Add(user);
            DBUser.Add(new UserCommon()
            {
                UserId = user.UserId,
                Name = user.NameOnPlatform,
                Friends = [],
                IsBanned = false
            });
            Auth.AddCurrent(user.UserId, Convert.ToBase64String(Encoding.UTF8.GetBytes($"{validate.email}:{validate.password}")), TokenType.BasicAuth);
        }
        // we already have a user, just send the id.
        serverStruct.Response.MakeGetResponse(JsonConvert.SerializeObject(new RegisterResponse() { UserId = user!.UserId }));
        serverStruct.SendResponse();
        return true;
    }
}
