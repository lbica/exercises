# Define the Docker Compose file location
$dockerComposeFile = "./docker/docker-compose.yml"

# Stop and remove existing containers (if running)
Write-Host "Stopping existing containers..."
docker-compose -f $dockerComposeFile down

# Build and start the containers in detached mode
Write-Host "Starting Docker Compose..."
docker-compose -f $dockerComposeFile up -d --build

# Show running containers
Write-Host "Listing running containers..."
docker ps

# Display logs (optional)
Write-Host "Showing logs..."
docker-compose -f $dockerComposeFile logs -f
