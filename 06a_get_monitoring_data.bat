@ECHO OFF
curl -s -H "Accept: application/json" "http://localhost:11341/api/monitoring/" | python -mjson.tool
ECHO.
