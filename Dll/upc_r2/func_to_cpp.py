"Func.txt"

voidFunc = "UPC_Uninit"
IntPtrFuncs = ["UPC_ErrorToString", "UPC_ContextCreate" , "UPC_EmailGet" , "UPC_IdGet", "UPC_InstallLanguageGet", "UPC_NameGet", "UPC_TicketGet"]
uint = "unsigned"
IntPtr = "const char*"


normal = """EXPORT int """

body = """
{
    GET_PROXY_FUNC(__REPLACEME__);
    return proxyFunc(__FUNC_);
}
"""

thing = ""

x = open("Func.txt","r")
y = x.readlines()
for f in y:
    if f.__contains__("#"):
        continue
    if f.__contains__(voidFunc):
        continue
    if f.__contains__("IntPtr"):
        f = f.replace("IntPtr",IntPtr)
    if f.__contains__("uint"):
        f = f.replace("uint",uint)
    sd = f.split("(")
    if sd[0] in IntPtrFuncs:
        x = normal
        x = x.replace("int",IntPtr)
        rep = x + f.strip()
        rep = rep + body
        rep = rep.replace("__REPLACEME__",sd[0])
        v = sd[1]
        v = v.replace(")","")
        if v.__contains__(", "):
            v = v.split(", ")
            yeet = ""
            for vv in v:
                dsf = vv.split(" ")[1]
                #print(dsf)
                yeet = yeet + dsf + ","
            rep = rep.replace("__FUNC_",yeet[:-1].strip())
        else:
            #print("no ,")
            dsf = v.split(" ")[1]
            yeet = dsf
            rep = rep.replace("__FUNC_",yeet.strip())
    else:
        #print(f)
        rep = normal + f.strip()
        rep = rep + body
        rep = rep.replace("__REPLACEME__",sd[0])
        v = sd[1]
        v = v.replace(")","")
        if v.__contains__(", "):
            v = v.split(", ")
            yeet = ""
            for vv in v:
                dsf = vv.split(" ")[1]
                #print(dsf)
                yeet = yeet + dsf + ","
            rep = rep.replace("__FUNC_",yeet[:-1].strip())
        else:
            #print("no ,")
            dsf = v.split(" ")[1]
            yeet = dsf
            rep = rep.replace("__FUNC_",yeet.strip())
        thing = thing + "\n" + rep
        #print(rep)


print(thing)
