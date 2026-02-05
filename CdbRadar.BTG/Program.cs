
// ==================================================
/*
 * 
taskkill /IM chrome.exe /F
 
"C:\Program Files\Google\Chrome\Application\chrome.exe" --remote-debugging-port=9222 --user-data-dir="C:\BTGProfile"

https://app.btgpactual.com/produtos-de-investimento/renda-fixa/credito-bancario


*/
// ==================================================

using CdbRadar.Domain.Model;
using CdbRadar.Repository.Abstractions;
using CdbRadar.Repository.Interfaces;
using CdbRadar.Repository.Repositories;
using CdbRadar.Utils.Services;
using CdbRadar.WebApi.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;

Console.WriteLine("===============================================");
Console.WriteLine("🚀 RADAR BTG - SCROLL INFINITO + SQL SERVER");
Console.WriteLine("===============================================\n");


// =====================================================
// ✅ 1. Carregar appsettings.json
// =====================================================

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

string connectionString =
    config.GetConnectionString("DefaultConnection")!;

// =====================================================
// ✅ 2. CONFIGURAR DI NO CONSOLE
// =====================================================

var services = new ServiceCollection();

services.AddSingleton<IConfiguration>(config);

// ✅ Factory no Console (pega connection string)
services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();

// ✅ Repository vem da Library
services.AddScoped<IOfertaRepository, OfertaRepository>();
services.AddScoped<ITipoAtivoRepository, TipoAtivoRepository>();

var provider = services.BuildServiceProvider();

// ✅ Resolver repository para uso
var repo = provider.GetRequiredService<IOfertaRepository>();
var repoTipo = provider.GetRequiredService<ITipoAtivoRepository>();

Console.WriteLine("✅ Banco configurado!\n");


// =====================================================
// ✅ 3. PLAYWRIGHT
// =====================================================
using var playwright = await Playwright.CreateAsync();

Console.WriteLine("🔌 Conectando ao Chrome Debug...");

var browser = await playwright.Chromium.ConnectOverCDPAsync(
    "http://127.0.0.1:9222"
);

var context = browser.Contexts[0];
var page = context.Pages[0];

Console.WriteLine("✅ Conectado!\n");


// =====================================================
// ✅ 4. ESPERAR A TABELA CARREGAR
// =====================================================

await page.WaitForSelectorAsync("tbody tr");

Console.WriteLine("✅ Página carregada!");
Console.WriteLine("📌 Fazendo scroll infinito...\n");


// =====================================================
// ✅ 5. SCROLL INFINITO REAL
// =====================================================

long lastHeight = 0;
int tentativas = 0;

while (tentativas < 8)
{
    long newHeight = await page.EvaluateAsync<long>(
        "document.body.scrollHeight"
    );

    Console.WriteLine($"Altura atual: {newHeight}");

    if (newHeight == lastHeight)
        tentativas++;
    else
        tentativas = 0;

    lastHeight = newHeight;

    // ✅ Scroll até o final
    await page.EvaluateAsync(
        "window.scrollTo(0, document.body.scrollHeight)"
    );

    await page.WaitForTimeoutAsync(2500);
}

Console.WriteLine("\n✅ Scroll completo! Coletando produtos...\n");


// =====================================================
// ✅ 6. COLETAR TODAS AS LINHAS
// =====================================================

var linhas = page.Locator("tbody tr");
int total = await linhas.CountAsync();

Console.WriteLine($"✅ Total produtos carregados: {total}\n");


// =====================================================
// ✅ 7. INSERIR NO BANCO VIA REPOSITORY (LIBRARY)
// =====================================================

for (int i = 0; i < total; i++)
{
    var linha = linhas.Nth(i);

    // ✅ Nome
    string nome = await linha
        .Locator("[data-testid='summary-title']")
        .InnerTextAsync();

    // ✅ Colunas
    var colunas = linha.Locator("td");

    string vencTexto = await colunas.Nth(1).InnerTextAsync();
    string taxaTexto = await colunas.Nth(2).InnerTextAsync();
    string minimoTexto = await colunas.Nth(4).InnerTextAsync();
    string liquiezTexto = await colunas.Nth(5).InnerTextAsync();

    // ✅ Detectar Mercado (Secundário ou Primário)
    string mercado = "Primário";

    var badges = linha.Locator("span.orq-badge__text");

    int qtdBadges = await badges.CountAsync();

    for (int b = 0; b < qtdBadges; b++)
    {
        string texto = (await badges.Nth(b).InnerTextAsync()).Trim();

        if (texto.Contains("secundário", StringComparison.OrdinalIgnoreCase))
        {
            mercado = "Secundário";
            break;
        }
    }

    // ✅ Detectar tipo
    string tipoNome = TipoHelper.Detectar(nome);

    int? tipoId = await repoTipo.ObterIdPorNomeAsync(tipoNome);

    if (tipoId == null)
    {
        Console.WriteLine($"⚠ Tipo não encontrado: {tipoNome}");
        continue;
    }

    // ✅ Criar objeto Oferta
    var oferta = new CdbOfertas
    {
        TipoId = tipoId.Value,
        Nome = nome,
        Emissor = nome,
        Vencimento = ParserHelper.ParseDate(vencTexto),
        TaxaDescricao = taxaTexto,
        TaxaValor = ParserHelper.ExtrairNumero(taxaTexto),
        Indexador = ParserHelper.DetectarIndexador(taxaTexto),
        MinimoAplicacao = ParserHelper.ParseMoney(minimoTexto),
        Liquidez = liquiezTexto,
        Mercado = mercado,
        Plataforma = "BTG"
    };

    // ✅ Inserção usando Repository da Library
    await repo.InserirAsync(oferta);

    Console.WriteLine($"✅ Inserido [{i + 1}/{total}]: {nome}");
}

Console.WriteLine("\n===============================================");
Console.WriteLine("✅ FINALIZADO! Tudo salvo no banco.");
Console.WriteLine("===============================================");
