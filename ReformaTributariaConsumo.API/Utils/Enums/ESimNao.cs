using ReformaTributaria.API.Utils.Json.JsonConverter;
using System.Text.Json.Serialization;

namespace ReformaTributaria.API.Utils.Enums;

[JsonConverter(typeof(ESimNaoConverter))]
public enum ESimNao { Sim, Não }

[JsonConverter(typeof(ESNConverter))]
public enum ESN { S, N }
