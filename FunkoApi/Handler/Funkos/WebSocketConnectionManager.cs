using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace FunkoApi.Handler.Funkos;

public class WebSocketConnectionManager(ILogger<WebSocketConnectionManager> logger)
{
    private readonly ConcurrentDictionary<string, WebSocket> _connections = new();
    private readonly ConcurrentDictionary<string, HashSet<string>> _userConnections = new();

    public string AddConnection(WebSocket webSocket)
    {
        var connectionId = Guid.NewGuid().ToString();
        _connections.TryAdd(connectionId, webSocket);
        logger.LogInformation("Conexion agregada: {ConnectionId}. Total: {Count}", 
            connectionId, _connections.Count);
        return connectionId;
    }

    public void RemoveConnection(string connectionId)
    {
        if (_connections.TryRemove(connectionId, out var webSocket))
        {
            foreach (var kvp in _userConnections)
            {
                kvp.Value.Remove(connectionId);
            }
            logger.LogInformation("Conexion eliminada: {ConnectionId}. Total: {Count}", 
                connectionId, _connections.Count);
        }
    }

    public WebSocket? GetConnection(string connectionId)
    {
        _connections.TryGetValue(connectionId, out var webSocket);
        return webSocket;
    }

    public async Task SendMessageAsync(string connectionId, string message)
    {
        if (_connections.TryGetValue(connectionId, out var webSocket) && 
            webSocket.State == WebSocketState.Open)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }
    }

    public async Task BroadcastAsync(string message)
    {
        var connections = _connections
            .Where(kvp => kvp.Value.State == WebSocketState.Open)
            .ToList();
        
        logger.LogInformation("Broadcast a {Count} conexiones", connections.Count);

        foreach (var (connectionId, webSocket) in connections)
        {
            await SendMessageAsync(connectionId, message);
        }
    }

    public async Task SendToGroupAsync(string groupName, string message)
    {
        if (_userConnections.TryGetValue(groupName, out var connections))
        {
            var tasks = connections
                .Where(id => _connections.TryGetValue(id, out var ws) && ws.State == WebSocketState.Open)
                .Select(id => SendMessageAsync(id, message));

            await Task.WhenAll(tasks);
        }
    }

    public void AddToGroup(string connectionId, string groupName)
    {
        _userConnections.GetOrAdd(groupName, _ => new HashSet<string>()).Add(connectionId);
    }

    public void RemoveFromGroup(string connectionId, string groupName)
    {
        if (_userConnections.TryGetValue(groupName, out var connections))
        {
            connections.Remove(connectionId);
        }
    }

    public IEnumerable<WebSocket> GetAllConnections()
    {
        return _connections.Values.Where(c => c.State == WebSocketState.Open);
    }
}

public class WebSocketMessage
{
    public string Type { get; set; } = string.Empty;
    public string? Payload { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}