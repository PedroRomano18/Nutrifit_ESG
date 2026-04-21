# Projeto - NutriFit ESG (Cidades ESG Inteligentes)

A aplicação **NutriFit** é voltada à nutrição e ao bem-estar, integrando Inteligência Artificial e profissionais da área da saúde para orientar, acompanhar e motivar os usuários na construção de hábitos alimentares e rotinas saudáveis.

**A problemática**: As pesquisas realizadas indicam que muitas pessoas enfrentam dificuldades para adotar hábitos saudáveis devido à rotina intensa, à falta de orientação especializada e ao desconhecimento dos benefícios proporcionados por práticas adequadas de alimentação e cuidado com a saúde. Esse cenário contribui para a insatisfação com o próprio corpo e para o surgimento de problemas de saúde que poderiam ser prevenidos por meio de acompanhamento adequado.

Como aprendizado com nosso MVP conceitual, evidenciamos a demanda real por uma solução que combine IA e acompanhamento profissional. Pessoas possuem falta de tempo e motivação contínua, necessitando de uma ferramenta personalizável e robusta.

## Como executar localmente com Docker

A aplicação foi orquestrada com `docker-compose`, que levanta simultaneamente a API C# e um banco de dados PostgreSQL. Para executar o projeto, siga os passos abaixo:

1. Certifique-se de ter o Docker e Docker Compose instalados.
2. Na raiz do projeto, execute o comando:
   ```bash
   docker-compose up --build
   ```
3. A aplicação estará disponível na porta mapeada (ex: `http://localhost:8080`).

## Pipeline CI/CD

Utilizamos o **GitHub Actions** (`.github/workflows/ci-cd.yml`) por sua forte integração e simplicidade nativa com repositórios.
Nosso pipeline funciona da seguinte maneira:

1. **Build e Tests**: A cada Push/PR na branch `main`, a Action de _Build & Test_ é acionada. Ela faz o setup do SDK do .NET 8, restaura as dependências, compila o projeto provando que o código está íntegro e roda a suíte de testes.
2. **Deploy Staging**: Uma vez que os testes passem, uma esteira sequencial aciona o deploy no ambiente de Staging (Homologação), garantindo que as mudanças fiquem disponíveis para testes de QA/usuários.
3. **Deploy Production**: Após o deploy em Staging finalizado com sucesso, é iniciada a implantação automatizada para o ambiente de Produção, disponibilizando a ferramenta para o usuário final.

## Containerização

O projeto utiliza um `Dockerfile` *multi-stage* robusto para a aplicação C#.

**Estratégias adotadas:**
- **Base image enxuta:** Utilização das imagens oficiais enxutas da Microsoft (`aspnet` para o runtime, `sdk` para o build).
- **Múltiplos Estágios (Multi-stage build):** O projeto primeiro restaura e realiza build na imagem SDK (maior e mais pesada). Em seguida, faz o publish. Finalmente, copia apenas os binários para a imagem de runtime (tamanho reduzido), aumentando segurança e performance.
- **Orquestração com Banco:** Usamos o `docker-compose.yml` para criar volumes e expor variáveis de ambiente ligadas de forma nativa a um banco `PostgreSQL`, usando redes Docker isoladas (`app-network`).

_Conteúdo do fluxo do Dockerfile (resumo):_
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Utilizando a imagem SDK para o ambiente de Compilação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
COPY ["src/Nutrifit_ESG.csproj", "src/"]
RUN dotnet restore "./src/Nutrifit_ESG.csproj"
COPY src/ src/
RUN dotnet build "./Nutrifit_ESG.csproj" -c Release -o /app/build
# Publicação enxuta e Setup Final
FROM build AS publish
RUN dotnet publish "./Nutrifit_ESG.csproj" -c Release -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nutrifit_ESG.dll"]
```


## Tecnologias utilizadas

- **Linguagem Principal:** C# / .NET 8 (API REST).
- **Banco de Dados:** PostgreSQL 15.
- **Containerização:** Docker e Docker Compose.
- **Integração e Entrega Contínuas (CI/CD):** GitHub Actions.

---

# Checklist de Entrega

Item | OK
--- | ---
Projeto compactado em .ZIP com estrutura organizada | [ X ]
Dockerfile funcional | [ X ]
docker-compose.yml ou arquivos Kubernetes | [ X ]
Pipeline com etapas de build, teste e deploy | [ X ]
README.md com instruções e prints | [ X ]
Documentação técnica com evidências (PDF ou PPT) | [ ] *A realizar pelo aluno*
Deploy realizado nos ambientes staging e produção | [ ] *A realizar pelo aluno/Prints ausentes*
