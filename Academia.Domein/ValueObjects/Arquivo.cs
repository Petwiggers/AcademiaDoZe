﻿//Peterson Wiggers
using AcademiaDoZe.Domain.Exceptions;

namespace AcademiaDoZe.Domain.ValueObjects;
public record Arquivo
{
    public byte[] Conteudo { get; }
    public string Tipo { get; }
    public Arquivo(byte[] conteudo)
    {
        Conteudo = conteudo;
    }

    public static Arquivo Criar(byte[] conteudo) 
    {
        if (conteudo == null || conteudo.Length < 0 ) throw new DomainException("ARQUIVO_VAZIO");

        const int tamanhoMaximoBytes = 5 * 1024 * 1024; // 5MB
        if (conteudo.Length > tamanhoMaximoBytes)
            throw new DomainException("ARQUIVO_TIPO_TAMANHO");

        return new Arquivo(conteudo);
    }
}
