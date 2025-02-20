openssl req -new -nodes -key global.key -config global.conf -nameopt utf8 -utf8 -out global.csr
openssl req -x509 -nodes -in global.csr -days 3650 -key global.key -config global.conf -extensions req_ext -nameopt utf8 -utf8 -out global.crt
openssl req -new -nodes -key services.key -config services_csr.conf -nameopt utf8 -utf8 -out services.csr
openssl x509 -req -in services.csr -days 3650 -CA global.crt -CAkey global.key -extfile services_cert.conf -extensions req_ext -CAserial services.srl -CAcreateserial -nameopt utf8 -sha256 -out services.crt
openssl req -new -nodes -key signer.key -config signer_csr.conf -nameopt utf8 -utf8 -out signer.csr
openssl x509 -req -in signer.csr -days 3650 -CA global.crt -CAkey global.key -extfile signer_cert.conf -extensions req_ext -CAserial signer.srl -CAcreateserial -nameopt utf8 -sha256 -out signer.crt
certutil -p "CustomUplay,CustomUplay" -mergepfx global.crt global.pfx
certutil -p "CustomUplay,CustomUplay" -mergepfx signer.crt signer.pfx
certutil -p "CustomUplay,CustomUplay" -mergepfx services.crt services.pfx
pause