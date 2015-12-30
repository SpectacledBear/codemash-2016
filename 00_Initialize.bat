DEL /Q manifest.json
nuget restore
"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" SpectacledBear.CodeMash2016.sln /t:clean /p:Configuration=Release
"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" SpectacledBear.CodeMash2016.sln /p:Configuration=Release
PAUSE
