//Peterson Wiggers
namespace Academia.Domain.Entities;
using AcademiaDoZe.Domain.Services;
using AcademiaDoZe.Domain.Exceptions;

public sealed class Logradouro : Entity
{
    // encapsulamento das propriedades, aplicando imutabilidade
    public string Cep { get; private set; }
    public string Nome { get; private set; }
    public string Bairro { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public string Pais { get; private set; }
    // construtor privado para evitar instância direta
    private Logradouro(int id, string cep, string nome, string bairro, string cidade, string estado, string pais) : base(id)
    {
        Id = id;
        Cep = cep;
        Nome = nome;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Pais = pais;
    }
    // método de fábrica, ponto de entrada para criar um objeto válido e normalizado
    public static Logradouro Criar(int id, string cep, string nome, string bairro, string cidade, string estado, string pais)
    {
        // Validações e normalizações

        if (string.IsNullOrWhiteSpace(cep)) throw new DomainException("CEP_OBRIGATORIO");

        cep = TextoNormalizadoService.LimparEDigitos(cep);
        if (cep.Length != 8) throw new DomainException("CEP_DIGITOS");
        if (string.IsNullOrWhiteSpace(nome)) throw new DomainException("NOME_OBRIGATORIO");
        nome = TextoNormalizadoService.LimparEspacos(nome);
        if (string.IsNullOrWhiteSpace(bairro)) throw new DomainException("BAIRRO_OBRIGATORIO");
        bairro = TextoNormalizadoService.LimparEspacos(bairro);
        if (string.IsNullOrWhiteSpace(cidade)) throw new DomainException("CIDADE_OBRIGATORIO");
        cidade = TextoNormalizadoService.LimparEspacos(cidade);
        if (string.IsNullOrWhiteSpace(estado)) throw new DomainException("ESTADO_OBRIGATORIO");
        estado = TextoNormalizadoService.ParaMaiusculo(TextoNormalizadoService.LimparTodosEspacos(estado));
        if (string.IsNullOrWhiteSpace(pais)) throw new DomainException("PAIS_OBRIGATORIO");
        pais = TextoNormalizadoService.LimparEspacos(pais);
        // criação e retorno do objeto

        return new Logradouro(id, cep, nome, bairro, cidade, estado, pais);
    }
}

