@ECHO OFF
curl -s -H "Accept: application/json" "http://localhost:11341/api/hobbit/" | python -mjson.tool
ECHO.
