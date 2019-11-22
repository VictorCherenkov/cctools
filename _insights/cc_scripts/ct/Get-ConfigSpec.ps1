#Parameters examples
#cgafe 2.45 cgafesr5451 
param([string]$productId,
[string]$release,
[string]$sr,
[string]$makeSR="true",
[string]$ignoreBTV="false")

Invoke-WebRequest -Uri "http://sc-css-wb-02-vm.amat.com/www1/cmweb/services/cmweb.srvc.mconfig-spec.php?optimize=true&ServiceCMD=QuickGet&productId=$productId&release=$release&sr=$sr&makeSR=$makeSR&ignoreBTV=$ignoreBTV" -UseDefaultCredentials | Write-Host 
