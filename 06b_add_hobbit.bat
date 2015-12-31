@ECHO OFF
curl -s -H "Content-Type: application/json" -H "Accept: application/json" -X POST -d "{ \"Name\": \"Meriadoc Brandybuck\", \"FamilyName\": \"Brandybuck\", \"BirthYear\": 1382, \"DeathYear\": 1485 }" http://localhost:11341/api/hobbit/ | python -mjson.tool
ECHO.
PAUSE
