using System;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2
{
    /// <summary>
    /// A reference to some other object within the asyncapi document. 
    /// </summary>
    public class Reference : ISchema
    {
        public Reference(string id, ReferenceType type)
        {
            _id = id ?? throw new ArgumentNullException(nameof(id));
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        private readonly string _id;
        private readonly ReferenceType _type;

        [JsonProperty("$ref")]
        public string Ref => _type.GetReferencePath(_id);
    }
    
    
    /// <summary>
    /// The type of a <see cref="Reference"/>. Determines where the reference will be located inside the asyncapi document.
    /// </summary>
    public class ReferenceType
    {
        public static readonly ReferenceType Schema = new ReferenceType(nameof(Schema), "#/components/schemas/{0}");
        
        public static readonly ReferenceType MessageTrait = new ReferenceType(nameof(MessageTrait), "#/components/messageTraits/{0}");
        
        public static readonly ReferenceType OperationTrait = new ReferenceType(nameof(OperationTrait), "#/operationTraits/{0}");

        private ReferenceType(string name, string format)
        {
            Name = name;
            _format = format;
        }

        private string _format;
        
        public string Name { get; }
        
        public string GetReferencePath(string id) => string.Format(_format, id);
    }


    /// <summary>
    /// A reference to an OperationTrait within the AsyncAPI components
    /// </summary>
    public class OperationTraitReference : Reference, IOperationTrait
    {
        public OperationTraitReference(string id) : base(id, ReferenceType.OperationTrait) { }
    }
    
    /// <summary>
    /// A reference to a MessageTrait within the AsyncAPI components
    /// </summary>
    public class MessageTraitReference : Reference, IMessageTrait
    {
        public MessageTraitReference(string id) : base(id, ReferenceType.MessageTrait) { }
    }
}