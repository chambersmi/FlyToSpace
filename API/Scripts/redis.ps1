# Stop and remove the container named 'redis' if it exists
docker stop redis 2>$null
docker rm redis 2>$null

# Remove all Docker images named 'redis'
$images = docker images --format "{{.Repository}}:{{.Tag}} {{.ID}}" | Where-Object { $_ -like "redis:*" }
foreach ($image in $images) {
    $id = $image.Split(" ")[1]
    docker rmi -f $id 2>$null
}

# Pull the latest redis:7 image (optional but ensures you get the latest)
docker pull redis:7

# Start a new redis container
docker run -d --name redis -p 6379:6379 redis:7

# Output status
docker ps | Where-Object { $_ -match "redis" }
