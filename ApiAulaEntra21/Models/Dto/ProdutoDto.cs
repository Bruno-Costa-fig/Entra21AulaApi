namespace ApiAulaEntra21.Models.Dto
{
    public class ProdutoDto
    {
        public string Nome { get; set; }
        public string Marca { get; set; }
        public int QuantidadeEstoque { get; set; }
        public int? LojaId { get; set; }
    }
}
