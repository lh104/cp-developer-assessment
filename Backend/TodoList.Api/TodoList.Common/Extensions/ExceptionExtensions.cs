namespace TodoList.Common.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetErrorMessage( this Exception ex ) {
            string message;
            if ( ex.InnerException != null ) {
                message = $"{ex.Message}, {ex.InnerException.Message}";
            } else {
                message = $"{ex.Message}";
            }

            return message;
        }
    }
}
