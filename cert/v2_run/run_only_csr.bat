openssl req -new -nodes -key global.key -config global.conf -nameopt utf8 -utf8 -out global.csr
openssl req -new -nodes -key services.key -config services_csr.conf -nameopt utf8 -utf8 -out services.csr
openssl req -new -nodes -key signer.key -config signer_csr.conf -nameopt utf8 -utf8 -out signer.csr
pause