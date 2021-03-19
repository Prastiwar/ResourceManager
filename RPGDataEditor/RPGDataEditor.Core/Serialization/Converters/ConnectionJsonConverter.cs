using RPGDataEditor.Core.Services;

namespace RPGDataEditor.Core.Serialization
{
    public class ConnectionJsonConverter : AbstractClassJsonConverter<IConnectionService>
    {
        public ConnectionJsonConverter() : base("RPGDataEditor.Core.Connection") { }

        protected override string Suffix => "Controller";
    }

}
