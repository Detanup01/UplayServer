using System.Text.Json.Serialization;

namespace upc_r1;


[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(UPC_Json.Root))]
internal partial class JsonSourceGen : JsonSerializerContext;