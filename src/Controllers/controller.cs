[ApiController]
[Route("api/[controller]")]
public class RefeicoesController : ControllerBase
{
    private static List<Refeicao> lista = new();

    [HttpPost]
    public IActionResult Criar(Refeicao refeicao)
    {
        lista.Add(refeicao);
        return Ok(refeicao);
    }

    [HttpGet]
    public IActionResult Listar()
    {
        return Ok(lista);
    }

    [HttpGet("saudavel")]
    public IActionResult Saudaveis()
    {
        var saudaveis = lista.Where(r => r.Calorias < 500);
        return Ok(saudaveis);
    }
}