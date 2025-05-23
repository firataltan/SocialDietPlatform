# Check if Docker is installed
if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    Write-Host "Docker is not installed. Please install Docker Desktop for Windows first."
    exit 1
}

# Check if Docker is running
try {
    docker info | Out-Null
}
catch {
    Write-Host "Docker is not running. Please start Docker Desktop."
    exit 1
}

# Create necessary directories if they don't exist
$directories = @(
    "docker",
    "docker/init-scripts",
    "scripts"
)

foreach ($dir in $directories) {
    if (-not (Test-Path $dir)) {
        New-Item -ItemType Directory -Path $dir
        Write-Host "Created directory: $dir"
    }
}

# Stop and remove existing containers
Write-Host "Stopping and removing existing containers..."
docker-compose -f docker/docker-compose.yml down -v

# Build and start containers
Write-Host "Building and starting containers..."
docker-compose -f docker/docker-compose.yml up -d

# Wait for SQL Server to be ready
Write-Host "Waiting for SQL Server to be ready..."
$maxAttempts = 30
$attempt = 0
$isReady = $false

while (-not $isReady -and $attempt -lt $maxAttempts) {
    try {
        $query = "SELECT 1"
        $result = docker exec socialdiet_sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrongPassword123 -Q $query
        if ($result -match "1") {
            $isReady = $true
        }
    }
    catch {
        $attempt++
        Start-Sleep -Seconds 2
    }
}

if (-not $isReady) {
    Write-Host "SQL Server failed to start within the expected time."
    exit 1
}

Write-Host "SQL Server is ready!"

# Wait for Elasticsearch to be ready
Write-Host "Waiting for Elasticsearch to be ready..."
$maxAttempts = 30
$attempt = 0
$isReady = $false

while (-not $isReady -and $attempt -lt $maxAttempts) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:9200" -Method GET
        if ($response.StatusCode -eq 200) {
            $isReady = $true
        }
    }
    catch {
        $attempt++
        Start-Sleep -Seconds 2
    }
}

if (-not $isReady) {
    Write-Host "Elasticsearch failed to start within the expected time."
    exit 1
}

Write-Host "Elasticsearch is ready!"

# Display container status
Write-Host "`nContainer Status:"
docker-compose -f docker/docker-compose.yml ps

Write-Host "`nSetup completed successfully!"
Write-Host "You can access:"
Write-Host "- SQL Server on localhost:1433"
Write-Host "- Redis on localhost:6379"
Write-Host "- Elasticsearch on localhost:9200"
Write-Host "- Azure Data Studio on http://localhost:8080" 