namespace Personal_Finance_Manager.Services;

public class MessageService
{
    private Dictionary<string, string> _messages = new Dictionary<string, string>();

    public void Set(string userId, string message)
    {
        _messages[userId] = message;
    }

    public string GetAndClear(string userId)
    {
        if (_messages.ContainsKey(userId))
        {
            var msg = _messages[userId];
            _messages.Remove(userId);
            return msg;
        }

        return null;
    }
}
