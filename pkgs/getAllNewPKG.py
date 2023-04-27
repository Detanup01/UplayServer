import os
import shutil
cwd = os.getcwd()
cwd_ = cwd.replace("pkgs","")

def cop(path):
    nupkgs = os.listdir(path)
    for nupkg in nupkgs:
        if nupkg.__contains__(".nupkg"):
            print(path+nupkg)
            print(cwd + nupkg)
            shutil.copy(path+nupkg, cwd + "/"+ nupkg)
            print(nupkg)


Libs = cwd_ + "/Libs/"

debug = "/bin/Debug/"
release = "/bin/Release/"

libslist = os.listdir(Libs)
for x in libslist:
    if not x.__contains__("."):
        cop(Libs + x + debug)
        cop(Libs + x + release)
        print(x)

