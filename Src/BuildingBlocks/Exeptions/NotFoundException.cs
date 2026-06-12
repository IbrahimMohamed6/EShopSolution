

using System.Runtime.Serialization;

namespace BuildingBlocks.Exeptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        : base("The requested resource was not found.") { }

        public NotFoundException(string message)
            : base(message) { }
        public NotFoundException(string name , object Key)
            : base($"Entity {name} with Key: {Key} was not found.") { }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        // Constructor for serialization support
        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
