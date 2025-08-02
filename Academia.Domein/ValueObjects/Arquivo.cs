//Peterson Wiggers
using System;
using AcademiaDoZe.Domain.Exceptions;
public record Arquivo
{
    public byte[] Conteudo { get; }
    public string Tipo { get; }
    public Arquivo(byte[] conteudo,string tipo)
    {
        Conteudo = conteudo;
        Tipo = tipo;
    }

    public static Arquivo Criar(byte[] conteudo, string tipo) 
    {
        if (conteudo == null || conteudo.Length == 0)
            throw new DomainException("ARQUIVO_VAZIO");
        if (string.IsNullOrEmpty(tipo))
            throw new DomainException("ARQUIVO_TIPO_OBRIGATORIO");
        var tiposPermitidos = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".docx" };
        if (!tiposPermitidos.Contains(tipo.ToLower()))
            throw new DomainException("ARQUIVO_TIPO_INVALIDO");
        const int tamanhoMaximoBytes = 5 * 1024 * 1024; // 5MB
        if (conteudo.Length > tamanhoMaximoBytes)
            throw new DomainException("ARQUIVO_TIPO_TAMANHO");

        return new Arquivo(conteudo, tipo);
    }
}
