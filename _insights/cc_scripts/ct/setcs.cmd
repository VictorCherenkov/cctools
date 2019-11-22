if "%4"=="nosr" (
    set p4=false
) else (
    set p4=true
)

if "%4"=="nosr" (
    set p5=true
) else (
    set p5=false
)

del c:\temp\cs.txt
powershell Save-ConfigSpec.ps1 %1 %2 %3 %p4% %p5% c:\temp\cs.txt
cleartool setcs c:\temp\cs.txt
del c:\temp\cs.txt
rem todo: open updt file
rem todo: remove updt files