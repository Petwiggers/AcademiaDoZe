using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Academia.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;

namespace AcademiaDoZe.Domain.Tests
{
    public class ColaboradorDomainTests
    {
        // Padrão AAA (Arrange, Act, Assert)
        // Arrange (Organizar): Preparamos tudo que o teste precisa.
        private Logradouro GetValidLogradouro() => Logradouro.Criar("12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");
        private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1], ".jpg");

        [Fact] // [Fact] é um atributo que marca este método como um teste para o xUnit.
        public void CriarColaborador_ComDadosValidos_DeveCriarObjeto() // Padrão de Nomenclatura: MetodoTestado_Cenario_ResultadoEsperado
        {
            // Arrange
            var nome = "João da Silva"; var cpf = "12345678901"; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var telefone = "11999999999";
            var email = "joao@email.com"; var logradouro = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = "Senha@1"; var foto = GetValidArquivo();
            var dataAdmissao = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var tipo = EColaboradorTipo.Instrutor; var vinculo = EColaboradorVinculo.CLT;
            // Act
            var colaborador = Colaborador.Criar(nome, cpf, dataNascimento, telefone, email, logradouro, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo);
            // Assert
            Assert.NotNull(colaborador);
        }

        [Fact]
        public void CriarColaborador_ComEmailInvalido_DeveLancarExcecao()
        {
            // Arrange
            var nome = "João da Silva"; var cpf = "12345678901"; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var telefone = "11999999999";
            var email = "joao@emailcom"; var logradouro = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = "Senha@1"; var foto = GetValidArquivo();
            var dataAdmissao = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var tipo = EColaboradorTipo.Instrutor; var vinculo = EColaboradorVinculo.CLT;
            // Act
            var ex = Assert.Throws<DomainException>(() => Colaborador.Criar(nome, cpf, dataNascimento, telefone, email, logradouro, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo));

            Assert.Equal("EMAIL_FORMATO", ex.Message);
        }

        [Fact]
        public void CriarColaborador_ComNomeVazio_DeveLancarExcecao()
        {
            // Arrange
            var cpf = "12345678901"; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var telefone = "11999999999";
            var email = "joao@email.com"; var logradouro = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = "Senha@1"; var foto = GetValidArquivo();
            var dataAdmissao = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var tipo = EColaboradorTipo.Instrutor; var vinculo = EColaboradorVinculo.CLT;
            // Act
            var ex = Assert.Throws<DomainException>(() =>
                Colaborador.Criar("", cpf, dataNascimento, telefone, email, logradouro, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo));

            // Assert
            Assert.Equal("NOME_OBRIGATORIO", ex.Message);
        }

        [Fact]
        public void CriarColaborador_VerificaNormalizacoes()
        {
            // Arrange
            var cpf = "1234 56  78901 "; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var telefone = " 11999 9   99999";
            var email = "joao@email.com"; var logradouro = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = " Se nha@1 2 3 "; var foto = GetValidArquivo();
            var dataAdmissao = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var tipo = EColaboradorTipo.Instrutor; var vinculo = EColaboradorVinculo.CLT;
            // Act & Assert
            var colaborador = Colaborador.Criar(
            "   Peterson  Wiggers   ",
            cpf,
            dataNascimento,
            telefone,
            email,
            logradouro,
            numero,
            complemento,
            senha,
            foto,
            dataAdmissao,
            tipo,
            vinculo
            );
            Assert.Equal("Peterson Wiggers", colaborador.Nome);
            Assert.Equal("11999999999", colaborador.Telefone);
            Assert.Equal("joao@email.com", colaborador.Email);
            Assert.Equal("123", colaborador.Numero);
            Assert.Equal("Senha@123", colaborador.Senha);
        }

    }
}
