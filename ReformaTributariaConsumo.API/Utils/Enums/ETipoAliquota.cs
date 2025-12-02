using System.Text.Json.Serialization;

namespace ReformaTributaria.API.Utils.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<ETipoAliquota>))]
public enum ETipoAliquota { Fixa, Padrão, SemAlíquota, UniformeNacionalReferência, UniformeSetorial };
