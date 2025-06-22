using System;
using System.Collections.Generic;
using System.Globalization;

class Produto
{
    public string Gtin;
    public string Descricao;
    public double PrecoVarejo;
    public double? PrecoAtacado; 
    public int? QtdeAtacado;     

    public Produto(string gtin, string descricao, double precoVarejo, double? precoAtacado = null, int? qtdeAtacado = null)
    {
        Gtin = gtin;
        Descricao = descricao;
        PrecoVarejo = precoVarejo;
        PrecoAtacado = precoAtacado;
        QtdeAtacado = qtdeAtacado;
    }
}

class RegistroVenda
{
    public string Gtin;
    public int Quantidade;
    public double Parcial;

    public RegistroVenda(string gtin, int quantidade, double parcial)
    {
        Gtin = gtin;
        Quantidade = quantidade;
        Parcial = parcial;
    }
}

class DescontoAtacado
{
    static void Main()
    {
        CultureInfo culture = CultureInfo.GetCultureInfo("pt-BR");

        // Catálogo de produtos
        var catalogo = new Dictionary<string, Produto>
        {
            { "7891024110348", new Produto("7891024110348", "SABONETE OLEO DE ARGAN 90G PALMOLIVE", 2.88, 2.51, 12) },
            { "7891048038017", new Produto("7891048038017", "CHÁ DE CAMOMILA DR.OETKER", 4.40, 4.37, 3) },
            { "7896066334509", new Produto("7896066334509", "TORRADA TRADICIONAL WICKBOLD", 5.19) },
            { "7891700203142", new Produto("7891700203142", "BEBIDA DE SOJA MAÇÃ ADES", 2.39, 2.38, 6) },
            { "7894321711263", new Produto("7894321711263", "ACHOCOLATADO TODDY", 9.79) },
            { "7896001250611", new Produto("7896001250611", "ADOÇANTE LINEA", 9.89, 9.10, 10) },
            { "7793306013029", new Produto("7793306013029", "CEREAL SUCRILHOS", 12.79, 12.35, 3) },
            { "7896004400914", new Produto("7896004400914", "COCO RALADO SOCOCO", 4.20, 4.05, 6) },
            { "7898080640017", new Produto("7898080640017", "LEITE UHT ITALAC", 6.99, 6.89, 12) },
            { "7891025301516", new Produto("7891025301516", "DANONINHO MORANGO", 12.99) },
            { "7891030003115", new Produto("7891030003115", "CREME DE LEITE MOCOCA", 3.12, 3.09, 4) },
        };

        var vendas = new List<RegistroVenda>
        {
            new RegistroVenda("7891048038017", 1, 4.40),
            new RegistroVenda("7896004400914", 4, 16.80),
            new RegistroVenda("7891030003115", 1, 3.12),
            new RegistroVenda("7891024110348", 6, 17.28),
            new RegistroVenda("7898080640017", 24, 167.76),
            new RegistroVenda("7896004400914", 8, 33.60),
            new RegistroVenda("7891700203142", 8, 19.12),
            new RegistroVenda("7891048038017", 1, 4.40),
            new RegistroVenda("7793306013029", 3, 38.37),
            new RegistroVenda("7896066334509", 2, 10.38)
        };

        var descontos = new Dictionary<string, double>();
        var totaisPorProduto = new Dictionary<string, int>();
        double subtotal = 0.0;

        foreach (var venda in vendas)
        {
            subtotal += venda.Parcial;

            if (!totaisPorProduto.ContainsKey(venda.Gtin))
                totaisPorProduto[venda.Gtin] = 0;

            totaisPorProduto[venda.Gtin] += venda.Quantidade;
        }

        foreach (var item in totaisPorProduto)
        {
            string gtin = item.Key;
            int quantidadeTotal = item.Value;
            Produto produto = catalogo[gtin];

            if (produto.PrecoAtacado.HasValue && produto.QtdeAtacado.HasValue)
            {
                int pacotes = quantidadeTotal / produto.QtdeAtacado.Value;
                int restantes = quantidadeTotal % produto.QtdeAtacado.Value;

                double totalVarejo = quantidadeTotal * produto.PrecoVarejo;
                double totalAtacado = pacotes * produto.QtdeAtacado.Value * produto.PrecoAtacado.Value + restantes * produto.PrecoVarejo;
                double desconto = totalVarejo - totalAtacado;

                if (desconto > 0.009) 
                    descontos[gtin] = Math.Round(desconto, 2);
            }
        }

        double totalDescontos = 0.0;
        Console.WriteLine("--- Desconto no Atacado ---\n");
        Console.WriteLine("Descontos:");

        foreach (var d in descontos)
        {
            Console.WriteLine($"{d.Key,-20} R$ {d.Value.ToString("0.00", culture)}");
            totalDescontos += d.Value;
        }

        Console.WriteLine();
        Console.WriteLine($"(+) Subtotal  =    R$ {subtotal.ToString("0.00", culture)}");
        Console.WriteLine($"(-) Descontos =      R$ {totalDescontos.ToString("0.00", culture)}");
        Console.WriteLine($"(=) Total     =    R$ {(subtotal - totalDescontos).ToString("0.00", culture)}");
    }
}
