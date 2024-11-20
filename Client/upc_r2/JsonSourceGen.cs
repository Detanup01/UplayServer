using System.Text.Json.Serialization;
using static upc_r2.Exports.Other;

namespace upc_r2;


[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Callback))]
[JsonSerializable(typeof(UPC_Json.Root))]
[JsonSerializable(typeof(BasicList))]
[JsonSerializable(typeof(UPC_RichPresenceToken))]
[JsonSerializable(typeof(UPC_Product))]
[JsonSerializable(typeof(List<UPC_Product>))]
internal partial class JsonSourceGen : JsonSerializerContext;