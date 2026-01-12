namespace Fatura.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando se viola una regla de negocio.
    /// </summary>
    public class BusinessRuleException : Exception
    {
        public string RuleName { get; }

        public BusinessRuleException(string message)
            : base(message)
        {
            RuleName = string.Empty;
        }

        public BusinessRuleException(string ruleName, string message)
            : base(message)
        {
            RuleName = ruleName;
        }

        public BusinessRuleException(string message, Exception innerException)
            : base(message, innerException)
        {
            RuleName = string.Empty;
        }

        public BusinessRuleException(string ruleName, string message, Exception innerException)
            : base(message, innerException)
        {
            RuleName = ruleName;
        }
    }
}
