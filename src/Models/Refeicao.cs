namespace Nutrifit_ESG.Models;

public class Refeicao
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Calorias { get; set; }
    public DateTime Data { get; set; }
    public bool Sustentavel { get; set; }
}
