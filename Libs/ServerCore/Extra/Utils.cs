using ServerCore.Models;
using SharedLib.Shared;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ServerCore;

public class Utils
{
    public static void WriteFile(string strLog, string FileName)
    {
        FileInfo logFileInfo = new FileInfo(FileName);
        if (logFileInfo.DirectoryName == null)
            return;
        DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
        if (!logDirInfo.Exists) 
            logDirInfo.Create();
        using FileStream fileStream = new FileStream(FileName, FileMode.Append);
        using StreamWriter log = new StreamWriter(fileStream);
        log.WriteLine(strLog);
    }

    public static string MakeAuth(string auth)
    {
        return B64.ToB64(Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(auth).Concat(Encoding.UTF8.GetBytes(ServerConfig.Instance.sql.AuthSalt)).ToArray())));
    }

    public static X509Certificate GetCert(string certname, string? password)
    {
        Debug.PWDebug($"[GetCert] {certname} {password}");
        X509Certificate2 cert = X509CertificateLoader.LoadPkcs12FromFile($"cert/{certname}.pfx", password);
        return cert;
    }

    public static (string protoname, byte[] buffer) GetCustomProto(Guid Id, byte[] buffer)
    {
        Debug.PWDebug($"{Id}: Custom Request Received!", "DMXSERVER");
        int ReqNameLenght = int.Parse(Encoding.UTF8.GetString(new byte[] { buffer[1] }));
        var bytename = buffer.Skip(2).Take(ReqNameLenght).ToArray();
        string protoname = Encoding.UTF8.GetString(bytename);
        Debug.PrintDebug($"[DMXSERVER] Request Name: {protoname}");
        var bytes = buffer.Skip(2 + ReqNameLenght).ToArray();
        return (protoname, bytes);


    }
}
