param (
    [string]$ContainerName
)

$ImageName = $null

# Si se pasó nombre, intentar obtener la imagen
if ($ContainerName) {
    try {
        $ImageName = docker inspect $ContainerName --format='{{.Config.Image}}' 2>$null
    } catch {
        $ImageName = $null
    }
}

Write-Host "Ejecutando docker compose down -v..."
docker compose down -v

# Eliminar imagen solo si existe
if ($ImageName) {
    Write-Host "Eliminando imagen $ImageName..."
    docker rmi -f $ImageName
} else {
    if ($ContainerName) {
        Write-Host "Contenedor '$ContainerName' no encontrado. Saltando eliminación de imagen."
    } else {
        Write-Host "No se pasó nombre de contenedor. Saltando eliminación de imagen."
    }
}

Write-Host "Ejecutando docker compose up -d..."
docker compose up -d

Write-Host "Listo."
