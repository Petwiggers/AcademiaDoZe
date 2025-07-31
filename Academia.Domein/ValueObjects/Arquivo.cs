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
        if (conteudo == null) throw new DomainException("CONTEUDO_OBRIGATORIO");
        if (string.IsNullOrEmpty(tipo)) throw new DomainException("TIPO_ARQUIVO_OBRIGATORIO");

        return new Arquivo(conteudo, tipo);  
    }
}
