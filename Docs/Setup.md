# This is a Setup file for hosting the server on local pc.
You must run Everything as Administator.


## Hosts:
You **need** to edit your hosts file for this to work!
The hosts file can be found here: `C:\Windows\System32\drivers\etc\hosts`.

The hosts file should look like this:
```
IP Domain
```

Now you can add this:
```
127.0.0.1 dmx.upc.ubisoft.com # Our Ubisoft DMX Server
127.0.0.1 local-ubiservices.ubi.com # Local Ubisoft Services
```
The `#` symbol indicates that everything after it in the line is a comment.

We using local instead of public because that hasnt been reversed fully yet.

## Config
Take a look into ServerConfig.md\
If you want to connect upc/ubiconnect to local server set it DemuxUrl to `dmx.upc.ubisoft.com`.

## CERT
Because the client and kinda everything wants to have a signed SSL connection we need to generate our own Cert.\
Go to the cert folder\
Then copy dll,exe,bat from v2_run paste to where you see the .conf files and run the bat!\
DO NOT forget to install all of the cert or pfx.\
The Certificate Import Wizard helps, Install into LOCAL MATCHINE\
Select PLACE ALL instead of AUTOMATIC.\
Then select:\
Trusted Root Certification Authorities\
Hit Next and Finish.

