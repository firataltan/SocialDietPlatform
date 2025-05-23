#!/bin/bash

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "Docker is not installed. Please install Docker first."
    exit 1
fi

# Check if Docker is running
if ! docker info &> /dev/null; then
    echo "Docker is not running. Please start Docker daemon."
    exit 1
fi

# Create necessary directories if they don't exist
directories=(
    "docker"
    "docker/init-scripts"
    "scripts"
)

for dir in "${directories[@]}"; do
    if [ ! -d "$dir" ]; then
        mkdir -p "$dir"
        echo "Created directory: $dir"
    fi
done

# Stop and remove existing containers
echo "Stopping and removing existing containers..."
docker-compose -f docker/docker-compose.yml down -v

# Build and start containers
echo "Building and starting containers..."
docker-compose -f docker/docker-compose.yml up -d

# Wait for PostgreSQL to be ready
echo "Waiting for PostgreSQL to be ready..."
max_attempts=30
attempt=0
is_ready=false

while [ "$is_ready" = false ] && [ $attempt -lt $max_attempts ]; do
    if docker exec socialdiet_postgres pg_isready -U postgres &> /dev/null; then
        is_ready=true
    else
        attempt=$((attempt + 1))
        sleep 2
    fi
done

if [ "$is_ready" = false ]; then
    echo "PostgreSQL failed to start within the expected time."
    exit 1
fi

echo "PostgreSQL is ready!"

# Wait for Elasticsearch to be ready
echo "Waiting for Elasticsearch to be ready..."
max_attempts=30
attempt=0
is_ready=false

while [ "$is_ready" = false ] && [ $attempt -lt $max_attempts ]; do
    if curl -s http://localhost:9200 &> /dev/null; then
        is_ready=true
    else
        attempt=$((attempt + 1))
        sleep 2
    fi
done

if [ "$is_ready" = false ]; then
    echo "Elasticsearch failed to start within the expected time."
    exit 1
fi

echo "Elasticsearch is ready!"

# Display container status
echo -e "\nContainer Status:"
docker-compose -f docker/docker-compose.yml ps

echo -e "\nSetup completed successfully!"
echo "You can access:"
echo "- PostgreSQL on localhost:5432"
echo "- Redis on localhost:6379"
echo "- Elasticsearch on localhost:9200"
echo "- Adminer on http://localhost:8080" 