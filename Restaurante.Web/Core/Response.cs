namespace Restaurante.Web.Core
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }

        public static Response Success(string message = "Tarea realizada con éxito")
            => new() { IsSuccess = true, Message = message };

        public static Response Failure(string message, List<string>? errors = null)
            => new() { IsSuccess = false, Message = message, Errors = errors };

        public static Response Failure(Exception ex, string message = "Ha ocurrido un error al procesar la solicitud")
            => new() { IsSuccess = false, Message = message, Errors = new List<string> { ex.Message } };
    }

    public class Response<T> : Response
    {
        public T? Result { get; set; }

        public static Response<T> Success(T result, string message = "Tarea realizada con éxito")
            => new() { IsSuccess = true, Message = message, Result = result };

        public static new Response<T> Failure(string message, List<string>? errors = null)
            => new() { IsSuccess = false, Message = message, Errors = errors };

        public static new Response<T> Failure(Exception ex, string message = "Ha ocurrido un error al procesar la solicitud")
            => new() { IsSuccess = false, Message = message, Errors = new List<string> { ex.Message } };
    }
}