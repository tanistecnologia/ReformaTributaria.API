namespace ReformaTributaria.API.Utils
{
    public static class Cnpj
    {
        // Lista de CNPJ inválidos e da Tanis para acesso como master no APP
        private static readonly List<string> ListCnpj =
        [
            "01114682000126",
            "00000000000101",
            "00000000000102",
            "00000000000103",
            "00000000000104",
            "00000000000105",
        ];

        public static bool CnpjLiberado(string cnpj) => ListCnpj.Any(x => x.Equals(cnpj));
    }
}
