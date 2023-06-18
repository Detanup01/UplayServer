# This is a Setup/Readme file for the ServerConfig.json!

## Common Knowledge:
- If you're hosting on a different computer, set `DemuxHOST` or `HTTPSHOST` to false.
- Ports **must** be different! You cannot host HTTP and TCP on the same Port!
- Default host IP is `127.0.0.1` (Localhost), only users on your network can connect.
  You can edit this to your IP (`192.168.1.50`) or VPN IP (`26.123.35.50`).

## Hosts:
You **need** to edit your hosts file for this to work!
The hosts file can be found here: `C:\Windows\System32\drivers\etc\hosts`.

The hosts file should look like this:
```
IP Domain
```

Now you can add this:
```
127.0.0.1 dmx.local.upc.ubisoft.com # Local ubisoft DMX
# 127.0.0.1 dmx.upc.ubisoft.com
127.0.0.1 local-ubiservices.ubi.com # Local ubisoft Services
```
The `#` symbol indicates that everything after it in the line is a comment.

## The Json:
* `DemuxIp` is the IP where you want to host the Demux server.
* `DemuxPort` is the Port where the client would connect. (Default is 443)
* `DemuxUrl` is usually `dmx.local.upc.ubisoft.com` or `dmx.upc.ubisoft.com`.
* `HTTPS_Ip` is the IP where you want to host the HTTPS server.
* `HTTPS_Port` is the Port where the client would connect. (Default is 7777)
* `HTTPS_Url` is usually `https://local-ubiservices.ubi.com`.

## SQL:
`AuthSalt` is used for logins to create a custom SALT password for the user (Default is `_CUSTOMDEMUX`).
If you use this, some user logins might not be available (`fUSER`, `0User`).

## Demux:
* `ServerFilesPath` is the path for server storage and receiving files.
* `DownloadGamePath` is where the server stores the manifest and chunks.
* `DefaultCountryCode` is the default country.
* `DefaultContinentCode` is the default continent.

- `GlobalOwnerShipCheck` bypasses the check to see if the user owns a game.
    ### In Ownership:
    - `EnableManifestRequest` toggles deprecated response for Manifest.
    - `EnableConfigRequest` toggles deprecated response for App Config.
