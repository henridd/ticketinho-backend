using FluentValidation.Results;

namespace Ticketinho.Validation
{
    public class ValidableRequest<T>
    {
        public T Value { get; set; }
        public bool IsValid { get; set; }
        public IList<ValidationFailure> Errors { get; set; }
    }
}

