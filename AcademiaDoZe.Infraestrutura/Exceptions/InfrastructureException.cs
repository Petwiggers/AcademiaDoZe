namespace AcademiaDoZe.Exceptions.Infrastructure
{
    // Classe base para exceções de infraestrutura
    public class InfrastructureException : Exception
    {
        public InfrastructureException(string message) : base(message)
        {
        }
        public InfrastructureException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}