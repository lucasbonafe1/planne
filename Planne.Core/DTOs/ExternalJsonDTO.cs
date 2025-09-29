namespace Planne.Core.DTOs;

public class ExternalJsonDTO<T> where T : class
{
    public T[] items { get; set; } = [];
    public int status_code { get; set; }
    public int total_items { get; set; }
    public int status { get; set; }
    public string message { get; set; } = string.Empty;
    public int total_registros { get; set; }
}
