DEL /Q SpectacledBear.CodeMash2016.WebApi\bin\style.css
DEL /Q SpectacledBear.CodeMash2016.WebApi\bin\uninstall.000
"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" SpectacledBear.CodeMash2016.sln /t:clean /p:Configuration=Release
"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" SpectacledBear.CodeMash2016.sln /p:Configuration=Release
PAUSE
