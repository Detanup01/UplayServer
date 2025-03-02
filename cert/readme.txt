Since ubisoft connect and some other service will gonna fuck you up with the certificate revokation. I advise use this site:
https://pkiaas.io/

Use run_only_csr.bat to generate the csr files.

Firstly in the website log in and create a "ROOT CA".
(Simpy click in Create New CA)

In here give the name of "Custom Servers CA" or something you know it is your CA.
Most of stuff here is not actually needed.
Make sure you Tick those Extended Key Usage:
TLS Web Server Authentication
TLS Web Client Authentication
Code Signing
E-mail Protection
Time Stamping
OCSP Signing
And click the button Create New CA.

After this click the Create New CA again and simply put "Custom Servers Assurance CA", many things check for root ca but not for Assurance CA.
Tick CSR Only
And click the button Create New CA.

Copy the Certificate Request.

Create a New Template.
Name could be anything I set as Assurance CA
Set the Starter Template to Certificate Authority
And click the button Create Template.

Go to Subject and tick Supplied by the CSR
Press Save.

Press right click on newly created template.
Press Submit CSR
Pase the Certificate Request
Press Submit

Make sure you select the "Custom Servers Assurance CA" or your Assurance CA name in upper place.
Open services.csr file and copy its content.
Go to Manage Template
Right click into Web Server
Press Submit CSR
Pase the services.csr files content
Press Submit.

Go to Pending Request.
And Sign it.

Now go to Issued Certificates
Right click the newest one and you can there now download the Certificate.

WINDOWS:
To get a pfx open cmd and navigate to a place where you downloaded.
Run this in command line:
certutil -p "CustomUplay,CustomUplay" -mergepfx services.crt services.pfx
This will create a services.pfx

Or search online to make pfx from crt.