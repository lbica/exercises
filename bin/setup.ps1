$oldPath = [System.Environment]::GetEnvironmentVariable("Path", "User")
$newPath = $oldPath + ";C:\Program Files\Docker\Docker\resources\bin"
[System.Environment]::SetEnvironmentVariable("Path", $newPath, "User")


docker exec -it mysql_server /bin/bash




