param([string]$productId,
[string]$release,
[string]$sr,
[string]$makeSR="true",
[string]$ignoreBTV="false",
[string]$optimize="false",
[string]$useStorageSrv="true")

Invoke-WebRequest -Uri "http://sc-css-wb-02-vm.amat.com/www1/cmweb/services/cmweb.srvc.mconfig-spec.php?optimize=$optimize&ServiceCMD=QuickGet&productId=$productId&release=$release&sr=$sr&makeSR=$makeSR&ignoreBTV=$ignoreBTV&useStorageSrv=$useStorageSrv" -UseDefaultCredentials | Write-Host 
