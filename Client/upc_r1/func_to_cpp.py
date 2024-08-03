"Func.txt"

uint = "unsigned"
IntPtr = "intptr_t"


normal = """UPLAY_EXPORT """

body = """
{
	PRINT_DEBUG("%s\\n", __FUNCTION__, __FUNC_);
	return 0;
}
"""

old_body = """
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
    if f.__contains__("IntPtr"):
        f = f.replace("IntPtr",IntPtr)
    if f.__contains__("uint"):
        f = f.replace("uint",uint)
    sd = f.split(" UPLAY_")[1].split("(")
    #print("f: " + f)
    rep = normal + f.strip()
    rep = rep + body
    rep = rep.replace("__REPLACEME__", "UPLAY_" +sd[0])
    v = sd[1]
    #print("v: " + v)
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
        #print("NOT CONTAINS ,")
        if v.__contains__(" "):
            dsf = v.split(" ")[1]
            yeet = dsf
            rep = rep.replace("__FUNC_",yeet.strip())
        else:
            rep = rep.replace("__FUNC_","")
    thing = thing + "\n" + rep
    #print(rep)


print(thing)
